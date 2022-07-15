
namespace KeyboardLearner
{
    partial class LevelComplete
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
            this.bodyContainer = new System.Windows.Forms.TableLayoutPanel();
            this.SuspendLayout();
            // 
            // bodyContainer
            // 
            this.bodyContainer.AutoScroll = true;
            this.bodyContainer.ColumnCount = 1;
            this.bodyContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.bodyContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyContainer.Location = new System.Drawing.Point(0, 0);
            this.bodyContainer.Name = "bodyContainer";
            this.bodyContainer.RowCount = 1;
            this.bodyContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.bodyContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 833F));
            this.bodyContainer.Size = new System.Drawing.Size(1903, 1033);
            this.bodyContainer.TabIndex = 6;
            // 
            // LevelComplete
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Controls.Add(this.bodyContainer);
            this.Name = "LevelComplete";
            this.Size = new System.Drawing.Size(1903, 1033);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel bodyContainer;
    }
}
