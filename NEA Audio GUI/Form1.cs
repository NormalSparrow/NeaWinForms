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
        private Karplus audioKarplus;
        private Trianglewave audioTriangle;
        private Squarewave audioSquare;
        private SawtoothWave audioSawtooth;
        private AudioPlayer audioPlayer;
        private RawSourceWaveStream? audioStream;
        private List<WaveType> inUseWaveTypes = new List<WaveType>();
        private double frequency = 55000d;
        private Dictionary<WaveType, Button> waveTypeButtons;
        private double[] latestSamples = new double[500]; // scottplot live buffer
        private System.Windows.Forms.Timer ScottPlottTimer;
        public readonly static WaveFormat CommonWaveFormat = new WaveFormat(44100, 16, 1);
        private StopWatchManager stopWatchManager;
        private AudioFileGenerator audioFileGenerator;



        public Form1()
        {
            InitializeComponent();

            audioKarplus = new Karplus();
            audioPlayer = new AudioPlayer();
            audioTriangle = new Trianglewave();
            audioSquare = new Squarewave();
            audioSawtooth = new SawtoothWave();
            Volume.Maximum = 1000;
            Frequency.Maximum = 1000;

            
            audioFileGenerator = new AudioFileGenerator(
                audioKarplus,
                audioTriangle,
                audioSquare,
                audioSawtooth,
                inUseWaveTypes,
                frequency,
                CommonWaveFormat
            );

            waveTypeButtons = new Dictionary<WaveType, Button>() {
                { WaveType.Karplus, decayButton },
                { WaveType.Triangle, TrianglewaveButton }, //dictionary to make it easier to add more waveTypes 
                { WaveType.Square, SquarewaveButton},
                { WaveType.Sawtooth, SawtoothWaveButton },
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
                    await audioStream.DisposeAsync(); 
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
                    // Use the class-level audioFileGenerator to mix streams
                    byte[] mixedBytes = audioFileGenerator.MixStreams(streams.ToArray());
                    MemoryStream mixedStream = new MemoryStream(mixedBytes);
                    mixedStream.Position = 0;
                    audioStream = new RawSourceWaveStream(mixedStream, CommonWaveFormat);
                }

                if (audioStream != null)
                {
                    stopWatchManager.Start();
                    ScottPlottTimer.Start();
                    await audioPlayer.PlayAudio(audioStream);
                }
                else
                {
                    MessageBox.Show("toggle one or more waves types to generate audio");
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

        private static double[] XAxisValues(int count)
        {
            List<double> xValues = new List<double>();
            for (int i = 0; i < count; i++)
            {
                xValues.Add(i);
            }
            return xValues.ToArray();
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

        private void TrianglewaveButton_Click(object sender, EventArgs e)
        {
            toggleButton(WaveType.Triangle);
        }

        private void SquarewaveButton_Click(object sender, EventArgs e)
        {
            toggleButton(WaveType.Square);
        }

        private void SawtoothWaveButton_Click(object sender, EventArgs e)
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

        private void DownloadButton_Click(object sender, EventArgs e)
        {
            using (DownloadPopupForm downloadPopup = new DownloadPopupForm())
            {
                if (downloadPopup.ShowDialog() == DialogResult.OK)
                {
                    double durationInSeconds = downloadPopup.Duration;

                    
                    audioFileGenerator.GenerateAndDownloadAudioData(durationInSeconds);
                }
            }
        }
    }
}
