using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyboardLearner
{
    public partial class ProfileStats : NavigationScreen
    {
        // PROPERTIES
        private Profile _profile;

        public ProfileStats(UserControl previous, Profile profile) : base(previous, "Profile Stats")
        {
            this._profile = profile;
            InitializeComponent();
            AddStats();
            AddDeleteButton();
        }

        /// <summary>
        /// Adds all graphical text to the container
        /// </summary>
        private void AddStats()
        {
            TableLayoutPanel tlp = new TableLayoutPanel();
            tlp.CellPaint += (s, e) => // for lines between stats 
            {
                var bottomLeft = new Point(e.CellBounds.Left, e.CellBounds.Bottom);
                var bottomRight = new Point(e.CellBounds.Right, e.CellBounds.Bottom);
                e.Graphics.DrawLine(Pens.DarkGray, bottomLeft, bottomRight);
            };
            tlp.AutoSize = true;
            string[] names = {
                "Profile Name",
                "Creation Date",
                "Levels Completed",
                "Estimated WPM",
                "Proficiency Level",
                "Favorite Level",
                "Progress"
            };
            string[] values = {
                _profile.Name,
                _profile.CreationDate.ToString("MMM d, yyyy"),
                _profile.GetLevelsCompletedCount().ToString(),
                _profile.Wpm.ToString(),
                _profile.GetProficiencyLevel(),
                _profile.FavLvl,
                _profile.GetProgress() + "%"
            };
            //Counts
            tlp.RowCount = names.Length;
            tlp.ColumnCount = 2;
            //Sizes
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 0.5F));
            tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 0.5F));
            //Text
            for(int row = 0; row < tlp.RowCount; row++)
            {
                //Label setup
                Label name = new Label();
                Label val = new Label();
                name.Margin = new Padding(0, 10, 0, 10);
                val.Margin = new Padding(0, 10, 0, 10);
                //Name
                name.Text = names[row] + ":";
                name.Font = new Font("Arial", 18);
                name.ForeColor = Color.Gold;
                name.AutoSize = true;
                name.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                name.TextAlign = ContentAlignment.MiddleLeft;
                //Value
                val.Text = values[row];
                val.Font = new Font("Arial", 18, FontStyle.Bold);
                val.ForeColor = Color.White;
                val.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                val.TextAlign = ContentAlignment.MiddleRight;
                //Add to control
                tlp.Controls.Add(name, 0, row);
                tlp.Controls.Add(val, 1, row);
            }
            this._contentContainer.Controls.Add(tlp);
        }

        private void AddDeleteButton()
        {
            Button delete = new Button();
            delete.Text = "Delete Profile";
            delete.Font = new Font("Cooper Black", 14);
            delete.BackColor = Color.Red;
            delete.ForeColor = Color.Black;
            delete.Size = new Size(200, 50);
            delete.FlatStyle = FlatStyle.Flat;
            delete.Margin = new Padding(0, 100, 0, 50);
            //button click
            delete.Click += (o, e) =>
            {
                string message = "Are you sure you want to delete this profile? This action cannot be undone.";
                string title = "Delete Profile?";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                var choice = MessageBox.Show(message, title, buttons, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
                if(choice == DialogResult.Yes)
                {
                    DeleteProfile();
                }
            };
            this._contentContainer.Controls.Add(delete);
        }

        private void DeleteProfile()
        {
            Profile.DeleteProfile(this._profile.Name);
            //Bring back user to a new profile selection screen
            ProfileSelection ps = new ProfileSelection(MainForm.Title);
            this.Parent.Controls.Add(ps);
            ps.BringToFront();
            ps.Focus();
            //Remove all these components
            this.Parent.Controls.Remove(this._previous);
            this._previous.Dispose();
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }
    }
}
