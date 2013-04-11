using System;
using System.IO;

using ScriptCs.Wpf.Interfaces;

namespace ScriptCs.Wpf
{
    public class FileSystem : IFileSystem
    {
        public string ReadAllText(string path)
        {
            return File.ReadAllText(path);
        }

        public string CurrentDirectory
        {
            get { return Environment.CurrentDirectory; }
        }

        public bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }
    }
}