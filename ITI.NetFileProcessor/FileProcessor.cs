using System;
using System.Collections.Generic;
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

        public void Process(string path)
        {
            RootDirectory = new DirectoryInfo(path);
            if (RootDirectory.Exists)
            {
                Process(path, isHidden(RootDirectory));
            }
        }

        #region private methods
        private void Process(string path, bool isInHiddenFolder)
        {
            if (File.Exists(path))
            {
                ProcessFile(new FileInfo(path), isInHiddenFolder);
            }
            else if (Directory.Exists(path))
            {
                IEnumerator<string> files = Directory.EnumerateFiles(path).GetEnumerator();
                IEnumerator<string> directories = Directory.EnumerateDirectories(path).GetEnumerator();
                isInHiddenFolder = isInHiddenFolder ? isInHiddenFolder : isHidden(path);
                
                while (files.MoveNext())
                {
                    ProcessFile(new FileInfo(files.Current), isInHiddenFolder);
                }

                while (directories.MoveNext())
                {
                    ProcessDirectory(new DirectoryInfo(directories.Current), isInHiddenFolder);
                }
            }
        }
        
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
            Process(directory.FullName, isInHiddenFolder);
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
