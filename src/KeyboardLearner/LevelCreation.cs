using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyboardLearner
{
    public partial class LevelCreation : NavigationScreen
    {
        // CONSTANTS
        public static readonly string[] KeyOptions =
        {
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z",";"
        };

        // PROPERTIES
        private TextBox titleInput = new TextBox();
        private ComboBox difficultyInput = new ComboBox();
        private NumericUpDown bpmInput = new NumericUpDown();
        private TextBox pitchesInput = new TextBox();
        private TextBox rhythmsInput = new TextBox();
        private TableLayoutPanel mappingsInput = new TableLayoutPanel();

        private List<ComboBox> qwertyInputs = new List<ComboBox>();
        private List<string> levelPitches = new List<string>();

        public LevelCreation(UserControl previous) : base(previous, "Level Creator")
        {
            InitializeComponent();
            this._contentContainer.AutoScroll = false;
            this._contentContainer.WrapContents = false;
            AddUIComponents();
        }

        /// <summary>
        /// Clears all lists containing data relating to the mapping process.
        /// Should be used when resetting the mapping UI.
        /// </summary>
        private void ClearMappingInformation()
        {
            this.mappingsInput.Controls.Clear();
            this.qwertyInputs.Clear();
            this.levelPitches.Clear();
        }

        /// <summary>
        /// Generates the form and displays to the user.
        /// </summary>
        private void AddUIComponents()
        {
            //Title text
            Label title = new Label();
            title.AutoSize = true;
            title.Text = "Level Name";
            title.Font = new Font("Cooper Black", 18);
            title.ForeColor = Color.White;
            this._contentContainer.Controls.Add(title);

            //Title input
            titleInput.Size = new Size(500, 50);
            titleInput.Font = new Font("Arial", 12);
            this._contentContainer.Controls.Add(titleInput);

            //Difficulty text
            Label difficulty = new Label();
            difficulty.AutoSize = true;
            difficulty.Text = "Difficulty";
            difficulty.Font = new Font("Cooper Black", 18);
            difficulty.ForeColor = Color.White;
            this._contentContainer.Controls.Add(difficulty);

            //Difficulty input
            difficultyInput.Items.AddRange(Level.DifficultyNames);
            difficultyInput.DropDownStyle = ComboBoxStyle.DropDownList;
            difficultyInput.Size = new Size(500, 50);
            difficultyInput.Font = new Font("Arial", 12);
            this._contentContainer.Controls.Add(difficultyInput);

            //BPM text
            Label bpm = new Label();
            bpm.AutoSize = true;
            bpm.Text = "BPM";
            bpm.Font = new Font("Cooper Black", 18);
            bpm.ForeColor = Color.White;
            this._contentContainer.Controls.Add(bpm);

            //BPM input
            bpmInput.Size = new Size(500, 50);
            bpmInput.Font = new Font("Arial", 12);
            bpmInput.Minimum = 50;
            bpmInput.Maximum = 300;
            this._contentContainer.Controls.Add(bpmInput);

            //Pitches text
            Label pitches = new Label();
            pitches.AutoSize = true;
            pitches.Text = "Pitches";
            pitches.Font = new Font("Cooper Black", 18);
            pitches.ForeColor = Color.White;
            this._contentContainer.Controls.Add(pitches);

            //Pitches tooltip
            ToolTip pitchesTp = new ToolTip();
            string pitchesTpText = "A comma-separated list of pitches for the song. Each element in the list should consist of\na) the " +
                "pitch and\nb) the octave number.\nAll pitches should be lowercase and all black keys should be denoted as sharps.\nExample: c3,c#4,d2,a5";
            pitchesTp.SetToolTip(pitches, pitchesTpText);

            //Pitches input
            pitchesInput.Size = new Size(500, 100);
            pitchesInput.Multiline = true;
            pitchesInput.Font = new Font("Arial", 12);
            this._contentContainer.Controls.Add(pitchesInput);

            //Rhythm text
            Label rhythms = new Label();
            rhythms.AutoSize = true;
            rhythms.Text = "Rhythms";
            rhythms.Font = new Font("Cooper Black", 18);
            rhythms.ForeColor = Color.White;
            this._contentContainer.Controls.Add(rhythms);

            //Rhythm tooltip
            ToolTip rhythmTp = new ToolTip();
            string rhythmTpText = "A string of rhythm labels (see key below). This list is NOT comma delimited.\n" +
                "The number of rhythms should match the number of pitches above.\n" +
                "Example: 111188888888224";
            rhythmTp.SetToolTip(rhythms, rhythmTpText);

            //Rhythm input
            rhythmsInput.Size = new Size(500, 100);
            rhythmsInput.Multiline = true;
            rhythmsInput.Font = new Font("Arial", 12);
            this._contentContainer.Controls.Add(rhythmsInput);

            //Rhythm key
            Label key = new Label();
            key.AutoSize = true;
            key.Text = "Rhythm Key:\n" +
                "1 = Quarter Note\n" +
                "2 = Half Note\n" +
                "3 = Triplet Note\n" +
                "4 = Whole Note\n" +
                "6 = Sixteenth Note\n" +
                "8 = Eighth Note";
            key.Font = new Font("Arial", 10);
            key.ForeColor = Color.White;
            this._contentContainer.Controls.Add(key);

            // Mappings
            Label mappings = new Label();
            mappings.AutoSize = true;
            mappings.Margin = new Padding(0, 20, 0, 20);
            mappings.Text = "Keyboard Mappings";
            mappings.Font = new Font("Cooper Black", 18);
            mappings.ForeColor = Color.White;
            this._contentContainer.Controls.Add(mappings);

            //Button to create mappings
            Button createMappings = new Button();
            createMappings.Text = "Parse Pitches";
            createMappings.Size = new Size(200, 50);
            createMappings.BackColor = Color.White;
            createMappings.Click += (o, ea) =>
            {
                try
                {
                    string[] sorted = Level.SortPitches(pitchesInput.Text);
                    Level.ParseRhythm(rhythmsInput.Text);
                    CreateMappings(sorted);
                }
                catch(Exception)
                {
                    MessageBox.Show("A parsing issue occurred. Please review the pitches and rhythms and try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            this._contentContainer.Controls.Add(createMappings);

            //Mappings input
            this.mappingsInput.AutoSize = true;
            this.mappingsInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 0.3F));
            this.mappingsInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 0.7F));
            this._contentContainer.Controls.Add(mappingsInput);

            //Save level button
            Button save = new Button();
            save.Text = "Save Level";
            save.Size = new Size(200, 50);
            save.BackColor = Color.White;
            save.Margin = new Padding(0, 50, 0, 50);
            save.Click += Save_Click;
            this._contentContainer.Controls.Add(save);
        }

        /// <summary>
        /// Procedure to occur when Save is clicked. Includes validating the form, creating the level, and saving the level to the database.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_Click(object sender, EventArgs e)
        {
            //Validate
            if(!ValidateAllFields())
            {
                return;
            }
            //Create
            Level newLevel = InstantiateLevel();
            //Save
            Level.SaveLevel(newLevel);
            MessageBox.Show("Level successfully created!", "Success!");
            Close();
        }

        /// <summary>
        /// Validates all fields within the level form.
        /// </summary>
        /// <returns>True if all fields are valid. If false, gives a popup to the user specifying the failure.</returns>
        private bool ValidateAllFields()
        {
            string title = "Error creating level";
            //All fields filled out
            Control[] required = { titleInput, difficultyInput, bpmInput, pitchesInput, rhythmsInput };
            foreach(Control c in required)
            {
                if(string.IsNullOrEmpty(c.Text))
                {
                    MessageBox.Show("One or more fields missing. Please review and try again.", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            //Level name doesn't already exist
            if(Level.LevelAlreadyExists(this.titleInput.Text))
            {
                MessageBox.Show("Level name already exists. Please review and try again.", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //Mappings exist
            if(qwertyInputs.Count == 0 || levelPitches.Count == 0)
            {
                MessageBox.Show("Level must contain keyboard mappings. Try pressing the \"Parse Pitches\" button.", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            //All notes have a mapping
            foreach (ComboBox cb in this.qwertyInputs)
            {
                if (cb.SelectedItem == null)
                {
                    MessageBox.Show("All piano pitches require a keyboard mapping. Please review and try again.", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            //No duplicate mappings
            for(int i = 0; i < this.qwertyInputs.Count; i++)
            {
                for(int j = i+1; j < this.qwertyInputs.Count; j++)
                {
                    //Check if any matches
                    if(this.qwertyInputs[i].SelectedItem.ToString().Equals(this.qwertyInputs[j].SelectedItem.ToString()))
                    {
                        MessageBox.Show("Duplicate keyboard mappings. Please review and try again.", title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Creates a visual representation for the user to set piano to keyboard mappings.
        /// Always resets any existing mapping UI that may exist.
        /// </summary>
        /// <param name="pitches">An array of pitch strings. Should be produced by Level.ParsePitches or Level.SortPitches.</param>
        private void CreateMappings(string[] pitches)
        {
            ClearMappingInformation(); //reset any current mappings
            this.mappingsInput.Margin = new Padding(0, 10, 0, 50);
            this.mappingsInput.RowCount = pitches.Length + 1;
            this.mappingsInput.ColumnCount = 2;
            //Header
            Label pitchLabel = new Label();
            pitchLabel.Text = "Piano Key";
            pitchLabel.ForeColor = Color.White;
            pitchLabel.BackColor = Color.Black;
            pitchLabel.Dock = DockStyle.Fill;
            pitchLabel.TextAlign = ContentAlignment.MiddleCenter;
            Label qwertyLabel = new Label();
            qwertyLabel.Text = "Computer Key";
            qwertyLabel.ForeColor = Color.White;
            qwertyLabel.BackColor = Color.Black;
            qwertyLabel.Dock = DockStyle.Fill;
            qwertyLabel.TextAlign = ContentAlignment.MiddleCenter;
            this.mappingsInput.Controls.Add(pitchLabel, 0, 0);
            this.mappingsInput.Controls.Add(qwertyLabel, 1, 0);

            for (int row = 0; row < pitches.Length; row++)
            {
                string pitch = pitches[row];
                this.levelPitches.Add(pitch);
                Font font = new Font("Arial", 14, FontStyle.Bold);
                //Pitch name
                Label name = new Label();
                name.Text = pitch.ToUpper();
                name.ForeColor = Color.White;
                name.Font = font;
                name.TextAlign = ContentAlignment.MiddleCenter;
                name.Dock = DockStyle.Fill;
                this.mappingsInput.Controls.Add(name, 0, row+1);
                //Mapped Keyboard key
                ComboBox cb = new ComboBox();
                cb.DropDownStyle = ComboBoxStyle.DropDownList;
                cb.Font = font;
                cb.Items.AddRange(KeyOptions);
                cb.Dock = DockStyle.Fill;
                this.qwertyInputs.Add(cb);
                this.mappingsInput.Controls.Add(cb, 1, row+1);
            }
        }

        /// <summary>
        /// Translates a list of characters (keyboard chars) and a list of piano keys (pitch name strings) to mapping objects.
        /// </summary>
        /// <param name="qwertyKeys">A list of characters representing the QWERTY keyboard input.</param>
        /// <param name="pianoKeys">A list of piano note name strings of the level.</param>
        /// <returns>A list of mapping objects with the appropriate chars and strings associated with each other.</returns>
        private List<Mapping> MapKeysToNotes(List<char> qwertyKeys, List<string> pianoKeys)
        {
            // Right away make sure the lists are the same length
            if(qwertyKeys.Count != pianoKeys.Count)
            {
                throw new ArgumentException("Cannot map keys to notes: lists are not the same length");
            }
            // Else create mapping objects and store them in a list
            List<Mapping> mappings = new List<Mapping>();
            for(int i = 0; i < qwertyKeys.Count; i++)
            {
                Mapping map = new Mapping(qwertyKeys[i], pianoKeys[i]);
                mappings.Add(map);
            }
            return mappings;
        }

        /// <summary>
        /// Instantiate a new level object from the values form the page.
        /// Assumes that fields are validated at this point, but throws exception in case validation is missed.
        /// </summary>
        /// <returns>A new level object.</returns>
        public Level InstantiateLevel()
        {
            string title = titleInput.Text;
            int difficulty = Level.GetDifficultyValue(difficultyInput.Text);
            int bpm = Convert.ToInt32(bpmInput.Value);
            string rhythm = rhythmsInput.Text;
            string pitches = pitchesInput.Text;

            Level level = new Level(title, difficulty, bpm, rhythm, pitches);

            List<char> qwertyChars = new List<char>();
            foreach(ComboBox cb in this.qwertyInputs)
            {
                if(cb.SelectedItem == null)
                {
                    throw new Exception("Not all piano pitches have an associated keyboard mapping.");
                }
                else
                {
                    qwertyChars.Add(cb.SelectedItem.ToString()[0]);
                }
            }
            level.Mappings = MapKeysToNotes(qwertyChars, this.levelPitches);

            return level;
        }
    }
}