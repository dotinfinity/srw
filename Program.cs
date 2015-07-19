using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Configuration;
using RadioStation;

namespace RadioStationConsole
{
    class Program
    {
        static RadioStation.RadioStationMain radio;
        static string baseFolder;

        static void Main(string[] args)
        {
            bool startPlaying = args.Any() && Directory.Exists(args[0]);
            if (startPlaying)
            {
                baseFolder = args[0];
            }
            else
            {
                baseFolder = ConfigurationManager.AppSettings["BaseFolder"];
            }
            radio = new RadioStation.RadioStationMain(baseFolder);

            Console.WriteLine("Test Radio");
            Console.WriteLine("[p] - Play; [s] - Stop; [a] - Pause; [n] - Next; [1] - Volume Up; [2] - Volume Down[x] - Exit");
            Console.WriteLine();

            var cancelToken = new CancellationTokenSource();
            var monitorThread = Task.Factory.StartNew(() => MonitorCompletion(cancelToken.Token), cancelToken.Token);
            
            if (startPlaying)
            {
                radio.Play();
                WriteStatusToConsole();
            }

            var continuePlaying = true;
            while (continuePlaying)
            {
                var input = Console.ReadKey(true);
                switch (input.KeyChar)
                {
                    case 'p': 
                        radio.Play();
                        WriteStatusToConsole();
                        break;
                    case 's':
                        radio.Stop();
                        WriteStatusToConsole("Stopped.");
                        break;
                    case 'a': 
                        radio.Pause(); 
                        WriteStatusToConsole("Paused.");
                        break;
                    case 'x': 
                        continuePlaying = false; 
                        break;
                    case 'n':
                        radio.Next();
                        WriteStatusToConsole();
                        break;
                    case '1':
                        radio.IncreaseVolume();
                        break;
                    case '2':
                        radio.DecreaseVolume();
                        break;
                    default:
                        break;
                }
            }

            cancelToken.Cancel();
            monitorThread.Wait();
            radio.Stop();   
        }

        private static void WriteStatusToConsole(string message = null)
        {
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.Write(new string(' ', Console.BufferWidth));
            if (message == null)
            {
                message = string.Format("Playing: {0}", Path.GetFileName(radio.CurrentPlayingFileName));
            }
            Console.SetCursorPosition(0, Console.CursorTop - 1);
            Console.WriteLine(message.Substring(0, Math.Min(message.Length, Console.BufferWidth)));
        }

        static void MonitorCompletion(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (radio.Finished)
                {
                    radio.Next();
                    WriteStatusToConsole();
                    radio.Finished = false;
                }

                Thread.Sleep(100);
            }
        }
    }
}
