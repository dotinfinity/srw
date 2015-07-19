using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;

namespace RadioStation.Analyzer
{
    internal class Utils
    {
        public static AudioFileReader LoadFile(string fileName)
        {
            return new AudioFileReader(fileName);
        }
    }
}
