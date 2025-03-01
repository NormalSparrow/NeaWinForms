namespace NEA_Audio_GUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            playButton = new Button();
            TrianglewaveButton = new Button();
            SawtoothWaveButton = new Button();
            SquarewaveButton = new Button();
            decayButton = new Button();
            Frequency = new TrackBar();
            Volume = new TrackBar();
            Oscillator = new ScottPlot.WinForms.FormsPlot();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            StopWatchDisplay = new TextBox();
            stopwatchTimer = new System.Windows.Forms.Timer(components);
            DownloadButton = new Button();
            ((System.ComponentModel.ISupportInitialize)Frequency).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Volume).BeginInit();
            SuspendLayout();
            // 
            // playButton
            // 
            playButton.Location = new Point(575, 346);
            playButton.Margin = new Padding(2, 1, 2, 1);
            playButton.Name = "playButton";
            playButton.Size = new Size(82, 47);
            playButton.TabIndex = 0;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = true;
            playButton.Click += playButton_Click;
            // 
            // TrianglewaveButton
            // 
            TrianglewaveButton.Location = new Point(121, 382);
            TrianglewaveButton.Margin = new Padding(2, 1, 2, 1);
            TrianglewaveButton.Name = "TrianglewaveButton";
            TrianglewaveButton.Size = new Size(78, 49);
            TrianglewaveButton.TabIndex = 1;
            TrianglewaveButton.Text = "▲";
            TrianglewaveButton.UseVisualStyleBackColor = true;
            TrianglewaveButton.Click += TrianglewaveButton_Click;
            // 
            // SawtoothWaveButton
            // 
            SawtoothWaveButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            SawtoothWaveButton.Location = new Point(43, 382);
            SawtoothWaveButton.Margin = new Padding(2, 1, 2, 1);
            SawtoothWaveButton.Name = "SawtoothWaveButton";
            SawtoothWaveButton.Size = new Size(76, 49);
            SawtoothWaveButton.TabIndex = 2;
            SawtoothWaveButton.Text = "N";
            SawtoothWaveButton.UseVisualStyleBackColor = true;
            SawtoothWaveButton.Click += SawtoothWaveButton_Click;
            // 
            // SquarewaveButton
            // 
            SquarewaveButton.Location = new Point(43, 333);
            SquarewaveButton.Margin = new Padding(2, 1, 2, 1);
            SquarewaveButton.Name = "SquarewaveButton";
            SquarewaveButton.Size = new Size(76, 47);
            SquarewaveButton.TabIndex = 3;
            SquarewaveButton.Text = " ⬛";
            SquarewaveButton.UseVisualStyleBackColor = true;
            SquarewaveButton.Click += SquarewaveButton_Click;
            // 
            // decayButton
            // 
            decayButton.Location = new Point(123, 333);
            decayButton.Margin = new Padding(2, 1, 2, 1);
            decayButton.Name = "decayButton";
            decayButton.Size = new Size(76, 47);
            decayButton.TabIndex = 4;
            decayButton.Text = "〜";
            decayButton.UseVisualStyleBackColor = true;
            decayButton.Click += decayButton_Click;
            // 
            // Frequency
            // 
            Frequency.Location = new Point(324, 465);
            Frequency.Margin = new Padding(2, 1, 2, 1);
            Frequency.Name = "Frequency";
            Frequency.Size = new Size(233, 45);
            Frequency.TabIndex = 5;
            Frequency.Scroll += frequency_Scroll;
            // 
            // Volume
            // 
            Volume.Location = new Point(575, 465);
            Volume.Margin = new Padding(2, 1, 2, 1);
            Volume.Name = "Volume";
            Volume.Size = new Size(233, 45);
            Volume.TabIndex = 6;
            Volume.Scroll += Volume_Scroll;
            // 
            // Oscillator
            // 
            Oscillator.BackColor = SystemColors.ControlLightLight;
            Oscillator.DisplayScale = 1F;
            Oscillator.Location = new Point(0, 12);
            Oscillator.Name = "Oscillator";
            Oscillator.Size = new Size(816, 317);
            Oscillator.TabIndex = 7;
          
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Menu;
            textBox1.Location = new Point(324, 438);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(100, 23);
            textBox1.TabIndex = 8;
            textBox1.Text = "Frequency";
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.Menu;
            textBox2.Location = new Point(582, 438);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(100, 23);
            textBox2.TabIndex = 9;
            textBox2.Text = "Volume";
            // 
            // StopWatchDisplay
            // 
            StopWatchDisplay.Location = new Point(662, 346);
            StopWatchDisplay.Name = "StopWatchDisplay";
            StopWatchDisplay.PlaceholderText = "RunTime ";
            StopWatchDisplay.ReadOnly = true;
            StopWatchDisplay.Size = new Size(154, 23);
            StopWatchDisplay.TabIndex = 10;
            // 
            // DownloadButton
            // 
            DownloadButton.Location = new Point(43, 495);
            DownloadButton.Name = "DownloadButton";
            DownloadButton.Size = new Size(156, 41);
            DownloadButton.TabIndex = 11;
            DownloadButton.Text = "Download ";
            DownloadButton.UseVisualStyleBackColor = true;
            DownloadButton.Click += DownloadButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(819, 565);
            Controls.Add(DownloadButton);
            Controls.Add(StopWatchDisplay);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(Oscillator);
            Controls.Add(Volume);
            Controls.Add(Frequency);
            Controls.Add(decayButton);
            Controls.Add(SquarewaveButton);
            Controls.Add(SawtoothWaveButton);
            Controls.Add(TrianglewaveButton);
            Controls.Add(playButton);
            Margin = new Padding(2, 1, 2, 1);
            Name = "Form1";
            Text = "Form1";
            
            ((System.ComponentModel.ISupportInitialize)Frequency).EndInit();
            ((System.ComponentModel.ISupportInitialize)Volume).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button playButton;
        private Button TrianglewaveButton;
        private Button SawtoothWaveButton;
        private Button SquarewaveButton;
        private Button decayButton;
        private TrackBar Frequency;
        private TrackBar Volume;
        private ScottPlot.WinForms.FormsPlot Oscillator;
        private TextBox textBox1;
        private TextBox textBox2;
        private NAudio.Gui.VolumeSlider volumeSlider1;
        private TextBox StopWatchDisplay;
        private System.Windows.Forms.Timer stopwatchTimer;
        private Button DownloadButton;
    }
}