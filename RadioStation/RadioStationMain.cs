using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace RadioStation
{
    public class RadioStationMain:IDisposable
    {
        #region Private Members
        
        private IWavePlayer player;
        private AudioFileReader aReader;
        private RandomFileSelector randomFileSelector;
        private ISampleProvider volumeMeter;
        private object _finishLock = new object();
        private bool _finished = false;
        
        #endregion

        public string CurrentPlayingFileName;

        public bool Finished
        {
            get { lock (_finishLock) return _finished; }
            set { lock (_finishLock) _finished = value; }
        }


        public RadioStationMain(string baseFolder)
        {
            if (player == null)
            {
                player = new WaveOut();
                player.PlaybackStopped += OnPlaybackStopped;
            }
            randomFileSelector = new RandomFileSelector(baseFolder);
            CurrentPlayingFileName = LoadRandomFile();
        }

        public void LoadMp3File(string fileName)
        {
            if (aReader != null)
                aReader.Dispose();

            aReader = new AudioFileReader(fileName);
            var sampleChannel = new SampleChannel(aReader, true);
            volumeMeter = new MeteringSampleProvider(sampleChannel);

            player.Init(volumeMeter);
        }

        public string LoadRandomFile()
        {
            while (true)
            {
                try
                {
                    var fileName = randomFileSelector.NextFile();
                    LoadMp3File(fileName);
                    return fileName;
                }
                catch
                {

                }
            }
        }

        public void Play()
        {
            if (player.PlaybackState != PlaybackState.Playing)
                player.Play();
        }

        public void Stop()
        {
            if (player.PlaybackState != PlaybackState.Stopped)
            {
                player.Stop();
                aReader.Position = 0;
            }
        }

        public void Pause()
        {
            if (player.PlaybackState == PlaybackState.Playing)
                player.Pause();
            else if (player.PlaybackState == PlaybackState.Paused)
                player.Play();
        }

        public void Next()
        {
            Stop();
            CurrentPlayingFileName = LoadRandomFile();
            Play();
        }

        public void IncreaseVolume()
        {
            var targetVolume = (player as WaveOut).Volume * 1.1f;
            if (targetVolume > 1.0f)
                targetVolume = 1.0f;

            (player as WaveOut).Volume = targetVolume;
        }

        public void DecreaseVolume()
        {
            var targetVolume = (player as WaveOut).Volume / 1.1f;
            if (targetVolume < 0.001f)
                targetVolume = 0.001f;

            (player as WaveOut).Volume = targetVolume;
        }

        public void Dispose()
        {
            if (aReader != null)
                aReader.Dispose();

            if (player != null)
                player.Dispose();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            if (aReader.Position == aReader.Length)
            {
                Finished = true;
            }
        }

    }
}
