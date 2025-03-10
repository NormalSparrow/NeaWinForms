using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace NEA_Audio_GUI
{
    public class StopWatchManager
    {
        private readonly Stopwatch stopwatch;
        private readonly System.Windows.Forms.Timer timer;
        private readonly  TextBox display;

        public StopWatchManager(TextBox stopWatchDisplay, System.Windows.Forms.Timer stopWatchTimer)
        {
            stopwatch = new Stopwatch();
            display = stopWatchDisplay;
            timer = stopWatchTimer;

            timer.Tick += LiveTick;
        }

        private void LiveTick(object sender, EventArgs e)
        {
            UpdateDisplay();
        }
        private void UpdateDisplay()
        {
            if (stopwatch.IsRunning)
            {
                TimeSpan elapsed = stopwatch.Elapsed;
                display.Text = $"Elapsed Time: {elapsed:mm\\:ss\\.ff}";
            }
        }

        public void Start()
        {
            stopwatch.Restart();
            display.Text = "00:00:00";
            timer.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
            timer.Stop();
        }

      
    }
}
