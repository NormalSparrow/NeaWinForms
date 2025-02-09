
using NAudio.Wave;
using System.Runtime.CompilerServices;

namespace NEA_Audio_GUI
{
    public partial class Form1 : Form
    {
        private karplus audioKarplus;
        private AudioPlayer audioPlayer;
        private RawSourceWaveStream? audioStream;
        private byte[] storedAudioData;
        private WaveType waveType;
        public Form1()
        {
            InitializeComponent();
            audioKarplus = new karplus();
            audioPlayer = new AudioPlayer();
            Volume.Maximum = 1000;
        }

        private async void playButton_Click(object sender, EventArgs e)
        {
            if (audioPlayer.GetState() == PlaybackState.Playing)
            {
                audioPlayer.StopAudio();
            }
            if (waveType == WaveType.Karplus)
            {
                audioStream = audioKarplus.Decay();
            }
            if (audioStream != null)
            {
            await audioPlayer.PlayAudio(audioStream);

            }
            else
            {
                MessageBox.Show("Please click one of the wave buttons to generate a sound");
            }
        }

        private void decayButton_Click(object sender, EventArgs e)
        {
            
            waveType = WaveType.Karplus;
        }

        private void Volume_Scroll(object sender, EventArgs e)
        {
            float volume = 0;
                if( Volume.Value / 1000f > 0)
            {
                volume = Volume.Value /1000f;
                audioPlayer.SetVolume(volume);
            }
           
        }
        private enum WaveType { 
        
         Karplus
        
        }



    }

}
