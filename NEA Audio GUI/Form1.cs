
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
        private List<WaveType> inUseWaveTypes = new List<WaveType>();
        private double frequency = 55000d;
        private Dictionary<WaveType, Button> waveTypeButtons;

        public Form1()
        {
            InitializeComponent();
            audioKarplus = new karplus();
            audioPlayer = new AudioPlayer();
            audioTriangle = new trianglewave();
            Volume.Maximum = 1000;
            Frequency.Maximum = 1000;

            waveTypeButtons = new Dictionary<WaveType, Button>() {
                
                { WaveType.Karplus, decayButton },
                { WaveType.Triangle, triangleWaveButton }, //dictionary to make it easier to add more waveTypes 
            };


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
            foreach (var wavetype in inUseWaveTypes)
            {
                switch (waveType)
                {
                    case WaveType.Karplus:
                        audioStream = audioKarplus.Decay(frequency: frequency);
                        break;

                    case WaveType.Triangle:
                        audioStream = audioTriangle.Triangle(frequency: frequency);
                        break;

                    default:
                        MessageBox.Show("Please toggle one or more wavetypes to generate sound.");
                        return; //break case
                }
                if (streams.Count > 0)
                {
                    //foreach (var stream in streams) {
                    //  List<byte[]> sampleBuffers = new List<byte[]>();}
                    audioStream = MixStreams(streams.ToArray());
                }
            }
       
            if (audioStream != null)
            {
                await audioPlayer.PlayAudio(audioStream);
            }
            else
            {
                MessageBox.Show("No audio stream generated.");
            }
        }
        private RawSourceWaveStream MixStreams(RawSourceWaveStream[] streams)
        {
            var format = streams[0].WaveFormat;
            foreach (var stream in streams)
            {
                if(stream.WaveFormat != format)
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

                
                mixedSample = Math.Max(short.MinValue, Math.Min(short.MaxValue, mixedSample));//overflow catcher

                
                mixedSamples.Add((short)mixedSample);
            }

            
            byte[] mixedBytes = mixedSamples.SelectMany(BitConverter.GetBytes).ToArray();// Convert the mixed samples back into a byte array


            MemoryStream mixedStream = new MemoryStream(mixedBytes);
            mixedStream.Position = 0; 

           
            return new RawSourceWaveStream(mixedStream, format); //final array
       
    }
        private void decayButton_Click(object sender, EventArgs e)
        {

            waveType = WaveType.Karplus;
        }
        private void triangleWaveButton_Click(object sender, EventArgs e)
        {
            waveType = WaveType.Triangle;
        }
        private void toggleButton (WaveType waveType)
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
            Triangle
        }



    }

}
