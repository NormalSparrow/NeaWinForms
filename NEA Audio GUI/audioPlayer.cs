using NAudio.Wave;
using Streamloop;
using System.Security.Cryptography.X509Certificates;    

namespace NEA_Audio_GUI
{
    internal class AudioPlayer
    {
        private readonly WaveOutEvent wo = new WaveOutEvent();
        private ISampleProvider liveSample = null;
        private float[] liveBuffer = null;
        private bool playing;
        private LoopStream loopStream = null;

        public async Task PlayAudio(RawSourceWaveStream rs)
        {
          
            loopStream = new LoopStream(rs);
            wo.Init(loopStream);

            liveSample = loopStream.ToSampleProvider(); 
            liveBuffer = new float[2048];
            wo.Play();
            playing = true;

            while (playing)
            {
                await Task.Delay(50);
            }

            if (wo.PlaybackState == PlaybackState.Playing)
            {
                wo.Stop();
            }
        }

        public float[] GetLiveSamples()
        {
            if (liveSample == null || liveBuffer == null)
            {
                return new float[0];
            }

            int samplesRead = liveSample.Read(liveBuffer, 0, liveBuffer.Length);

            if (samplesRead > 0)
            {
                return liveBuffer.Take(samplesRead).ToArray();
            }
            else
            {
                return new float[0];
            }
        }

        public void SetVolume(float volume)
        {
            wo.Volume = volume;
        }

        public PlaybackState GetState()
        {
            return wo.PlaybackState;
        }

        public void StopAudio()
        {
            wo.Stop();
            playing = false;

            if (loopStream != null)
            {
                loopStream.Dispose();
                loopStream = null;
            }
        }
    }
}