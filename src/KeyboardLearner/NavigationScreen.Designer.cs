
namespace KeyboardLearner
{
    partial class NavigationScreen
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
            this.titleText = new System.Windows.Forms.Label();
            this.bodyContainer = new System.Windows.Forms.TableLayoutPanel();
            this.backButton = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.backButton)).BeginInit();
            this.SuspendLayout();
            // 
            // titleText
            // 
            this.titleText.AccessibleRole = System.Windows.Forms.AccessibleRole.TitleBar;
            this.titleText.Dock = System.Windows.Forms.DockStyle.Top;
            this.titleText.Font = new System.Drawing.Font("Cooper Black", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.titleText.ForeColor = System.Drawing.SystemColors.HighlightText;
            this.titleText.Location = new System.Drawing.Point(0, 0);
            this.titleText.Name = "titleText";
            this.titleText.Padding = new System.Windows.Forms.Padding(50);
            this.titleText.Size = new System.Drawing.Size(1903, 200);
            this.titleText.TabIndex = 4;
            this.titleText.Text = "Title";
            this.titleText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // bodyContainer
            // 
            this.bodyContainer.AutoScroll = true;
            this.bodyContainer.ColumnCount = 1;
            this.bodyContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.bodyContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bodyContainer.Location = new System.Drawing.Point(0, 200);
            this.bodyContainer.Name = "bodyContainer";
            this.bodyContainer.RowCount = 1;
            this.bodyContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.bodyContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 833F));
            this.bodyContainer.Size = new System.Drawing.Size(1903, 833);
            this.bodyContainer.TabIndex = 5;
            // 
            // backButton
            // 
            this.backButton.Image = global::KeyboardLearner.Properties.Resources.back;
            this.backButton.Location = new System.Drawing.Point(26, 24);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(109, 146);
            this.backButton.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.backButton.TabIndex = 6;
            this.backButton.TabStop = false;
            this.backButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // NavigationScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.bodyContainer);
            this.Controls.Add(this.titleText);
            this.Name = "NavigationScreen";
            this.Size = new System.Drawing.Size(1903, 1033);
            ((System.ComponentModel.ISupportInitialize)(this.backButton)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        protected System.Windows.Forms.TableLayoutPanel bodyContainer;
        protected System.Windows.Forms.Label titleText;
        private System.Windows.Forms.PictureBox backButton;
    }
}
