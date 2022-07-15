using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyboardLearner
{
    public partial class ProfileSelection : NavigationScreen
    {

        public ProfileSelection(UserControl previous) : base(previous, "Select a Profile")
        {
            InitializeComponent();
            AddProfiles();
            MainForm.Profiles = this;
        }

        private void AddProfiles()
        {
            List<Profile> profiles = Profile.GetAllProfiles();
            foreach(Profile profile in profiles)
            {
                //Create button
                Button b = new Button();
                b.Size = new Size(362, 100);
                b.Text = profile.Name;
                Color col = ColorTranslator.FromHtml($"#{profile.Color}");
                b.BackColor = col;
                b.ForeColor = (col.R + col.G + col.B < 255) ? Color.White : Color.Black;
                b.Font = new Font("Cooper Black", 18);

                //Add click control
                b.Click += new System.EventHandler(this.SelectProfile);

                //Add to container/control
                this._contentContainer.Controls.Add(b);
            }
            Button create = new Button();
            create.Size = new Size(362, 100);
            create.Text = "Create a New Profile";
            create.ForeColor = Color.Gray;
            create.BackColor = Color.LightGray;
            create.Font = new Font("Cooper Black", 18);
            create.FlatStyle = FlatStyle.Flat;
            create.Margin = new Padding(0,100,0,100);

            //Add click control
            create.Click += new System.EventHandler(this.CreateNewProfile);

            //Add to container/control
            this._contentContainer.Controls.Add(create);
        }

        private void SelectProfile(object sender, EventArgs e)
        {
            //Load the profile
            Button target = (Button)sender;
            Profile profile = Profile.LoadProfile(target.Text);
            //Load a new level selection screen
            LevelSelection ls = new LevelSelection(profile, this);
            this.Parent.Controls.Add(ls);
            ls.Focus();
            ls.BringToFront();
        }

        private void CreateNewProfile(object sender, EventArgs e)
        {
            ProfileCreation pc = new ProfileCreation(this);
            this.Parent.Controls.Add(pc);
            pc.BringToFront();
        }
    }
}
