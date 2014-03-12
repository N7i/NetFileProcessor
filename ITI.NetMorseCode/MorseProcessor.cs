using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.NetMorseCode
{
   
    public class MorseProcessor
    {
       #region statics
       private static readonly Dictionary<char, string> MorseTable = new Dictionary<char, string>()
       {
           {'A', ".-"},
           {'B', "-..."},
           {'C', "-.-."},
           {'D', "-.."},
           {'E', "."},
           {'F', "..-."},
           {'G', "--."},
           {'H', "...."},
           {'I', ".."},
           {'J', ".---"},
           {'K', "-.-"},
           {'L', ".-.."},
           {'M', "--"},
           {'N', "-."},
           {'O', "---"},
           {'P', ".--."},
           {'Q', "--.-"},
           {'R', ".-."},
           {'S', "..."},
           {'T', "."},
           {'U', "..-"},
           {'V', "...-"},
           {'W', ".--"},
           {'X', "-..-"},
           {'Y', "-.--"},
           {'Z', "--.."},
           {'1', ".----"},
           {'2', "..---"},
           {'3', "...--"},
           {'4', "....-"},
           {'5', "....."},
           {'6', "-...."},
           {'7', "--..."},
           {'8', "---.."},
           {'9', "----."},
           {'0', "-----"},
           {' ', "/"}
       };

        public static string ConvertToMorseCode(string text) {
            if (null == text || text.Length == 0) { return ""; }

            var upperText = text.ToUpper();
            var morseBuilder = new StringBuilder(upperText.Length * 4 + upperText.Length);
            
            foreach(char key in upperText.ToCharArray()) 
            {
                string morseVal;
                MorseTable.TryGetValue(key, out morseVal);
                if (null != morseVal)
                {
                    if (morseVal.Contains('/'))
                    {
                        morseBuilder.AppendFormat(" {0} ", morseVal);
                    }
                    else
                    {
                        morseBuilder.Append(morseVal);
                        morseBuilder.Append(' ');
                    }
                }
            }

            return morseBuilder.ToString();
        }

        public static string ConvertFromMorseCode(string morse) {
            if (null == morse || morse.Length == 0) { return ""; }

            var textBuilder = new StringBuilder(morse.Length / 4);
            var reverseMorseTable = MorseTable
                                    .GroupBy(p => p.Value)
                                    .ToDictionary(g => g.Key, g => g.Select(pp => pp.Key).FirstOrDefault());
            
            foreach (string morseVal in morse.Split(' '))
            {
                char charVal;
                reverseMorseTable.TryGetValue(morseVal, out charVal);
                if ('\0' != charVal)
                {
                    textBuilder.Append(charVal);
                }
            }
            return textBuilder.ToString();
        }
        #endregion

        private int pulseDurationInMSeconds { get; set; }
        public MorseProcessor() : this(13) { }
        public MorseProcessor(int wordPerMinutes) : this(wordPerMinutes, MorseStandard.PARIS) { }
        public MorseProcessor(int wordPerMinutes, MorseStandard standard)
        {
            if (wordPerMinutes <= 0) { throw new ArgumentOutOfRangeException("wordPerMinutes", "Should be superior at 0"); }
            pulseDurationInMSeconds = 60000 / ((int)standard * wordPerMinutes);
        }

        public List<MorsePulsation> GetPulseFromText(string text)
        {
            if (null == text) { throw new ArgumentNullException("text", "Should not be null");  }
            return GetPulseFromMorse(ConvertToMorseCode(text));
        }

        public List<MorsePulsation> GetPulseFromMorse(string morse)
        {
            if (null == morse) { throw new ArgumentNullException("morse", "Should not be null"); }
            List<MorsePulsation> pulsations = new List<MorsePulsation>();

            foreach (string word in morse.Split('/'))
            {
                foreach (string letter in word.Split(' '))
                {
                    char lastChar = ' ';
                    foreach (char c in letter)
                    {
                        switch (c)
                        {
                            case '.':
                                pulsations.Add(new MorsePulsation(PulsationToMSeconds(1), PulsationType.EMISSION));
                                break;
                            case '-':
                                if (lastChar == '.')
                                {
                                    pulsations.Add(new MorsePulsation(PulsationToMSeconds(1), PulsationType.PAUSE));
                                }
                                pulsations.Add(new MorsePulsation(PulsationToMSeconds(3), PulsationType.EMISSION));
                                break;
                        }
                        lastChar = c;
                    }
                    pulsations.Add(new MorsePulsation(PulsationToMSeconds(3), PulsationType.PAUSE));
                }
                // TODO Fix
                // NXT Should be 7 instead of 1 pulse units at the end of an word, but till we use an ugly thread.sleep in the morse player, the pause beat is too slow
                pulsations.Add(new MorsePulsation(PulsationToMSeconds(1), PulsationType.PAUSE));
            }
            return pulsations;
        }

        private int PulsationToMSeconds(int pCount)
        {
            return pCount * pulseDurationInMSeconds;
        }
    }
}
