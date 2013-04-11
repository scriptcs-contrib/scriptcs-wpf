namespace ScriptCs.Wpf.Interfaces
{
    public interface IFileSystem
    {
        string ReadAllText(string path);

        string CurrentDirectory { get; }

        bool FileExists(string filePath);
    }
}