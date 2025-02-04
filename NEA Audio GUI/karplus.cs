using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
namespace NEA_Audio_GUI
{
    internal class karplus
    {
        


        public class Decay
        {
            public void karplus(int sampleRate = 48000, double frequency = 5000d, double amplitude = 1d, double seconds = 5d)
            {
                List<short> data = new List<short>();
                int samples = (int)(sampleRate * seconds);

                Random rand = new Random();

                Queue<double> buf = new Queue<double>();
                for (int n = 0; n < sampleRate / 55; n++)
                {
                    buf.Enqueue(rand.NextDouble() * 2.0 - 1.0);
                }

                for (int n = 0; n < samples; n++)
                {
                    double first = buf.Dequeue();
                    double next = buf.Peek();
                    double sample = 0.996 * 0.5 * (first + next);
                    buf.Enqueue(sample);
                    data.Add((short)(sample * short.MaxValue));
                }

                MemoryStream ms = new MemoryStream(data.SelectMany(BitConverter.GetBytes).ToArray());
                RawSourceWaveStream rs = new RawSourceWaveStream(ms, new WaveFormat(sampleRate, 16, 1));
                WaveOutEvent wo = new WaveOutEvent();
                wo.Init(rs);
                wo.Play();
                while (wo.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(500);
                }
            }
        }
    }

}

