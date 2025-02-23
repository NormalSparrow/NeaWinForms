﻿using System;

namespace NEA_Audio_GUI
{
    internal class karplus
    {
        public short[] ApplyDecay(short[] inputSamples)
        {
            int sampleRate = Form1.CommonWaveFormat.SampleRate;
            List<short> decayedSamples = new List<short>();

          
            Random rand = new Random(45);
            Queue<double> buf = new Queue<double>();

            
            for (int n = 0; n < sampleRate / (55000d / 1000); n++) // create buffers for samples
            {
                buf.Enqueue(rand.NextDouble() * 2.0 - 1.0);
            }

            // Apply the Karplus-Strong decay to the input samples
            for (int n = 0; n < inputSamples.Length; n++)
            {
                double first = buf.Dequeue();
                double next = buf.Peek();
                double sample = 0.996 * 0.5 * (first + next);
                buf.Enqueue(sample);

                // Mix the input sample with the decayed sample
                double mixedSample = (inputSamples[n] / (double)short.MaxValue) + sample;
                mixedSample = Math.Max(-1.0, Math.Min(1.0, mixedSample)); // Clamp to [-1, 1]

                decayedSamples.Add((short)(mixedSample * short.MaxValue));
            }

            return decayedSamples.ToArray();
        }
    }
}