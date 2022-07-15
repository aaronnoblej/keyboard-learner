
namespace KeyboardLearner
{
    partial class LevelSelection
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.playButton = new System.Windows.Forms.Label();
            this.statsButton = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // playButton
            // 
            this.playButton.AutoSize = true;
            this.playButton.Font = new System.Drawing.Font("Cooper Black", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.playButton.ForeColor = System.Drawing.Color.Gold;
            this.playButton.Location = new System.Drawing.Point(1755, 36);
            this.playButton.Name = "playButton";
            this.playButton.Size = new System.Drawing.Size(110, 46);
            this.playButton.TabIndex = 0;
            this.playButton.Text = "Play";
            // 
            // statsButton
            // 
            this.statsButton.AutoSize = true;
            this.statsButton.Font = new System.Drawing.Font("Cooper Black", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statsButton.ForeColor = System.Drawing.Color.Gold;
            this.statsButton.Location = new System.Drawing.Point(1755, 870);
            this.statsButton.Name = "statsButton";
            this.statsButton.Size = new System.Drawing.Size(126, 46);
            this.statsButton.TabIndex = 1;
            this.statsButton.Text = "Stats";
            // 
            // LevelSelection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Controls.Add(this.statsButton);
            this.Controls.Add(this.playButton);
            this.Name = "LevelSelection";
            this.Size = new System.Drawing.Size(1903, 1033);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label playButton;
        private System.Windows.Forms.Label statsButton;
    }
}
