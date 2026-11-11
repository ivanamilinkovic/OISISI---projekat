namespace SajamKnjiga.Core.Utils
{
    public static class DataPaths
    {
        private const string DataFolderName = "Data";
        private static string? _dataDirectory;
        public static string DataDirectory
        {
            get
            {
                if (_dataDirectory == null)
                    _dataDirectory = FindDataDirectory();
                return _dataDirectory;
            }
        }

        public static string GetFilePath(string fileName)
        {
            return DataDirectory + Path.DirectorySeparatorChar + fileName;
        }

        private static string FindDataDirectory()
        {
            DirectoryInfo? dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = dir.FullName + Path.DirectorySeparatorChar + DataFolderName;
                if (Directory.Exists(candidate))
                    return candidate;
                dir = dir.Parent;
            }

            string fallback = AppContext.BaseDirectory + DataFolderName;
            Directory.CreateDirectory(fallback);
            return fallback;
        }
    }
}