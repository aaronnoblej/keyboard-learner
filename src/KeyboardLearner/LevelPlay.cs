using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace KeyboardLearner
{
    partial class LevelPlay : UserControl
    {
        // CONSTANTS
        public const int ScreenHeight = 1033;
        public const int ScreenWidth = 1903;
        public const int Sensitivity = 50; //Note must be within 50 pixels to be considered

        // PROPERTIES
        private Profile _profile;
        private Level _level;
        private Score _score;
        private Keyboard _keyboard;
        private Queue<QwertyKey> _keys;
        private int endCounter = 0;
        private bool started = false;

        // easily obtainable mappings (unfortunately need two dictionaries...)
        private Dictionary<PianoKey, char> PianoToKeyboardMappings { get; set; }
        private Dictionary<char, PianoKey> KeyboardToPianoMappings { get; set; }

        private List<QwertyKey> Fading { get; set; }
        private QwertyKey NextKey { get { return this._keys.Peek(); } }
        private Label starter = new Label();
        public readonly int PixelsPerSecond;

        // CONSTRUCTORS
        public LevelPlay(Profile profile, Level level)
        {
            // Instance vars
            this._profile = profile;
            this._level = level;
            this._score = new Score(0, profile, level);

            //Keyboard
            this._keyboard = new Keyboard(level.Pitches);
            this._keyboard.Location = new Point(0, ScreenHeight - this._keyboard.Height - 50);
            this.Controls.Add(_keyboard);

            //Bars
            this.Paint += (o, e) =>
            {
                int counter = 0;
                Graphics g = e.Graphics;
                using Brush b = new SolidBrush(Color.DarkGray);
                foreach (PianoKey pk in this._keyboard.Keys)
                {
                    if (pk.IsWhiteKey())
                    {
                        if (counter % 2 == 0)
                        {
                            int xPos = pk.Location.X;
                            int yPos = 0;
                            int width = pk.Width;
                            int height = ScreenHeight - pk.Location.Y;
                            g.FillRectangle(b, xPos, yPos, width, height);
                        }
                        counter++;
                    }
                }
            };
            //Mappings
            PianoToKeyboardMappings = new Dictionary<PianoKey, char>();
            KeyboardToPianoMappings = new Dictionary<char, PianoKey>();
            InitializeMappings();

            //Speed
            PixelsPerSecond = level.Bpm * 3 + _keyboard.WhiteKeyWidth;
            //PixelsPerSecond = Convert.ToInt32(level.GetNotesPerSecond() * 100 + _keyboard.WhiteKeyWidth);

            //Qwertys
            this._keys = ParseLevelKeys();
            Fading = new List<QwertyKey>();
            SetQwertyPositions();

            //Components
            InitializeComponent();

            // Starter text
            starter.Text = $"\"{level.Title}\"\nPress Enter or Space to begin";
            starter.Font = new Font("Arial", 24);
            starter.Size = new Size(this.Width, 200);
            starter.Location = new Point(0, this._keyboard.Location.Y - 200);
            starter.ForeColor = Color.White;
            starter.BackColor = Color.Transparent;
            starter.AutoSize = false;
            starter.TextAlign = ContentAlignment.MiddleCenter;
            this.Controls.Add(starter);

        }

        // METHODS
        private void LevelPlay_Load(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Populate the two dictionaries with chars to PianoKeys and vice versa.
        /// </summary>
        private void InitializeMappings()
        {
            foreach (Mapping map in this._level.Mappings)
            {
                PianoKey pk = this._keyboard.FindKey(map.Key);
                PianoToKeyboardMappings.Add(pk, map.Qwerty);
                KeyboardToPianoMappings.Add(map.Qwerty, pk);
            }
        }

        /// <summary>
        /// Executes each time a QWERTY keyboard input is given.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if(!started)
            {
                if(keyData == Keys.Space || keyData == Keys.Enter)
                {
                    Start();
                }
            }
            else if (!IsOver())
            {
                char input = TranslateKeyToChar(keyData);
                // Make sound
                if (KeyboardToPianoMappings.ContainsKey(input))
                {
                    PianoKey key = KeyboardToPianoMappings[input];
                    key.PerformClick();
                    // Set fade
                    if (IsCorrectNote(input))
                    {
                        NextKey.PressTime = DateTime.Now;
                        if (KeyPressedWithinRange())
                        {
                            if (NextKey.ArrivalTime == null)
                            {
                                NextKey.ArrivalTime = GetEstimatedArrivalTime(NextKey);
                            }
                            int points = this._score.GivePoints((DateTime)NextKey.ArrivalTime, (DateTime)NextKey.PressTime);
                            Fading.Add(this._keys.Dequeue());
                            // Show score increase to user
                            this._keyboard.ShowScoreMarker(key, points);
                        }
                        else
                        {
                            int points = this._score.TakePoints();
                            this._keyboard.ShowScoreMarker(key, points);
                        }
                    }
                    else
                    {
                        int points = this._score.TakePoints();
                        this._keyboard.ShowScoreMarker(key, points);
                    }
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Converts a system key into a character.
        /// </summary>
        /// <param name="key">The keyboard input to be translated.</param>
        /// <returns>The char associated with the given key.</returns>
        private char TranslateKeyToChar(Keys key)
        {
            return key != Keys.Oem1 ? (char)key : ';';
        }

        /// <summary>
        /// Checks if the input matches the next note.
        /// Note that the input char MUST be a valid mapping
        /// </summary>
        /// <param name="input">The input char.</param>
        /// <returns>True if the input matches the next note.</returns>
        private bool IsCorrectNote(char input)
        {
            return KeyboardToPianoMappings[input] == KeyboardToPianoMappings[NextKey.AssociatedChar];
        }

        /// <summary>
        /// Converts rhythm and pitch strings of the level into graphical objects with correct distances between.
        /// </summary>
        /// <returns>A queue of QwertyKey objects.</returns>
        private Queue<QwertyKey> ParseLevelKeys()
        {
            Queue<QwertyKey> notes = new Queue<QwertyKey>();
            double[] rhythm = this._level.ParseRhythm();
            string[] pitches = PianoKey.ParsePitches(this._level.Pitches);
            for (int i = 0; i < rhythm.Length; i++)
            {
                PianoKey note = this._keyboard.FindKey(pitches[i]);
                char qwerty = PianoToKeyboardMappings[note];
                int distance = Convert.ToInt32((PixelsPerSecond * (rhythm[i] / _level.BeatsPerSecond()))); //pixels after this note
                QwertyKey temp = new QwertyKey(note, qwerty, distance);
                notes.Enqueue(temp);
            }
            return notes;
        }

        /// <summary>
        /// Sets the location of each QwertyKey object in the keys collection.
        /// </summary>
        private void SetQwertyPositions()
        {
            int y = 0; //position of bottom
            foreach(QwertyKey qk in this._keys)
            {
                int x = qk.AssociatedKey.Location.X;
                qk.Location = new Point(x, y - qk.Width);
                this.Controls.Add(qk);

                y -= qk.Distance;
            }
        }

        /// <summary>
        /// Starts the level by beginning the level timer and allowing notes to come down to the keyboard.
        /// </summary>
        public void Start()
        {
            this._keyboard.EnableKeys();
            this._keyboard.Focus();
            started = true;
            starter.Dispose();
            this.timer.Start();
            timer.Tick += Timer_Tick;
        }

        /// <summary>
        /// Checks if the queue of QWERTY keys is empty.
        /// Also stops the timer if the game is over.
        /// </summary>
        /// <returns>True if the level is over.</returns>
        public bool IsOver()
        {
            if(!KeysRemain() && !KeysFading())
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if queue of QWERTY keys is empty.
        /// </summary>
        /// <returns>True if there are still keys remaining.</returns>
        private bool KeysRemain()
        {
            return _keys.Count != 0;
        }

        /// <summary>
        /// Checks if the list of fading keys is empty.
        /// </summary>
        /// <returns>True if there are still keys fading.</returns>
        private bool KeysFading()
        {
            return Fading.Count != 0;
        }

        /// <summary>
        /// Executes each time the timer's interval passes.
        /// Animated events go in here.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            if(KeysRemain())
            {
                if (NextKey.Bottom <= this._keyboard.Top)
                {
                    int ips = 1000 / timer.Interval; //intervals per second
                    int move = (PixelsPerSecond / ips) * 2;
                    foreach (QwertyKey qk in this._keys)
                    {
                        qk.Top += move;
                    }
                }
                else // executed when key has arrived
                {
                    if(NextKey.ArrivalTime == null) // if not set, set the arrival time
                    {
                        NextKey.ArrivalTime = DateTime.Now;
                    }
                }
            }
            // Execute when a note has been pressed and needs to fade away
            Fade();

            //Check if the game is over
            if(IsOver())
            {
                EndGame();
            }
        }

        /// <summary>
        /// Executes steps needed to end the level, including holding for five seconds before continuing navigation.
        /// </summary>
        private void EndGame()
        {
            endCounter += this.timer.Interval;
            if(endCounter >= 3000)
            {
                Score.SaveScore(this._score);
                this.timer.Stop();
                LevelComplete lc = new LevelComplete(this._score);
                this.Parent.Controls.Add(lc);
                lc.BringToFront();
                lc.Focus();
                this.Parent.Controls.Remove(this);
                this.Dispose();
            }
        }

        /// <summary>
        /// Indicates if the time the key was pressed was pressed within the sensitivity level.
        /// </summary>
        /// <returns>True if the next QwertyKey is within the specified sensitivity.</returns>
        private bool KeyPressedWithinRange()
        {
            if(NextKey.PressTime != null)
            {
                return this._keyboard.Location.Y - NextKey.Bottom <= Sensitivity;
            }
            return false;
        }

        /// <summary>
        /// Used when the key is pressed before arriving. Calculates the time the key should arrive at.
        /// </summary>
        /// <param name="key">The name of the key. Typically used 'NextKey'</param>
        /// <returns>The DateTime in which the key should arrive at the keyboard.</returns>
        private DateTime GetEstimatedArrivalTime(QwertyKey key)
        {
            int distance = this._keyboard.Location.Y - key.Bottom;
            double secs = (double)distance / (double)PixelsPerSecond;
            return DateTime.Now.AddSeconds(secs);
        }

        /// <summary>
        /// Code that controls how QwertyKey objects fade away and dispose after being pressed.
        /// Current just disappears with no effect, but may wish to add other effects.
        /// <param name="shrinkFactor"/>The amount to shrink by each time. Defaulted at 10 pixels.</param>
        /// </summary>
        private void Fade(int shrinkFactor = 10)
        {
            List<QwertyKey> toRemove = new List<QwertyKey>();
            foreach(QwertyKey qk in Fading)
            {
                if(qk.Width <= shrinkFactor || qk.Height <= shrinkFactor)
                {
                    toRemove.Add(qk);
                    qk.Dispose();
                }
                else
                {
                    qk.Width = qk.Width - shrinkFactor;
                    qk.Height = qk.Height - shrinkFactor;
                    qk.Top += shrinkFactor;
                    qk.Left += shrinkFactor / 2;
                }
            }
            foreach(QwertyKey qk in toRemove)
            {
                Fading.Remove(qk);
            }
        }
    }
}
