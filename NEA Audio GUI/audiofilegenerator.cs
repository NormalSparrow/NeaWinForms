using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;

namespace NEA_Audio_GUI
{
    public class AudioFileGenerator
    {
        private readonly Karplus audioKarplus;
        private readonly Trianglewave audioTriangle;
        private readonly Squarewave audioSquare;
        private readonly SawtoothWave audioSawtooth;
        private readonly List<WaveType> inUseWaveTypes;
        private readonly double frequency;
        private readonly WaveFormat commonWaveFormat;

        public AudioFileGenerator(Karplus audioKarplus, Trianglewave audioTriangle, Squarewave audioSquare, SawtoothWave audioSawtooth, List<WaveType> inUseWaveTypes, double frequency, WaveFormat commonWaveFormat)
        {
            this.audioKarplus = audioKarplus;
            this.audioTriangle = audioTriangle;
            this.audioSquare = audioSquare;
            this.audioSawtooth = audioSawtooth;
            this.inUseWaveTypes = inUseWaveTypes;
            this.frequency = frequency;
            this.commonWaveFormat = commonWaveFormat;
        }

        public void GenerateAndDownloadAudioData(double durationInSeconds)
        {
            
            byte[] audioData = GenerateAudioData(durationInSeconds);

            if (audioData != null)
            {
              
                byte[] repeatedAudioData = RepeatAudioData(audioData, durationInSeconds);  

                if (repeatedAudioData != null)
                {
                    
                    DownloadAudio(repeatedAudioData);
                }
                else
                {
                    MessageBox.Show("cannot repeat audio data", "repeat audio data error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("no audio data generated", "audio data error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private byte[] GenerateAudioData(double durationInSeconds)
        {
            List<RawSourceWaveStream> streams = new List<RawSourceWaveStream>();

            
            foreach (var waveType in inUseWaveTypes)
            {
                RawSourceWaveStream stream = null;
                switch (waveType)
                {
                    case WaveType.Triangle:
                        stream = audioTriangle.Triangle(frequency: frequency, seconds: durationInSeconds);
                        break;
                    case WaveType.Square:
                        stream = audioSquare.Square(frequency: frequency, seconds: durationInSeconds);
                        break;
                    case WaveType.Sawtooth:
                        stream = audioSawtooth.Sawtooth(frequency: frequency, seconds: durationInSeconds);
                        break;
                }

                if (stream != null)
                {
                    streams.Add(stream);
                }
            }

          
            if (streams.Count > 0)
            {
                return MixStreams(streams.ToArray());
            }

            return new byte[0]; 
        }

        public byte[] MixStreams(RawSourceWaveStream[] streams)
        {
            if (streams.Length == 0)
            {
                throw new ArgumentException("no streams to mix, please pick 2 or more to use");
            }

           
            List<byte[]> sampleBuffers = new List<byte[]>();

            foreach (var stream in streams)
            {
                byte[] buffer = new byte[stream.Length];
                _ = stream.Read(buffer, 0, buffer.Length); 
                sampleBuffers.Add(buffer);
            }

            if (inUseWaveTypes.Contains(WaveType.Karplus)) 
            {
                for (int i = 0; i < sampleBuffers.Count; i++)
                {
                    byte[] buffer = sampleBuffers[i];
                    short[] samples = new short[buffer.Length / 2];

                    for (int j = 0; j < samples.Length; j++) 
                    {
                        samples[j] = BitConverter.ToInt16(buffer, j * 2);
                    }

                    samples = audioKarplus.ApplyDecay(samples);

                    byte[] decayedBuffer = new byte[samples.Length * 2]; 
                    Buffer.BlockCopy(samples, 0, decayedBuffer, 0, decayedBuffer.Length);
                    sampleBuffers[i] = decayedBuffer;
                }
            }

            List<short> mixedSamples = new List<short>();
            for (int i = 0; i < sampleBuffers[0].Length; i += 2) //16 bit samples
            {
                int mixedSample = 0;

                foreach (var buffer in sampleBuffers) 
                {
                    short sample = BitConverter.ToInt16(buffer, i); 
                    mixedSample += sample;
                }

                mixedSample = mixedSample / streams.Length;
                mixedSample = Math.Max(short.MinValue, Math.Min(short.MaxValue, mixedSample)); 

                mixedSamples.Add((short)mixedSample);
            }

            
            return mixedSamples.SelectMany(BitConverter.GetBytes).ToArray();
        }
        private byte[] RepeatAudioData(byte[] audioData, double durationInSeconds) 
        {
            if (audioData == null || audioData.Length == 0)
            {
                return new byte[0];
            }

            int sampleRate = commonWaveFormat.SampleRate;
            int bytesPerSecond = sampleRate * commonWaveFormat.BitsPerSample / 8 * commonWaveFormat.Channels;
            int totalBytes = (int)(bytesPerSecond * durationInSeconds);

          
            byte[] repeatedData = new byte[totalBytes]; // this to hold the previous data

            
            for (int i = 0; i < totalBytes; i += audioData.Length)
            {
                int bytesToCopy = Math.Min(audioData.Length, totalBytes - i);
                Array.Copy(audioData, 0, repeatedData, i, bytesToCopy);
            }

            return repeatedData;
        }

        private static void DownloadAudio(byte[] audioData)
        {
            if (audioData == null || audioData.Length == 0)
            {
                MessageBox.Show("no audio data to save.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

          
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                folderDialog.Description ="select folder";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                   
                    string filePath = Path.Combine(folderDialog.SelectedPath, "audio_sample.wav");

               
                    try
                    {
                        File.WriteAllBytes(filePath, audioData);
                        MessageBox.Show("audio saved", "download finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"error: {ex.Message}", "download error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}