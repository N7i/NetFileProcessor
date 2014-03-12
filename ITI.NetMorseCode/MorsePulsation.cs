using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITI.NetMorseCode
{
    public class MorsePulsation
    {
        public int DurationInMSeconds { get; private set; }
        public PulsationType Type { get; private set; }

        public MorsePulsation(int durationInSecond, PulsationType type)
        {
            DurationInMSeconds = durationInSecond;
            Type = type;
        }
    }
}
