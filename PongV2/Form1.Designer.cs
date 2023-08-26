namespace PongV2
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
            Ohjain2 = new PictureBox();
            Ohjain1 = new PictureBox();
            Pallo = new PictureBox();
            Tulos1 = new Label();
            ((System.ComponentModel.ISupportInitialize)Ohjain2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Ohjain1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Pallo).BeginInit();
            SuspendLayout();
            // 
            // Ohjain2
            // 
            Ohjain2.BackColor = SystemColors.ControlLightLight;
            Ohjain2.Location = new Point(778, 3);
            Ohjain2.Name = "Ohjain2";
            Ohjain2.Size = new Size(10, 438);
            Ohjain2.TabIndex = 0;
            Ohjain2.TabStop = false;
            // 
            // Ohjain1
            // 
            Ohjain1.BackColor = SystemColors.ActiveCaptionText;
            Ohjain1.Location = new Point(12, 25);
            Ohjain1.Name = "Ohjain1";
            Ohjain1.Size = new Size(27, 63);
            Ohjain1.TabIndex = 1;
            Ohjain1.TabStop = false;
            // 
            // Pallo
            // 
            Pallo.BackColor = SystemColors.Highlight;
            Pallo.Location = new Point(419, 197);
            Pallo.Name = "Pallo";
            Pallo.Size = new Size(18, 20);
            Pallo.TabIndex = 2;
            Pallo.TabStop = false;
            // 
            // Tulos1
            // 
            Tulos1.AutoSize = true;
            Tulos1.Location = new Point(12, 416);
            Tulos1.Name = "Tulos1";
            Tulos1.Size = new Size(59, 25);
            Tulos1.TabIndex = 4;
            Tulos1.Text = "label1";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(Tulos1);
            Controls.Add(Pallo);
            Controls.Add(Ohjain1);
            Controls.Add(Ohjain2);
            Name = "Form1";
            Text = "Form1";
            Load += Form1_Load_1;
            KeyDown += Form1_KeyDown;
            KeyUp += Form1_KeyUp;
            ((System.ComponentModel.ISupportInitialize)Ohjain2).EndInit();
            ((System.ComponentModel.ISupportInitialize)Ohjain1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Pallo).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox Ohjain2;
        private PictureBox Ohjain1;
        private PictureBox Pallo;
        private Label Tulos1;
    }
}