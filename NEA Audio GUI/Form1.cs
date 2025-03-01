using NAudio.Wave;
using Streamloop;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows.Forms;
namespace NEA_Audio_GUI
{
    public partial class Form1 : Form
    {
        private karplus audioKarplus;
        private trianglewave audioTriangle;
        private squarewave audioSquare;
        private sawtoothwave audioSawtooth;
        private AudioPlayer audioPlayer;
        private RawSourceWaveStream? audioStream;
        private byte[] storedAudioData;
        private WaveType waveType;
        private List<WaveType> inUseWaveTypes = new List<WaveType>();
        private double frequency = 55000d;
        private Dictionary<WaveType, Button> waveTypeButtons;
        private double[] latestSamples = new double[500]; // scottplot live buffer
        private System.Windows.Forms.Timer ScottPlottTimer;
        public static WaveFormat CommonWaveFormat = new WaveFormat(44100, 16, 1);
        private StopWatchManager stopWatchManager;
       // public byte[] previousArray;
        private double[] XAxisValues(int count)
        {
            List<double> xValues = new List<double>();
            for (int i = 0; i < count; i++)
            {
                xValues.Add(i);
            }
            return xValues.ToArray();
        }
      
        public Form1()
        {
            InitializeComponent();


            audioKarplus = new karplus();
            audioPlayer = new AudioPlayer();
            audioTriangle = new trianglewave();
            audioSquare = new squarewave();
            audioSawtooth = new sawtoothwave();
            Volume.Maximum = 1000;
            Frequency.Maximum = 1000;

            waveTypeButtons = new Dictionary<WaveType, Button>() {

                { WaveType.Karplus, decayButton },
                { WaveType.Triangle, triangleWaveButton }, //dictionary to make it easier to add more waveTypes 
                { WaveType.Square, squareWaveButton},
                { WaveType.Sawtooth, sawtoothWaveButton },
            };

            Oscillator.Plot.Title("WaveForm visualizer");
            Oscillator.Plot.XLabel("Time");
            Oscillator.Plot.YLabel("Amplitude");
            Oscillator.Refresh();

            ScottPlottTimer = new System.Windows.Forms.Timer();
            ScottPlottTimer.Interval = 10;
            ScottPlottTimer.Tick += UpdateScottPlott;

            stopWatchManager = new StopWatchManager(StopWatchDisplay, stopwatchTimer);
          

        }

        private async void playButton_Click(object sender, EventArgs e)
        {
            if (playButton.Text == "Play")
            {
                playButton.Text = "Stop";

                if (audioPlayer.GetState() == PlaybackState.Playing)
                {
                    audioPlayer.StopAudio();
                }

                if (audioStream != null)
                {
                    audioStream.Dispose();
                    audioStream = null;
                }

                List<RawSourceWaveStream> streams = new List<RawSourceWaveStream>();
                foreach (var waveType in inUseWaveTypes)
                {
                    RawSourceWaveStream stream = null;
                    switch (waveType)
                    {
                        case WaveType.Triangle:
                            stream = audioTriangle.Triangle(frequency: frequency);
                            break;
                        case WaveType.Square:
                            stream = audioSquare.Square(frequency: frequency);
                            break;
                        case WaveType.Sawtooth:
                            stream = audioSawtooth.Sawtooth(frequency: frequency);
                            break;
                    }

                    if (stream != null)
                    {
                        streams.Add(stream);
                    }
                }

                if (streams.Count > 0)
                {
                    var audioFileGenerator = new AudioFileGenerator(
                        audioKarplus,
                        audioTriangle,
                        audioSquare,
                        audioSawtooth,
                        inUseWaveTypes,
                        frequency,
                        CommonWaveFormat
                    );


                    byte[] mixedBytes = audioFileGenerator.MixStreams(streams.ToArray()); 
                    MemoryStream mixedStream = new MemoryStream(mixedBytes);
                    mixedStream.Position = 0;
                    audioStream = new RawSourceWaveStream(mixedStream, CommonWaveFormat); // result of mixstreams 
                }

                if (audioStream != null)
                {
                    stopWatchManager.Start();
                    ScottPlottTimer.Start();
                    await audioPlayer.PlayAudio(audioStream);
                }
                else
                {
                    MessageBox.Show("No audio stream generated, click one or more wavetypes to mix");
                    playButton.Text = "Play";
                }
            }
            else
            {
                playButton.Text = "Play";
                stopWatchManager.Stop();
                audioPlayer.StopAudio();
                ScottPlottTimer.Stop();
            }
        }
   
        private void UpdateScottPlott(object sender, EventArgs e)
        {

            float[] liveSamples = audioPlayer.GetLiveSamples();

            if (liveSamples == null)
            {
                playButton.Text = "Play";
                stopWatchManager.Stop();
                ScottPlottTimer.Stop();
                return;
            }
            if (liveSamples.Length > 0)
            {

                latestSamples = liveSamples
                    .Select(s => (double)s)
                    .Take(1000)
                    .ToArray();
                UpdatePlot();
            }

        }

        private void UpdatePlot()
        {
            if (latestSamples.Length > 0)
            {
                Oscillator.Plot.Clear();
                Oscillator.Plot.Add.Scatter(XAxisValues(latestSamples.Length), latestSamples);
                Oscillator.Plot.Axes.AutoScale();
                Oscillator.Refresh();
            }
        }


        private void decayButton_Click(object sender, EventArgs e)
        {

            toggleButton(WaveType.Karplus);
        }
        private void triangleWaveButton_Click(object sender, EventArgs e)
        {
            toggleButton(WaveType.Triangle);
        }
        private void squareWaveButton_Click(object sender, EventArgs e)
        {
            toggleButton(WaveType.Square);
        }
        private void sawtoothWaveButton_Click(object sender, EventArgs e)
        {
            toggleButton(WaveType.Sawtooth);
        }
        private void toggleButton(WaveType waveType)
        {
            if (inUseWaveTypes.Contains(waveType))
            {
                inUseWaveTypes.Remove(waveType);
            }
            else
            {
                inUseWaveTypes.Add(waveType);
            }
            ButtonAppearance();
        }

        private void ButtonAppearance()
        {
            //    decayButton.BackColor = inUseWaveTypes.Contains(WaveType.Karplus) ? Color.Green : SystemColors.Control;
            //  triangleWaveButton.BackColor = inUseWaveTypes.Contains(WaveType.Triangle) ? Color.Green : SystemColors.Control; make dictionary


            foreach (var keyValuePair in waveTypeButtons)
            {
                WaveType waveType = keyValuePair.Key;
                Button button = keyValuePair.Value;

                if (inUseWaveTypes.Contains(waveType))
                {
                    button.BackColor = Color.Green;
                }
                else
                {
                    button.BackColor = Color.White;
                }

            }
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


  


        private void formsPlot1_Load(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void StopWatchDisplay_TextChanged(object sender, EventArgs e)
        {

        }

        private void DownloadButton_Click(object sender, EventArgs e)
        {
        
            using (DownloadPopupForm downloadPopup = new DownloadPopupForm())
            {
                if (downloadPopup.ShowDialog() == DialogResult.OK)
                {
                    double durationInSeconds = downloadPopup.Duration;

                   
                    var audioFileGenerator = new AudioFileGenerator(
                        audioKarplus,
                        audioTriangle,
                        audioSquare,
                        audioSawtooth,
                        inUseWaveTypes,
                        frequency,
                        CommonWaveFormat
                    );

                    
                    audioFileGenerator.GenerateAndDownloadAudioData(durationInSeconds);
                }
            }
        }
    }

}
