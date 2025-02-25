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
                    audioStream = MixStreams(streams.ToArray());
                }

                if (audioStream != null)
                {
                    stopWatchManager.Start();
                    ScottPlottTimer.Start();
                    await audioPlayer.PlayAudio(audioStream); // until bool playing = false, this will play audio
                }
                else
                {
                    MessageBox.Show("No audio stream generated. Please toggle one or more wave types.");
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

        private RawSourceWaveStream MixStreams(RawSourceWaveStream[] streams)
        {
            if (streams.Length == 0)
            {
                throw new ArgumentException("something has gone incredibly wrong, mixstreams is being called with no streams");
            }
            var format = streams[0].WaveFormat;
            foreach (var stream in streams)
            {
                if (stream.WaveFormat != format)
                {
                    Console.WriteLine("error occured: waveStreams are not formatted the same");
                }
            }
            List<byte[]> sampleBuffers = new List<byte[]>(); //all bytes of samples are in a list 
            foreach (var stream in streams)
            {
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length); //read samples to buffer
                sampleBuffers.Add(buffer);
            }


            if (inUseWaveTypes.Contains(WaveType.Karplus)) //application of decay to all waves
            {
                for (int i = 0; i < sampleBuffers.Count; i++)
                {
                    byte[] buffer = sampleBuffers[i];
                    short[] samples = new short[buffer.Length / 2];


                    for (int j = 0; j < samples.Length; j++)  // byte to short array
                    {
                        samples[j] = BitConverter.ToInt16(buffer, j * 2);
                    }


                    samples = audioKarplus.ApplyDecay(samples);


                    byte[] decayedBuffer = new byte[samples.Length * 2];// Convert short array back to byte array
                    Buffer.BlockCopy(samples, 0, decayedBuffer, 0, decayedBuffer.Length);
                    sampleBuffers[i] = decayedBuffer;
                }
            }

            List<short> mixedSamples = new List<short>();
            for (int i = 0; i < sampleBuffers[0].Length; i += 2) // Process 2 bytes at a time (16-bit samples)  
            {
                int mixedSample = 0;

                foreach (var buffer in sampleBuffers)      // add the samples from all streams
                {
                    short sample = BitConverter.ToInt16(buffer, i); // Convert 2 bytes to a short 
                    mixedSample += sample;
                }

                mixedSample = mixedSample / streams.Length;
                mixedSample = Math.Max(short.MinValue, Math.Min(short.MaxValue, mixedSample));// makes sure sample is in range

                mixedSamples.Add((short)mixedSample);
            }

            latestSamples = mixedSamples
                .Select(s => (double)s)
                .Take(1000) //resolution for display is 1000 samples max
                .ToArray();// to plot points on the scottplot, float to double

            if (latestSamples.Length > 0)
            {
                Oscillator.Plot.Clear();
                Oscillator.Plot.Add.Scatter(XAxisValues(latestSamples.Length), latestSamples);
                Oscillator.Plot.Axes.AutoScale();
                Oscillator.Refresh();
            }

            byte[] mixedBytes = mixedSamples.SelectMany(BitConverter.GetBytes).ToArray();// Convert the mixed samples back into a byte 
            MemoryStream mixedStream = new MemoryStream(mixedBytes);
            mixedStream.Position = 0;

            return new RawSourceWaveStream(mixedStream, format); //final array
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


        private enum WaveType
        {

            Karplus,
            Triangle,
            Square,
            Sawtooth
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

            DownloadPopupForm downloadPopup = new DownloadPopupForm();
            downloadPopup.ShowDialog();
        }
    }

}
