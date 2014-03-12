using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITI.NetFileProcessor
{
    public class ConsoleFileProcessorRenderer : IFileProcessorRenderer
    {
        public void render(FileProcessor processor)
        {
            Console.WriteLine("Found {0} files(s) with {1} hidden and {2} innacessible", processor.FileCount, processor.HiddenFileCount, processor.InaccesibleFileCount);
            Console.WriteLine("Found {0} folder(s) with {1} hidden and {2} innacessible", processor.DirectoryCount, processor.HiddenDirectoryCount, processor.InaccesibleDirectoryCount);
        }
    }
}
