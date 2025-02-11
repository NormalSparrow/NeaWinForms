
using NAudio.Wave;
using System.Runtime.CompilerServices;

namespace NEA_Audio_GUI
{
    public partial class Form1 : Form
    {
        private karplus audioKarplus;
        private trianglewave audioTriangle;
        private AudioPlayer audioPlayer;
        private RawSourceWaveStream? audioStream;
        private byte[] storedAudioData;
        private WaveType waveType;
        private double frequency = 55000d;

        public Form1()
        {
            InitializeComponent();
            audioKarplus = new karplus();
            audioPlayer = new AudioPlayer();
            audioTriangle = new trianglewave();
            Volume.Maximum = 1000;
            Frequency.Maximum = 1000;
        }

        private async void playButton_Click(object sender, EventArgs e)
        {
            if (audioPlayer.GetState() == PlaybackState.Playing)
            {
                audioPlayer.StopAudio();
            }


            if (audioStream != null)
            {
                audioStream.Dispose();
                audioStream = null;
            }

            if (waveType == WaveType.Karplus)
            {
                audioStream = audioKarplus.Decay(frequency: frequency);
            }
            if (waveType == WaveType.Triangle)
            {
                audioStream = audioTriangle.Triangle(frequency: frequency);
            }
            if (audioStream != null)
            {
                await audioPlayer.PlayAudio(audioStream);
            }
            else
            {
                MessageBox.Show("please click something else to generate noise before play");
            }
        }
        private void decayButton_Click(object sender, EventArgs e)
        {

            waveType = WaveType.Karplus;
        }

        private void Volume_Scroll(object sender, EventArgs e)
        {
            float volume = 0;
            if (Volume.Value / 1000f > 0)
            {
                volume = Volume.Value / 1000f;
                audioPlayer.SetVolume(volume);
            }

        }

        private void frequency_Scroll(object sender, EventArgs e)
        {
            frequency = 1000 + (Frequency.Value * 100);
        }

        private void triangleWaveButton_Click(object sender, EventArgs e)
        {
            waveType = WaveType.Triangle;
        }

        private enum WaveType
        {

            Karplus,
            Triangle
        }



    }

}
