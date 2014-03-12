using System;
using System.IO;

namespace ITI.NetFileProcessor
{
    public class FileProcessor
    {
        IFileProcessorRenderer _rendererProvider;
        DirectoryInfo _startDirectory;

        #region attributes
        public int FileCount { get; private set; }
        public int HiddenFileCount { get; private set; }
        public int InaccesibleFileCount { get; private set; }
        public int DirectoryCount { get; private set; }
        public int HiddenDirectoryCount { get; private set; }
        public int InaccesibleDirectoryCount { get; private set; }
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

        public void Process(string path)
        {
            if (File.Exists(path))
            {
                ProcessFile(new FileInfo(path));
            }
            else if (Directory.Exists(path))
            {
                // save init start Directory
                if (null == _startDirectory)
                {
                    _startDirectory = new DirectoryInfo(path);
                }

                string[] directories = Directory.GetDirectories(path);
                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    ProcessFile(new FileInfo(file), new DirectoryInfo(path));
                }

                foreach (string directory in directories)
                {
                    ProcessDirectory(new DirectoryInfo(directory));
                    Process(directory);
                }
            }
        }

        #region private methods
        private void ProcessFile(FileInfo file, DirectoryInfo parent)
        {
            if (!isHidden(file) && hasAnHiddenParent(parent))
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

        private void ProcessDirectory(DirectoryInfo directory)
        {
            DirectoryCount += 1;
            if (isHidden(directory))
            {
                HiddenDirectoryCount += 1;
            }
            else if (hasAnHiddenParent(directory))
            {
                InaccesibleDirectoryCount += 1;
            }
        }

        private bool hasAnHiddenParent(DirectoryInfo directory)
        {
            if (null != directory && directory.FullName != _startDirectory.FullName)
            {
                if (isHidden(directory))
                {
                    return true;
                }
                else
                {
                    return hasAnHiddenParent(directory.Parent);
                }
            }
            else
            {
                return false;
            }
        }

        private bool isHidden(FileSystemInfo file)
        {
            return file.Attributes.HasFlag(FileAttributes.Hidden);
        }
        #endregion
    }
}
