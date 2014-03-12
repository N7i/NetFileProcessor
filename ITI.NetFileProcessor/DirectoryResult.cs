using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.NetFileProcessor
{
    public class DirectoryResult
    {
        internal DirectoryResult() { }
        public int FileCount { get; internal set; }
        public int HiddenFileCount { get; internal set; }
        public int InaccesibleFileCount { get; internal set; }
        public int DirectoryCount { get; internal set; }
        public int HiddenDirectoryCount { get; internal set; }
        public int InaccesibleDirectoryCount { get; internal set; }
        public DirectoryInfo RootDirectory { get; internal set; }
    }
}
