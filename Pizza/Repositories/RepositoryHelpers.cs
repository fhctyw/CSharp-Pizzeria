namespace Pizza;
public static class RepositoryHelpers
{
    public static string FolderName = "db";
    public static string GetFilePath(string fileName) => Path.Combine(FolderName, fileName);
}