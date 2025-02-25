using System;
using System.Windows.Forms;

namespace NEA_Audio_GUI
{
    public partial class DownloadPopupForm : Form
    {
    
        public double Duration { get; private set; }

  
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

                if (duration < 0)
                {
                    MessageBox.Show("enter amount of seconds that is greater than 0", "invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
               
                this.Close(); 
            }
            catch 
            {
              
                MessageBox.Show("Please enter a valid number greater than 0", "invalid input", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}