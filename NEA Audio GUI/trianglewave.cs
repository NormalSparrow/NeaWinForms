using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Wave;


namespace NEA_Audio_GUI
{
    internal class trianglewave
    {
        public RawSourceWaveStream Triangle( double frequency = 440.0, double amplitude = 1.0, double seconds = 5.0)
        {
            int sampleRate = Form1.CommonWaveFormat.SampleRate;
            List<short> data = new List<short>();
            int samples = (int)(sampleRate * seconds);
            double period = sampleRate / frequency;

            for (int n = 0; n < samples; n++)
            {
                double sample = (2.0 * amplitude / Math.PI) * Math.Asin(Math.Sin(2.0 * Math.PI * n / period));
                data.Add((short)(sample * short.MaxValue));
            }

            MemoryStream ms = new MemoryStream(data.SelectMany(BitConverter.GetBytes).ToArray());
            ms.Position = 0;
            return new RawSourceWaveStream(ms, Form1.CommonWaveFormat);
        }
    }
}
