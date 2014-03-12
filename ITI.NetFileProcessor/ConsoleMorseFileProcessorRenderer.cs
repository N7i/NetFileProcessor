using ITI.NetMorseCode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ITI.NetFileProcessor
{
    public class ConsoleMorseFileProcessorRenderer : IFileProcessorRenderer
    {
        public void render(FileProcessor processor)
        {
            MorsePlayer player = new MorsePlayer(new MorseProcessor());
            StringBuilder pResult = new StringBuilder(256);

            pResult.AppendFormat("Found {0} files(s) with {1} hidden and {2} inacessible", processor.Result.FileCount, processor.Result.HiddenFileCount, processor.Result.InaccesibleFileCount);
            pResult.AppendFormat(" and found {0} folder(s) with {1} hidden and {2} inacessible", processor.Result.DirectoryCount, processor.Result.HiddenDirectoryCount, processor.Result.InaccesibleDirectoryCount);

            player.Text = pResult.ToString().Trim();
            player.Play();

            Console.WriteLine();
            Console.WriteLine(pResult.ToString());
        }
    }
}
