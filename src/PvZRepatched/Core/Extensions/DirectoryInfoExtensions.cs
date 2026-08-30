namespace PvZRepatched.Extensions;

public static class DirectoryInfoExtensions
{
    public static FileInfo GetFile(this DirectoryInfo directory, string relativePath)
        => new(Path.Join(directory.FullName, relativePath));
}
