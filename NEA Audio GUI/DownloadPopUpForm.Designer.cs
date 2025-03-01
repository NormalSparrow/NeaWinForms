namespace NEA_Audio_GUI
{
    partial class DownloadPopupForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label label1;
        private TextBox AmountOfSecondsBox;
        private Button FinishButton;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            label1 = new Label();
            AmountOfSecondsBox = new TextBox();
            FinishButton = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(233, 79);
            label1.Name = "label1";
            label1.Size = new Size(152, 15);
            label1.TabIndex = 0;
            label1.Text = "Enter duration (in seconds):";
            // 
            // AmountOfSecondsBox
            // 
            AmountOfSecondsBox.Location = new Point(260, 111);
            AmountOfSecondsBox.Name = "AmountOfSecondsBox";
            AmountOfSecondsBox.Size = new Size(100, 23);
            AmountOfSecondsBox.TabIndex = 1;
            
            // 
            // FinishButton
            // 
            FinishButton.Location = new Point(269, 167);
            FinishButton.Name = "FinishButton";
            FinishButton.Size = new Size(75, 23);
            FinishButton.TabIndex = 2;
            FinishButton.Text = "Finish";
            FinishButton.Click += FinishButton_Click;
            // 
            // DownloadPopupForm
            // 
            ClientSize = new Size(657, 318);
            Controls.Add(label1);
            Controls.Add(AmountOfSecondsBox);
            Controls.Add(FinishButton);
            Name = "DownloadPopupForm";
            Text = "Download Audio Sample";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}