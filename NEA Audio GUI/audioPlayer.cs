using NAudio.Wave;
using System.Security.Cryptography.X509Certificates;

internal class AudioPlayer
{
    private WaveOutEvent wo = new WaveOutEvent();
    private ISampleProvider liveSample;
    private float[] liveBuffer;
    private bool playing;
    public async Task PlayAudio(RawSourceWaveStream rs)
    {
        wo.Init(rs);

        liveSample = rs.ToSampleProvider();//Naudio function to read the sample without waveStream
        liveBuffer = new float[2048];
        wo.Play();
        playing= true;

        while (playing)
        {
            await Task.Delay(50);
        }
        if (wo.PlaybackState == PlaybackState.Playing) {
            wo.Stop();  
        }
        

    }
    public float[] GetLiveSamples()
    {
       
        int samplesRead = liveSample.Read(liveBuffer, 0, liveBuffer.Length);

     
        if (samplesRead > 0)
        {
     
            return liveBuffer.Take(samplesRead).ToArray();
        }
        else
        {
            //         MessageBox.Show("live samples for the scottplott graph has been returned as 0");

            return null;    
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
        playing= false;
    }
}
