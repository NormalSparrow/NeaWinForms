using NAudio.Wave;

internal class AudioPlayer
{
    private WaveOutEvent wo = new WaveOutEvent();
    
    public async Task PlayAudio(RawSourceWaveStream rs)
    {
        wo.Init(rs);
        wo.Play();
        await Task.Delay(5000);
        if (wo.PlaybackState == PlaybackState.Playing) {
            wo.Stop();
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
    }
}
