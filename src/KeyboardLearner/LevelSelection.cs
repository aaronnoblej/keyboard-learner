using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyboardLearner
{
    public partial class LevelSelection : NavigationScreen
    {
        // PROPERTIES
        Profile _profile;
        LevelBox _selected = null;

        // CONSTRUCTORS
        public LevelSelection(Profile profile, UserControl previous) : base(previous, "Select a Level")
        {
            this._profile = profile;
            InitializeComponent();
            CreateHeader();
            AddLevels();
            //Play button
            this.playButton.BringToFront();
            this.playButton.ForeColor = Color.LightGray;
            this.playButton.Cursor = Cursors.Hand;
            this.playButton.Click += Play;
            //Stats button
            this.statsButton.BringToFront();
            this.statsButton.Cursor = Cursors.Hand;
            this.statsButton.Click += Stats;
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(keyData == (Keys.L | Keys.Alt) && this._profile.Role == 1)
            {
                ToLevelCreator();
                Close();
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // METHODS
        /// <summary>
        /// Setter for the selected property.
        /// </summary>
        /// <param name="levelBox">The LevelBox that was selected.</param>
        private void SetSelected(LevelBox levelBox)
        {
            this._selected = levelBox;
            playButton.ForeColor = Color.Gold;
        }

        /// <summary>
        /// Add LevelBox objects to the main container. Loads all levels from the database.
        /// </summary>
        private void AddLevels()
        {
            List<Level> levels = Level.GetAllLevels();
            foreach(Level level in levels)
            {
                LevelBox lb = new LevelBox(_profile, level, this);
                this._contentContainer.Controls.Add(lb);
            }
        }

        private void CreateHeader()
        {
            //Header object
            TableLayoutPanel header = new TableLayoutPanel();
            header.Size = new Size(1000, 50);
            //Counts
            header.ColumnCount = 5;
            header.RowCount = 1;
            //Sizes
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50 * 0.33F));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50 * 0.66F));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
            header.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
            //Color
            header.BackColor = Color.FromArgb(105, 105, 105); //Dark gray color
            header.ForeColor = Color.White;
            //Text
            for (int i = 0; i < header.ColumnCount; i++)
            {
                Label text = new Label();
                text.Font = new Font("Arial", 14);
                switch (i)
                {
                    case 0:
                        text.Text = "Difficulty";
                        break;
                    case 1:
                        text.Text = "Level Name";
                        break;
                    case 2:
                        text.Text = "BPM";
                        break;
                    case 3:
                        text.Text = "Note Count";
                        break;
                    case 4:
                        text.Text = "High Score";
                        break;
                }
                text.AutoSize = true;
                text.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                text.TextAlign = ContentAlignment.MiddleCenter;
                header.Controls.Add(text, i, 0);
            }
            //this._levels.Controls.Add(header);
            this._contentContainer.Controls.Add(header);
        }

        /// <summary>
        /// Loads and initates a level after being selected.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Play(object sender, EventArgs e)
        {
            if(_selected != null)
            {
                Level lvl = Level.LoadLevel(this._selected.Level.Title);
                LevelPlay game = new LevelPlay(this._profile, lvl);
                this.Parent.Controls.Add(game);
                game.BringToFront();
                game.Focus();
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }
        }

        private void Stats(object sender, EventArgs e)
        {
            ProfileStats ps = new ProfileStats(this, this._profile);
            this.Parent.Controls.Add(ps);
            ps.BringToFront();
            ps.Focus();
        }

        public void ToLevelCreator()
        {
            LevelCreation lc = new LevelCreation(this);
            this.Parent.Controls.Add(lc);
            lc.BringToFront();
            lc.Focus();
        }

        // INNER CLASSES
        private class LevelBox : TableLayoutPanel
        {
            // CONSTANTS
            public readonly Color SelectedColor = Color.LightGray;
            public readonly Color DeselectedColor = Color.White;

            // PROPERTIES
            private Level _level;
            private Profile _profile;
            private LevelSelection _parent;

            // GETTERS AND SETTERS
            public Level Level { get { return _level; } set { _level = value; } }

            // CONSTRUCTORS
            public LevelBox(Profile profile, Level level, LevelSelection parent)
            {
                this._profile = profile;
                this._level = level;
                this._parent = parent;
                //Component setup
                SetupComponents();
                this.Size = new Size(1000, 100);
                this.Cursor = Cursors.Hand;
                //Set click action
                this.Click += new System.EventHandler(this.Select);
            }

            // METHODS
            /// <summary>
            /// Set the selected cell to the clicked cell.
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void Select(object sender, EventArgs e)
            {
                if(this._parent._selected != null) //executes if a different level is already selected
                {
                    this._parent._selected.BackColor = DeselectedColor;
                }
                this._parent.SetSelected(this);
                this.BackColor = SelectedColor;
            }

            /// <summary>
            /// Sets up all the UI components for the LevelBox.
            /// </summary>
            private void SetupComponents()
            {
                //Counts
                ColumnCount = 5;
                RowCount = 1;
                //Sizes
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50 * 0.33F));
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50 * 0.66F));
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15));
                ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
                //Color
                BackColor = DeselectedColor;
                //Text
                for(int i = 0; i < ColumnCount; i++)
                {
                    Label text = new Label();
                    text.Font = new Font("Cooper Black", 14);
                    switch(i)
                    {
                        case 0:
                            text.Text = this._level.GetDifficultyName();
                            text.BackColor = this._level.GetDifficultyColor();
                            break;
                        case 1:
                            text.Text = this._level.Title;
                            break;
                        case 2:
                            text.Text = this._level.Bpm.ToString();
                            break;
                        case 3:
                            text.Text = this._level.NoteCount.ToString();
                            break;
                        case 4:
                            text.Text = this._profile.GetBestScore(_level);
                            break;
                    }
                    text.AutoSize = true;
                    text.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                    text.TextAlign = ContentAlignment.MiddleCenter;
                    //Pass clicks
                    text.Click += (o, ea) => { InvokeOnClick(this, ea); };
                    this.Controls.Add(text, i, 0);
                }
            }
        }
    }
}