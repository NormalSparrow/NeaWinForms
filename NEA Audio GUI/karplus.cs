using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NAudio.Wave;

namespace NEA_Audio_GUI
{
    internal class karplus
    {
        public RawSourceWaveStream KarplusString(double frequency = 55000d, double amplitude = 1d, double seconds = 5d)
        {
            int sampleRate = Form1.CommonWaveFormat.SampleRate;
            List<short> data = new List<short>();
            int samples = (int)(sampleRate * seconds);

            // Use a fixed seed for consistent results
            Random rand = new Random(42); // You can use any integer value as the seed
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
            ms.Position = 0;
            return new RawSourceWaveStream(ms, Form1.CommonWaveFormat);
        }

        public short[] ApplyDecay(short[] inputSamples) //for > 1 amount of samples
        {
            int sampleRate = Form1.CommonWaveFormat.SampleRate;
            List<short> decayedSamples = new List<short>();

            
            Random rand = new Random(42); 
            Queue<double> buf = new Queue<double>();

            for (int n = 0; n < sampleRate / (55000d / 1000); n++) 
            {
                buf.Enqueue(rand.NextDouble() * 2.0 - 1.0);
            }

        
            for (int n = 0; n < inputSamples.Length; n++)
            {
                double first = buf.Dequeue();
                double next = buf.Peek();
                double sample = 0.996 * 0.5 * (first + next);
                buf.Enqueue(sample);

                
                double mixedSample = (inputSamples[n] / (double)short.MaxValue) + sample;// Mix the input sample with the decayed sample
                mixedSample = Math.Max(-1.0, Math.Min(1.0, mixedSample)); // range 1, -1

                decayedSamples.Add((short)(mixedSample * short.MaxValue)); //add buffer to list
            }

            return decayedSamples.ToArray();
        }
    }
}