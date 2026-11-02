namespace PvZRepatched.Fixes;

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using Il2CppInterop.Runtime.Runtime;

using Il2CppReloaded.Gameplay;

using MelonLoader;

using PvZRepatched.Extensions;
using PvZRepatched.Hooks;

internal sealed class CobCannonCoinFix : IDisposable
{
    private readonly Il2CppHook<MouseHitTestDelegate> hook;

    private CobCannonCoinFix()
    {
        this.hook = this.CreateHook();
    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate byte MouseHitTestDelegate(
        IntPtr instance,
        int x,
        int y,
        IntPtr theHitResult,
        int playerIndex,
        IntPtr methodInfo);

    public static IDisposable Apply()
    {
        var patch = new CobCannonCoinFix();
        patch.hook.Attach();
        return patch;
    }

    public void Dispose()
    {
        this.hook.Dispose();
    }

    private Il2CppHook<MouseHitTestDelegate> CreateHook()
    {
        MethodInfo targetMethod = typeof(Board).GetMethodOrThrow(
            nameof(Board.MouseHitTest),
            typeof(int),
            typeof(int),
            typeof(HitResult).MakeByRefType(),
            typeof(int));

        return Il2CppHook<MouseHitTestDelegate>.Create(targetMethod, this.MouseHitTestDetour);
    }

    private byte MouseHitTestDetour(
        IntPtr instance,
        int x,
        int y,
        IntPtr theHitResult,
        int playerIndex,
        IntPtr methodInfo)
    {
        const byte NoHit = 0;
        byte result = this.hook.Trampoline.Invoke(instance, x, y, theHitResult, playerIndex, methodInfo);
        if (result == NoHit)
        {
            return result;
        }

        try
        {
            var hit = GetHitResultInstance(theHitResult);
            if (hit.ObjectType != ReloadedObjectType.Coin)
            {
                return result;
            }

            var cursor = new Board(instance).CursorObjects?[playerIndex];
            if (cursor?.mCursorType != CursorType.CobCannonTarget)
            {
                return result;
            }

            hit.Object = null;
            hit.ObjectType = ReloadedObjectType.None;
            return NoHit;
        }
        catch (Exception ex)
        {
            Melon<Core>.Logger.Error($"Error in {nameof(this.MouseHitTestDetour)}", ex);
        }

        return result;

        static HitResult GetHitResultInstance(IntPtr hitResult)
            => new(hitResult - Unsafe.SizeOf<Il2CppObject>());
    }
}
