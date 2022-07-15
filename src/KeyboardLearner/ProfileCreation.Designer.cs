
namespace KeyboardLearner
{
    partial class ProfileCreation
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
            this.profileContainer = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.profileContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // profileContainer
            // 
            this.profileContainer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.profileContainer.ColumnCount = 1;
            this.profileContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.profileContainer.Controls.Add(this.tableLayoutPanel1, 0, 0);
            this.profileContainer.Location = new System.Drawing.Point(0, 0);
            this.profileContainer.Name = "profileContainer";
            this.profileContainer.RowCount = 1;
            this.profileContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.profileContainer.Size = new System.Drawing.Size(200, 100);
            this.profileContainer.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 833F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(194, 94);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // ProfileCreation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Name = "ProfileCreation";
            this.Size = new System.Drawing.Size(1903, 1033);
            this.profileContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel profileContainer;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
