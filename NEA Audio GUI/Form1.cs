


namespace NEA_Audio_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {

        }

        private void decayButton_Click(object sender, EventArgs e)
        {
            var decay = new karplus.Decay();
            decay.karplus();
        }
    }
}