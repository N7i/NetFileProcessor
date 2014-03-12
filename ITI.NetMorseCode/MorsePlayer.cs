using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.NetMorseCode
{
    public class MorsePlayer
    {
        MorseProcessor _processor;
        string Morse { get; set; }

        public string Text { get; set; }

        public MorsePlayer(MorseProcessor processor)
        {
            _processor = processor;
            Text = "sos";
        }

        public void Play()
        {
            Morse = MorseProcessor.ConvertToMorseCode(Text);
            int idx = 0;

            foreach (MorsePulsation pulse in _processor.GetPulseFromMorse(Morse))
            {
                if (PulsationType.PAUSE == pulse.Type)
                {
                    while (idx < Morse.Length && (Morse[idx] == ' ' || Morse[idx] == '/'))
                    {
                        Console.Write(Morse[idx]);
                        ++idx;
                    }
                    System.Threading.Thread.Sleep(pulse.DurationInMSeconds);
                }
                else
                {
                    Console.Write(Morse[idx]);
                    Console.Beep(700, pulse.DurationInMSeconds);
                    ++idx;
                }
            }
        }
    }
}
