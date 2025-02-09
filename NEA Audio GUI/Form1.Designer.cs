﻿namespace NEA_Audio_GUI
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
            playButton = new Button();
            triangleWaveButton = new Button();
            sawtoothWaveButton = new Button();
            squareWaveButton = new Button();
            decayButton = new Button();
            frequency = new TrackBar();
            Volume = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)frequency).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Volume).BeginInit();
            SuspendLayout();
            // 
            // playButton
            // 
            playButton.Location = new Point(600, 333);
            playButton.Margin = new Padding(2, 1, 2, 1);
            playButton.Name = "playButton";
            playButton.Size = new Size(82, 47);
            playButton.TabIndex = 0;
            playButton.Text = "Play";
            playButton.UseVisualStyleBackColor = true;
            playButton.Click += playButton_Click;
            // 
            // triangleWaveButton
            // 
            triangleWaveButton.Location = new Point(121, 382);
            triangleWaveButton.Margin = new Padding(2, 1, 2, 1);
            triangleWaveButton.Name = "triangleWaveButton";
            triangleWaveButton.Size = new Size(78, 49);
            triangleWaveButton.TabIndex = 1;
            triangleWaveButton.Text = "▲";
            triangleWaveButton.UseVisualStyleBackColor = true;
            // 
            // sawtoothWaveButton
            // 
            sawtoothWaveButton.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            sawtoothWaveButton.Location = new Point(43, 382);
            sawtoothWaveButton.Margin = new Padding(2, 1, 2, 1);
            sawtoothWaveButton.Name = "sawtoothWaveButton";
            sawtoothWaveButton.Size = new Size(76, 49);
            sawtoothWaveButton.TabIndex = 2;
            sawtoothWaveButton.Text = "N";
            sawtoothWaveButton.UseVisualStyleBackColor = true;
            // 
            // squareWaveButton
            // 
            squareWaveButton.Location = new Point(43, 333);
            squareWaveButton.Margin = new Padding(2, 1, 2, 1);
            squareWaveButton.Name = "squareWaveButton";
            squareWaveButton.Size = new Size(76, 47);
            squareWaveButton.TabIndex = 3;
            squareWaveButton.Text = " ⬛";
            squareWaveButton.UseVisualStyleBackColor = true;
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
            // frequency
            // 
            frequency.Location = new Point(566, 468);
            frequency.Margin = new Padding(2, 1, 2, 1);
            frequency.Name = "frequency";
            frequency.Size = new Size(146, 45);
            frequency.TabIndex = 5;
            // 
            // Volume
            // 
            Volume.Location = new Point(566, 421);
            Volume.Margin = new Padding(2, 1, 2, 1);
            Volume.Name = "Volume";
            Volume.Size = new Size(146, 45);
            Volume.TabIndex = 6;
            Volume.Scroll += Volume_Scroll;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(819, 565);
            Controls.Add(Volume);
            Controls.Add(frequency);
            Controls.Add(decayButton);
            Controls.Add(squareWaveButton);
            Controls.Add(sawtoothWaveButton);
            Controls.Add(triangleWaveButton);
            Controls.Add(playButton);
            Margin = new Padding(2, 1, 2, 1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)frequency).EndInit();
            ((System.ComponentModel.ISupportInitialize)Volume).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button playButton;
        private Button triangleWaveButton;
        private Button sawtoothWaveButton;
        private Button squareWaveButton;
        private Button decayButton;
        private TrackBar frequency;
        private TrackBar Volume;
    }
}