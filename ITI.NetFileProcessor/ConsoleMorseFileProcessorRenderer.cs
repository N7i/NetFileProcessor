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
            MorsePlayer player = new MorsePlayer(new MorseProcessor(15));
            StringBuilder pResult = new StringBuilder(256);

            pResult.AppendFormat("Found {0} files(s) with {1} hidden and {2} innacessible", processor.FileCount, processor.HiddenFileCount, processor.InaccesibleFileCount);
            pResult.AppendFormat(" and found {0} folder(s) with {1} hidden and {2} innacessible", processor.FileCount, processor.HiddenFileCount, processor.InaccesibleFileCount);

            player.Text = pResult.ToString().Trim();
            player.Play();

            Console.WriteLine();
            Console.WriteLine(pResult.ToString());
        }
    }
}
