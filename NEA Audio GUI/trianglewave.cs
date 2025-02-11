using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Wave;


namespace NEA_Audio_GUI
{
    internal class trianglewave
    {
        public RawSourceWaveStream Triangle(int sampleRate = 48000, double frequency = 55000d, double amplitude = 1d, double seconds = 5d)
        {
            List<short> data = new List<short>();// 
            int samples = (int)(sampleRate * seconds);
            double period = frequency / samples;
            
            for (int n = 0; n < sampleRate / (frequency / 1000); n++) //making buffer size 
            {
                double time = n / period;
                double sample = 2 * amplitude / Math.PI * Math.Asin(Math.Sin(2 * Math.PI * frequency * time)); //algorithm for a triangle wave 
                data.Add((short)(sample * short.MaxValue));
            }
            MemoryStream ms = new MemoryStream(data.SelectMany(BitConverter.GetBytes).ToArray());
            ms.Position = 0; 
            return new RawSourceWaveStream(ms, new WaveFormat(sampleRate, 16, 1));
        }
    }
}
