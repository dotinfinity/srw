using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.CoreAudioApi;
using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace RadioStation.Analyzer
{
    public class Analyzer
    {
        private AudioFileReader _reader;

        public Analyzer(string fileName)
        {
            _reader = Utils.LoadFile(fileName);
        }

        public void GetBpm()
        {
            var channel = new SampleChannel(_reader);
            
        }
    }
}
