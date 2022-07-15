using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyboardLearner
{
    public partial class LevelComplete : UserControl
    {
        // CONSTANTS
        private readonly Color TextColor = Color.White;

        // PROPERTIES
        private Score _score;
        private FlowLayoutPanel _contentContainer = new FlowLayoutPanel();
        //Text labels
        private Label _title = new Label();
        private Label _levelName = new Label();
        private Label _scoreText = new Label();
        private Label _wpmText = new Label();
        private Label _continue = new Label();

        public LevelComplete(Score score)
        {
            this._score = score;
            InitializeComponent();
            ConfigureFlowLayoutContainer();
            AddGraphics();
            AddTextComponents();
        }

        /// <summary>
        /// Configures the FlowLayoutContainer used for centering components.
        /// </summary>
        private void ConfigureFlowLayoutContainer()
        {
            this._contentContainer.AutoSize = true;
            this._contentContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this._contentContainer.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom);
            this._contentContainer.AutoScroll = true;
            this._contentContainer.FlowDirection = FlowDirection.TopDown;
            bodyContainer.Controls.Add(this._contentContainer);
        }

        /// <summary>
        /// Adds musical graphics at each side of the UI
        /// </summary>
        private void AddGraphics()
        {
            //Left notes
            PictureBox left = new PictureBox();
            left.Image = Properties.Resources.eighth_notes;
            left.Dock = DockStyle.Left;
            left.SizeMode = PictureBoxSizeMode.CenterImage;
            left.Width = this.Width / 4;
            this.Controls.Add(left);

            //Right notes
            PictureBox right = new PictureBox();
            right.Image = Properties.Resources.eighth_notes;
            right.Dock = DockStyle.Right;
            right.SizeMode = PictureBoxSizeMode.CenterImage;
            right.Width = this.Width / 4;
            this.Controls.Add(right);
        }

        /// <summary>
        /// Adds all text labels to the screen.
        /// </summary>
        private void AddTextComponents()
        {
            //Title
            this._title.Text = "Level Completed!";
            this._title.TextAlign = ContentAlignment.MiddleCenter;
            this._title.Font = new Font("Cooper Black", 48);
            this._title.Width = this.Width / 2 - 20;
            this._title.Height = 250;
            this._title.ForeColor = TextColor;
            this._contentContainer.Controls.Add(_title);

            //Level name
            this._levelName.Text = $"\"{this._score.Level.Title}\"";
            this._levelName.TextAlign = ContentAlignment.MiddleCenter;
            this._levelName.Font = new Font("Cooper Black", 36);
            this._levelName.Width = this.Width / 2 - 20;
            this._levelName.Height = 150;
            this._levelName.ForeColor = Color.LightGray;
            _contentContainer.Controls.Add(_levelName);

            //Score
            int percentage = Convert.ToInt32(this._score.GetAccuracy() * 100);
            this._scoreText.Text = $"Score: {this._score.Total} ({percentage}%)";
            this._scoreText.TextAlign = ContentAlignment.MiddleCenter;
            this._scoreText.Font = new Font("Arial", 24, FontStyle.Bold);
            this._scoreText.Width = this.Width / 2 - 20;
            this._scoreText.Height = 100;
            this._scoreText.ForeColor = Color.Gold;
            this._contentContainer.Controls.Add(_scoreText);

            //WPM
            this._wpmText.Text = $"Current Estimated WPM: {this._score.Profile.Wpm}";
            this._wpmText.TextAlign = ContentAlignment.MiddleCenter;
            this._wpmText.Font = new Font("Arial", 20, FontStyle.Bold);
            this._wpmText.Width = this.Width / 2 - 20;
            this._wpmText.Height = 100;
            this._wpmText.ForeColor = TextColor;
            this._wpmText.Margin = new Padding(0, 0, 0, 50);
            this._contentContainer.Controls.Add(_wpmText);

            //Continue
            this._continue.Text = $"Press the spacebar to continue";
            this._continue.TextAlign = ContentAlignment.MiddleCenter;
            this._continue.Font = new Font("Cooper Black", 18);
            this._continue.Width = this.Width / 2 - 20;
            this._continue.Height = 75;
            this._continue.ForeColor = Color.LightGray;
            this._contentContainer.Controls.Add(_continue);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == Keys.Space)
            {
                LevelSelection ls = new LevelSelection(this._score.Profile, MainForm.Profiles);
                this.Parent.Controls.Add(ls);
                ls.BringToFront();
                ls.Focus();
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}
