using System;
using System.IO;

namespace ITI.NetFileProcessor
{
    public class FileProcessor
    {
        IFileProcessorRenderer _rendererProvider;
        
        #region attributes
        public int FileCount { get; private set; }
        public int HiddenFileCount { get; private set; }
        public int InaccesibleFileCount { get; private set; }
        public int DirectoryCount { get; private set; }
        public int HiddenDirectoryCount { get; private set; }
        public int InaccesibleDirectoryCount { get; private set; }
        public DirectoryInfo RootDirectory { get; private set; }
        #endregion

        #region constructor
        public FileProcessor() : this(new ConsoleFileProcessorRenderer()) { } 

        public FileProcessor(IFileProcessorRenderer rendererProvider)
        {
            _rendererProvider = rendererProvider;
        }
        #endregion

        public void ShowResults()
        {
            _rendererProvider.render(this);
        }

        public void Process(string path, bool isInHiddenFolder = false)
        {
            if (File.Exists(path))
            {
                ProcessFile(new FileInfo(path));
            }
            else if (Directory.Exists(path))
            {
                if (null == RootDirectory)
                {
                    RootDirectory = new DirectoryInfo(path);
                }

                isInHiddenFolder = isInHiddenFolder ? isInHiddenFolder : isHidden(path);

                string[] directories = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    ProcessFile(new FileInfo(file), isInHiddenFolder);
                }

                foreach (string directory in directories)
                {
                    ProcessDirectory(new DirectoryInfo(directory), isInHiddenFolder);
                    Process(directory, isInHiddenFolder);
                }
            }
        }

        #region private methods
        private void ProcessFile(FileInfo file, bool isInHiddenFolder)
        {
            if (!isHidden(file) && isInHiddenFolder)
            {
                InaccesibleFileCount += 1;
            }
            ProcessFile(file);
        }

        private void ProcessFile(FileInfo file)
        {
            FileCount += 1;
            if (isHidden(file))
            {
                HiddenFileCount += 1;
            }
        }

        private void ProcessDirectory(DirectoryInfo directory, bool isInHiddenFolder)
        {
            DirectoryCount += 1;
            if (isHidden(directory))
            {
                HiddenDirectoryCount += 1;
            }
            else if (isInHiddenFolder)
            {
                InaccesibleDirectoryCount += 1;
            }
        }

        private bool isHidden(string directoryPath)
        {
            return isHidden(new DirectoryInfo(directoryPath));
        }

        private bool isHidden(FileSystemInfo file)
        {
            return file.Attributes.HasFlag(FileAttributes.Hidden);
        }
        #endregion
    }
}
