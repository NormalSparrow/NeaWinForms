using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Wave;

namespace NEA_Audio_GUI
{
    internal class karplus
    {
        public RawSourceWaveStream Decay(int sampleRate = 48000, double frequency = 55000d, double amplitude = 1d, double seconds = 5d)
        {
            List<short> data = new List<short>();
            int samples = (int)(sampleRate * seconds);

            Random rand = new Random();
            Queue<double> buf = new Queue<double>();

            for (int n = 0; n < sampleRate / (frequency / 1000); n++) //making the buffer size
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
            return new RawSourceWaveStream(ms, new WaveFormat(sampleRate, 16, 1));
        }
    }
}
