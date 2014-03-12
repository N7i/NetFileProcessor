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
            Console.WriteLine("Found {0} files(s) with {1} hidden and {2} inacessible", processor.Result.FileCount, processor.Result.HiddenFileCount, processor.Result.InaccesibleFileCount);
            Console.WriteLine("Found {0} folder(s) with {1} hidden and {2} inacessible", processor.Result.DirectoryCount, processor.Result.HiddenDirectoryCount, processor.Result.InaccesibleDirectoryCount);
        }
    }
}
