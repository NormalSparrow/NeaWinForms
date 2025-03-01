using System;
using System.IO;
using System.Windows.Forms;

namespace NEA_Audio_GUI
{
    public partial class DownloadPopupForm : Form
    {
        
        public double Duration { get; set; }

        
        public DownloadPopupForm()
        {
            InitializeComponent();
        }

      
        private void FinishButton_Click(object sender, EventArgs e)
        {
            
            string durationText = AmountOfSecondsBox.Text;

            try
            {
              
                double duration = Convert.ToDouble(durationText);

               
                if (duration <= 0)
                {
                    MessageBox.Show("Please enter a number greater than 0.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }

                
                this.Duration = duration;
                this.DialogResult = DialogResult.OK; 
                this.Close(); 
            }
            catch
            { 
                MessageBox.Show("Please enter a valid number greater than 0.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private void AmountOfSecondsBox_TextChanged(object sender, EventArgs e)
        {
            
        }

    
        public void GenerateAndDownloadAudioData(double durationInSeconds)
        {
            
            byte[] audioData = GenerateAudioData(durationInSeconds);

            if (audioData != null)
            {
               
                byte[] repeatedAudioData = RepeatAudioData(audioData, durationInSeconds);

                if (repeatedAudioData != null)
                {
                    
                    DownloadAudio(repeatedAudioData);
                }
                else
                {
                    MessageBox.Show($"audio data couldn't be repeated", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("No audio data generated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

       
        private byte[] GenerateAudioData(double durationInSeconds)
        {
            
            int sampleRate = 44100; 
            int bitsPerSample = 16; 
            int channels = 1; 
            int bytesPerSecond = sampleRate * bitsPerSample / 8 * channels;
            int totalBytes = (int)(bytesPerSecond * durationInSeconds);

            return new byte[totalBytes]; 
        }

       
        private byte[] RepeatAudioData(byte[] audioData, double durationInSeconds)
        {
            if (audioData == null || audioData.Length == 0)
            {
                return null;
            }

            int sampleRate = 44100; 
            int bitsPerSample = 16; 
            int channels = 1; 
            int bytesPerSecond = sampleRate * bitsPerSample / 8 * channels;
            int totalBytes = (int)(bytesPerSecond * durationInSeconds);

           
            byte[] repeatedData = new byte[totalBytes];

           
            for (int i = 0; i < totalBytes; i += audioData.Length)
            {
                int bytesToCopy = Math.Min(audioData.Length, totalBytes - i);
                Array.Copy(audioData, 0, repeatedData, i, bytesToCopy);
            }

            return repeatedData;
        }


        private void DownloadAudio(byte[] audioData)
        {
            if (audioData == null || audioData.Length == 0)
            {
                MessageBox.Show("there is either no data, or the data length is = 0", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

          
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog()) //user chooses folder
            {
                folderDialog.Description = "Select where to save the file";
                folderDialog.ShowNewFolderButton = true;

                if (folderDialog.ShowDialog() == DialogResult.OK)
                {

                    string filePath = Path.Combine(folderDialog.SelectedPath, "NeaAudioSample.wav"); 

                    
                    try
                    {
                        File.WriteAllBytes(filePath, audioData);
                        MessageBox.Show("audio downloaded", "download finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}