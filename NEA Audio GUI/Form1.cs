
using NAudio.Wave;
using System.IO;
using System.Runtime.CompilerServices;

namespace NEA_Audio_GUI
{
    public partial class Form1 : Form
    {
        private karplus audioKarplus;
        private trianglewave audioTriangle;
        private squarewave audioSquare;
        private AudioPlayer audioPlayer;
        private RawSourceWaveStream? audioStream;
        private byte[] storedAudioData;
        private WaveType waveType;
        private List<WaveType> inUseWaveTypes = new List<WaveType>();
        private double frequency = 55000d;
        private Dictionary<WaveType, Button> waveTypeButtons;
        private double[] latestSamples = new double[500]; // Buffer for visualization
        private System.Windows.Forms.Timer visualizerTimer;

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
            Volume.Maximum = 1000;
            Frequency.Maximum = 1000;

            waveTypeButtons = new Dictionary<WaveType, Button>() {

                { WaveType.Karplus, decayButton },
                { WaveType.Triangle, triangleWaveButton }, //dictionary to make it easier to add more waveTypes 
                { WaveType.Square, squareWaveButton},
            };

            Oscillator.Plot.Title("WaveForm visualizer");
            Oscillator.Plot.XLabel("Time");
            Oscillator.Plot.YLabel("Amplitude");
            Oscillator.Refresh();

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

            List<RawSourceWaveStream> streams = new List<RawSourceWaveStream>();
            foreach (var waveType in inUseWaveTypes)
            {
                RawSourceWaveStream stream = null;
                switch (waveType)
                {
                    case WaveType.Karplus:
                        stream = audioKarplus.Decay(frequency: frequency);
                        break;

                    case WaveType.Triangle:
                        stream = audioTriangle.Triangle(frequency: frequency);
                        break;
                    case WaveType.Square:
                        stream = audioSquare.Square(frequency: frequency);
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
                await audioPlayer.PlayAudio(audioStream);
            }
            else
            {
                MessageBox.Show("No audio stream generated. Please toggle one or more wave types.");
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

            latestSamples = mixedSamples.Select(s => s / (double)short.MaxValue).Take(500).ToArray(); // to plot points on the scottplot


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

        private enum WaveType
        {

            Karplus,
            Triangle,
            Square
        }



    }

}
