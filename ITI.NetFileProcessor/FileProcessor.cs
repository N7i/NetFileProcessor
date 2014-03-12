using System;
using System.Collections.Generic;
using System.IO;

namespace ITI.NetFileProcessor
{
    public class FileProcessor
    {
        IFileProcessorRenderer _rendererProvider;
        DirectoryResult _directoryResult;

        public DirectoryResult Result { get { return _directoryResult;  } }

        #region constructor
        public FileProcessor() : this(new ConsoleFileProcessorRenderer()) { } 

        public FileProcessor(IFileProcessorRenderer rendererProvider)
        {
            _rendererProvider = rendererProvider;
            _directoryResult = new DirectoryResult();
        }
        #endregion

        public void ShowResults()
        {
            _rendererProvider.render(this);
        }

        public DirectoryResult Process(string path)
        {
            _directoryResult.RootDirectory = new DirectoryInfo(path);
            if (_directoryResult.RootDirectory.Exists)
            {
                Process(path, isHidden(_directoryResult.RootDirectory));
            }
            return _directoryResult;
        }

        #region private methods
        private void Process(string path, bool hasParentHidden)
        {
            if (File.Exists(path))
            {
                ProcessFile(path, hasParentHidden);
            }
            else if (Directory.Exists(path))
            {
                hasParentHidden = hasParentHidden ? hasParentHidden : isHidden(path);

                foreach (string file in SafeFileEnumerator.EnumerateFiles(path))
                {
                    ProcessFile(file, hasParentHidden);
                }

                foreach (string directory in SafeFileEnumerator.EnumerateDirectories(path))
                {
                    ProcessDirectory(directory, hasParentHidden);
                }
            }
        }

        private void ProcessFile(string path, bool hasParentHidden)
        {
            FileInfo file = new FileInfo(path);

            if (isHidden(file))
            {
                _directoryResult.HiddenFileCount += 1;
            }

            if (isHidden(file) || hasParentHidden)
            {
                _directoryResult.InaccesibleFileCount += 1;
            }
            _directoryResult.FileCount += 1;
        }

        private void ProcessDirectory(string directoryPath, bool hasParentHidden)
        {
            DirectoryInfo directory = new DirectoryInfo(directoryPath);

            if (isHidden(directory))
            {
                _directoryResult.HiddenDirectoryCount += 1;
            }

            if (isHidden(directory) || hasParentHidden)
            {
                _directoryResult.InaccesibleDirectoryCount += 1;
            }

            _directoryResult.DirectoryCount += 1;
            Process(directory.FullName, hasParentHidden);
        }

        private bool isHidden(string directoryPath)
        {
            return isHidden(new DirectoryInfo(directoryPath));
        }

        private bool isHidden(FileSystemInfo file)
        {
            //return file.Attributes.HasFlag(FileAttributes.Hidden);
            var mask = FileAttributes.Hidden;
            return (file.Attributes & mask) != 0;
        }
        #endregion
    }
}
