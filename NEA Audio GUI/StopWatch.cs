using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace NEA_Audio_GUI
{
    public class StopWatchManager
    {
        private Stopwatch stopwatch;
        private  System.Windows.Forms.Timer timer;
        private  TextBox display;

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
            timer.Start();
        }

        public void Stop()
        {
            stopwatch.Stop();
            timer.Stop();
        }

        public void Reset()
        {
            stopwatch.Reset();
            display.Text = "00:00:00";
        }
    }
}
