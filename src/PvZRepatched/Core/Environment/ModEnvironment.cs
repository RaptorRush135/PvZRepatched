namespace PvZRepatched.Environment;

using MelonLoader.Utils;

public sealed class ModEnvironment
{
    public ModEnvironment(string modName)
    {
        ArgumentNullException.ThrowIfNull(modName);
        this.ModName = modName;
    }

    public string ModName { get; }

    public DirectoryInfo ModUserDataDirectory
        => field ??= GetDirectoryInternal(MelonEnvironment.UserDataDirectory, this.ModName);

    public DirectoryInfo GetDirectory(bool ensureCreated, params IEnumerable<string> paths)
    {
        var directory = GetDirectoryInternal([this.ModUserDataDirectory.FullName, .. paths]);
        if (ensureCreated)
        {
            directory.Create();
        }

        return directory;
    }

    private static DirectoryInfo GetDirectoryInternal(params IEnumerable<string> paths)
    {
        string path = Path.Join([.. paths]);
        return new DirectoryInfo(path);
    }
}
