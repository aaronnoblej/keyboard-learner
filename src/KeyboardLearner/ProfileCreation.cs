using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KeyboardLearner
{
    public partial class ProfileCreation : NavigationScreen
    {
        // PROPERTIES
        TextBox nameInput = new TextBox();
        TextBox colorInput = new TextBox();

        public ProfileCreation(UserControl previous) : base(previous, "Create a Profile")
        {
            InitializeComponent();
            AddUIComponents();
        }

        private void AddUIComponents()
        {
            //Name text
            Label name = new Label();
            name.AutoSize = true;
            name.Text = "Profile Name";
            name.Font = new Font("Cooper Black", 18);
            name.ForeColor = Color.White;
            this._contentContainer.Controls.Add(name);

            //Name input
            nameInput.Size = new Size(350, 50);
            nameInput.Font = new Font("Cooper Black", 18);
            this._contentContainer.Controls.Add(nameInput);

            //Color text
            Label color = new Label();
            color.AutoSize = true;
            color.Text = "Profile Color";
            color.Font = new Font("Cooper Black", 18);
            color.ForeColor = Color.White;
            this._contentContainer.Controls.Add(color);

            //Color input
            colorInput.Size = new Size(350, 50);
            colorInput.Font = new Font("Arial", 18);
            colorInput.MaxLength = 6;
            this._contentContainer.Controls.Add(colorInput);

            //Save button
            Button save = new Button();
            save.Size = new Size(362, 100);
            save.Text = "Save Profile";
            save.BackColor = Color.White;
            save.Font = new Font("Cooper Black", 18);
            save.Margin = new Padding(0, 100, 0, 100);
            save.Click += new System.EventHandler(this.SaveProfile);
            this._contentContainer.Controls.Add(save);
        }

        private void SaveProfile(object sender, EventArgs e)
        {
            string pname = nameInput.Text;
            string color = colorInput.Text;
            int status = ValidateForm(pname, color);
            if (status != 0)
            {
                string title = "Error saving profile";
                string message = status switch
                {
                    1 => "A field is missing. Please try again.",
                    2 => "A profile with this name already exists. Please try again.",
                    3 => "Invalid color. Please try again.",
                    _ => "An unknown error has ocurred. Please try again.",
                };
                MessageBox.Show(message, title);
                return;
            }
            Profile p = new Profile(pname, color);
            Profile.SaveProfile(p);
            ProfileSelection ps = new ProfileSelection(this);
            this.Parent.Controls.Add(ps);
            ps.BringToFront();
            this.Parent.Controls.Remove(this._previous);
            this._previous.Dispose();
            this.Parent.Controls.Remove(this);
            this.Dispose();
        }

        /// <summary>
        /// Validates the name and color entries when creating a profile. Status codes returned are:
        /// 0: No errors
        /// 1: Field missing
        /// 2: Profile name already exists
        /// 3: Color is invalid
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        private int ValidateForm(string name, string color)
        {
            //empty string
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(color))
            {
                return 1;
            }
            //profile already exists
            else if (Profile.ProfileAlreadyExists(name))
            {
                return 2;
            }
            //bad color
            else if(!(color.Length == 6 && color.ToCharArray().All(c => "0123456789abcdefABCDEF".Contains(c))))
            {
                return 3;
            }
            return 0;
        }

        public new void Dispose()
        {
            base.Dispose();
            MainForm.Profiles = null;
        }
    }
}
