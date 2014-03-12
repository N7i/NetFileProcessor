using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using ITI.NetFileProcessor;
using ITI.NetMorseCode;

namespace EatMyShort
{

    class Program
    {
        static ConsoleColor getRandomColor()
        {
            Random rand = new Random();
            string[] colorNames = ConsoleColor.GetNames(typeof(ConsoleColor));
            int numColors = colorNames.Length;
            string colorName = colorNames[rand.Next(numColors)];

            return (ConsoleColor)Enum.Parse(typeof(ConsoleColor), colorName);
        }

        static void Main(string[] args)
        {
            Console.Title = "My Rainbow Beep";
            FileProcessor p = new FileProcessor(new ConsoleMorseFileProcessorRenderer());

            p.Process(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures));
            p.ShowResults();

            char key = 'c';
            do
            {
                key = Console.ReadKey().KeyChar;
                Random rnd = new Random();

                Console.BackgroundColor = getRandomColor();
                Console.ForegroundColor = getRandomColor();
                Console.CursorSize = rnd.Next(10, 25);
                Console.SetCursorPosition(rnd.Next(1, 80), rnd.Next(1, 60));
            }
            while (key != 'q');
        }

        static void Walk(string directoryPath, Func<FileInfo, FileInfo> fileHandler) {
            
        }
    }
}
