namespace PvZRepatched.Fixes;

#pragma warning disable SA1313 // Parameter names should begin with lower-case letter

using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Text;

using HarmonyLib;

using Il2CppSteamworks;

// Workaround for https://github.com/Facepunch/Facepunch.Steamworks/issues/802
[HarmonyPatch]
internal static class SteamInitFix
{
    [DllImport(
        "steam_api64",
        EntryPoint = nameof(SteamInternal_SteamAPI_Init),
        CallingConvention = CallingConvention.Cdecl)]
    private static extern SteamAPIInitResult SteamInternal_SteamAPI_Init(
        IntPtr pszInternalCheckInterfaceVersions,
        IntPtr pOutErrMsg);

    [HarmonyPrefix]
    [HarmonyPatch(typeof(SteamAPI), nameof(SteamAPI.Init))]
    private static bool InitPrefix(
        ref SteamAPIInitResult __result,
        string pszInternalCheckInterfaceVersions,
        out string pOutErrMsg)
    {
        __result = Init(pszInternalCheckInterfaceVersions, out pOutErrMsg);
        return false;
    }

    private static SteamAPIInitResult Init(string pszInternalCheckInterfaceVersions, out string pOutErrMsg)
    {
        using var interfaceVersionsStr = new Utf8StringToNative(pszInternalCheckInterfaceVersions);
        var buffer = Helpers.Memory.Take();

        try
        {
            var result = SteamInternal_SteamAPI_Init(interfaceVersionsStr.Pointer, buffer.Ptr);
            pOutErrMsg = Helpers.MemoryToString(buffer.Ptr);
            return result;
        }
        finally
        {
            buffer.Dispose();
        }
    }

    // https://github.com/Facepunch/Facepunch.Steamworks/blob/2.5.0/Facepunch.Steamworks/Utility/Utf8String.cs
    private struct Utf8StringToNative : IDisposable
    {
        private static readonly Encoding Utf8NoBom = new UTF8Encoding(false, false);

        [SuppressMessage(
            "Major Vulnerability",
            "S6640:Unsafe code blocks should not be used",
            Justification = "Required to write UTF-8 data directly to the unmanaged buffer.")]
        public unsafe Utf8StringToNative(string value)
        {
            if (value == null)
            {
                this.Pointer = IntPtr.Zero;
                return;
            }

            int byteCount = Utf8NoBom.GetByteCount(value) + 1;
            IntPtr buffer = Marshal.AllocHGlobal(byteCount);

            fixed (char* strPtr = value)
            {
                var destination = (byte*)buffer;
                int written = Utf8NoBom.GetBytes(strPtr, value.Length, destination, byteCount);
                destination[written] = 0;
            }

            this.Pointer = buffer;
        }

        public IntPtr Pointer { get; private set; }

        public void Dispose()
        {
            if (this.Pointer != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(this.Pointer);
                this.Pointer = IntPtr.Zero;
            }
        }
    }
}
