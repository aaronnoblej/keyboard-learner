using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Drawing;
using System.Media;
using NAudio.Wave;
using System.IO;

[assembly: InternalsVisibleTo("KeyboardLearner.Tests")]
namespace KeyboardLearner
{
    class PianoKey : Button
    {
        // PROPERTIES
        private string _note;
        private string _filename;
        private WaveOut _sound;

        // GETTERS AND SETTERS
        public string Note { get { return _note; } set { _note = value; } }
        public string Filename { get { return _filename; } set { _filename = value; } }
        public char Qwerty { get; set; }

        // CONSTRUCTORS
        /// <summary>
        /// Used for specifying all attributes explicitly.
        /// </summary>
        /// <param name="note">The note name (scientific notation).</param>
        /// <param name="file">The path and filename of the audio.</param>
        private PianoKey(string note, string file, bool showNoteNames = true)
        {
            this._note = note;
            this._filename = $"{Program.BaseDirectory}\\{file}";
            // Need to initially load the sound so that the first note does not lag
            this._sound = new WaveOut();
            this._sound.Init(new WaveFileReader(this._filename));

            // Key styling
            this.FlatStyle = FlatStyle.Flat;
            if (this.IsWhiteKey())
            {
                this.BackColor = Color.White;
            }
            else
            {
                this.BackColor = Color.Black;
            }
            if(showNoteNames)
            {
                this.Text = note.ToUpper()[0..^1];
                this.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
                this.Padding = new Padding(0, 0, 0, 10);
                this.ForeColor = Color.Gray;
            }
            // Play sound on click
            this.Click += PlayKey_Click;
            // Initially disable selecting
            this.SetStyle(ControlStyles.Selectable, false);
        }

        /// <summary>
        /// Used to generate the filename automatically.
        /// </summary>
        /// <param name="note">The note name (scientific notation).</param>
        public PianoKey(string note)
        {
            this._note = note;
            this._filename = $"{Program.BaseDirectory}audio\\piano\\{note}.wav";
            //this._sound = new SoundPlayer(this._filename);
            //this._sound.Load();

            this.FlatStyle = FlatStyle.Flat;
            if (this.IsWhiteKey())
            {
                this.BackColor = Color.White;
            }
            else
            {
                this.BackColor = Color.Black;
            }
            // Play sound on click
            //this.Click += (sender, args) =>
            //{
            //    PlayKey();
            //};
        }

        // METHODS
        /// <summary>
        /// Gets a specified key from the database.
        /// </summary>
        /// <param name="note">The name of the note; also the primary key of the table.</param>
        /// <returns>A new instance of the loaded piano key.</returns>
        public static PianoKey LoadKey(string note)
        {
            string query = Database.LoadQuery("load_key.sql");
            (string, dynamic)[] param =
            {
                ("@note", note)
            };
            Dictionary<string, dynamic> key = Database.ExecuteSelectQuery(query, param).First();
            PianoKey temp = new PianoKey(
                (string)key["note"],
                @$"{key["filename"]}");
            return temp;
        }

        /// <summary>
        /// Allows the key to be pressed, as all PianoKeys are initially disabled.
        /// </summary>
        public void Enable()
        {
            this.SetStyle(ControlStyles.Selectable, true);
        }

        /// <summary>
        /// Finds the octave number of the note.
        /// </summary>
        /// <returns>The octave number as an integer.</returns>
        public int GetOctave()
        {
            return this._note.Last() - '0';
        }

        /// <summary>
        /// Static version of GetOctave, but takes in a string.
        /// </summary>
        /// <param name="note">The name of the note.</param>
        /// <returns>The octave number as an integer.</returns>
        public static int GetOctave(string note)
        {
            return note.Last() - '0';
        }

        /// <summary>
        /// Calculates a numerical value unique to the note.
        /// Used for comparing it's physical piano location to others in a collection.
        /// </summary>
        /// <returns>The value of the note.</returns>
        public int GetRangeValue()
        {
            int octave = (GetOctave() - '0') * Keyboard.PianoNotes.Length;
            int noteVal = Array.IndexOf(Keyboard.PianoNotes, this._note[0] - '0');
            return octave + noteVal;
        }

        /// <summary>
        /// Static method for calculating a numerical value unique to the note. Takes in a string.
        /// </summary>
        /// <param name="note">The name of the note to be calculated.</param>
        /// <returns>The value of the note.</returns>
        public static int GetRangeValue(string note)
        {
            int octave = GetOctave(note) * Keyboard.PianoNotes.Length;
            int noteVal = Array.IndexOf(Keyboard.PianoNotes, note[0..^1]);
            return octave + noteVal;
        }

        /// <summary>
        /// Indicates if the key is a white key.
        /// </summary>
        /// <returns>True if key is a white key.</returns>
        public bool IsWhiteKey()
        {
            return this._note[1] != '#';
        }

        public static bool IsWhiteKey(string note)
        {
            return note[1] != '#';
        }

        /// <summary>
        /// Converts a pitch string into an array of strings, each value representing a note.
        /// Also validates the the pitch name exists
        /// </summary>
        /// <param name="pitches"></param>
        /// <returns>An array of note pitches, as strings.</returns>
        public static string[] ParsePitches(string pitches)
        {
            string[] result = pitches.Split(',');
            //Validate each pitch, throw exception if non-existent note found
            foreach(string pitch in result)
            {
                if(!Keyboard.PianoNotes.Contains(pitch[0..^1]))
                {
                    throw new ArgumentException("Invalid note name.");
                }
            }
            return result;
            
        }

        /// <summary>
        /// Plays the pitch on the speakers from mouse input.
        /// </summary>
        public void PlayKey_Click(object sender, EventArgs args)
        {
            this._sound = new WaveOut();
            this._sound.Init(new WaveFileReader(this._filename));
            this._sound.Play();
        }

    }
}
