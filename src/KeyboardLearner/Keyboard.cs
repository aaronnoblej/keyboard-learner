﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace KeyboardLearner
{
    class Keyboard : Panel
    {
        // CONSTANTS
        public static readonly string[] PianoNotes = { "c", "c#", "d", "d#", "e", "f", "f#", "g", "g#", "a", "a#", "b" };

        // PROPERTIES
        private List<PianoKey> _keys;
        private Label _scoreMarker { get; set; }

        public int WhiteKeyCount { get; set; }

        // GETTERS AND SETTERS
        public List<PianoKey> Keys { get { return _keys; } }
        public Label ScoreMarker { get { return _scoreMarker; } set { _scoreMarker = value; } }
        public int WhiteKeyWidth { get { return this.Width / this.WhiteKeyCount; } }
        public int WhiteKeyHeight { get { return this.Height; } }

        // CONSTRUCTORS
        public Keyboard(string pitches)
        {
            WhiteKeyCount = 1;
            // Panel initialization
            this.Width = 1903;
            this.Height = 485;
            // Keys initialization
            string[] pitchArray = PianoKey.ParsePitches(pitches);
            string start = GetLowestKey(pitchArray);
            string end = GetHighestKey(pitchArray);
            this._keys = CreateKeys(start, end);
            this._scoreMarker = new Label();
            this._scoreMarker.Visible = false;
            this.Controls.Add(_scoreMarker);
            AdjustKeySizesAndLocations();
        }

        /// <summary>
        /// Finds the pitch with the lowest range value in the specified pitch string.
        /// </summary>
        /// <param name="pitches">The parsed pitches. Should be generated by the ParsePitches function.</param>
        /// <returns>The lowest note in the pitch string, as a string.</returns>
        private string GetLowestKey(string[] pitches)
        {
            string lowest = pitches[0];
            foreach(string pitch in pitches)
            {
                if(PianoKey.GetRangeValue(pitch) < PianoKey.GetRangeValue(lowest))
                {
                    lowest = pitch;
                }
            }
            return lowest;
        }

        private void ColorScoreMarker(PianoKey pressed, int points)
        {
            Color c;
            if (points < 0)
            {
                c = Color.Red;
            }
            else
            {
                int interval = 255 / Score.MaxPointValue;
                int green = points * interval;
                int red = 255 - green;
                c = Color.FromArgb(red, green, 0);
            }
            this._scoreMarker.ForeColor = c;
            this._scoreMarker.BackColor = pressed.BackColor;
        }

        public void ShowScoreMarker(PianoKey pressed, int points)
        {
            //Location
            this._scoreMarker.Width = Convert.ToInt32(pressed.Width * 0.9);
            this._scoreMarker.Height = 100;
            this._scoreMarker.Location = new Point(pressed.Location.X + 3, pressed.Bottom - this._scoreMarker.Height - 5);
            //Content
            this._scoreMarker.Text = points.ToString();
            this._scoreMarker.TextAlign = ContentAlignment.MiddleCenter;
            this._scoreMarker.Font = new Font("Arial", pressed.Width / 5);
            ColorScoreMarker(pressed, points);
            this._scoreMarker.Visible = true;
            this._scoreMarker.BringToFront();
        }

        /// <summary>
        /// Finds the pitch with the highest range value in the specified pitch string.
        /// </summary>
        /// /// <param name="pitches">The parsed pitches. Should be generated by the ParsePitches function.</param>
        /// <returns>The highest note in the pitch string, as a string.</returns>
        private string GetHighestKey(string[] pitches)
        {
            string highest = pitches[0];
            foreach (string pitch in pitches)
            {
                if (PianoKey.GetRangeValue(pitch) > PianoKey.GetRangeValue(highest))
                {
                    highest = pitch;
                }
            }
            return highest;
        }

        /// <summary>
        /// Enables all keys on the keyboard.
        /// </summary>
        public void EnableKeys()
        {
            foreach(PianoKey key in this._keys)
            {
                key.Enable();
            }
        }

        /// <summary>
        /// Generates all necessary piano keys within the range of the lowest and highest notes.
        /// </summary>
        /// <param name="lowest">The note nate of the lowest pitch (scientific notation).</param>
        /// <param name="highest">The note nate of the lowest pitch (scientific notation).</param>
        /// <returns></returns>
        private List<PianoKey> CreateKeys(string lowest, string highest)
        {
            List<PianoKey> keys = new List<PianoKey>();
            // FIND THE START
            // find lowest note's location in the array
            int startIndex = 0;
            int startOctave = lowest[^1] - '0';
            for(int i = 0; i < PianoNotes.Length; i++)
            {
                string notename = lowest[0..^1];
                if(notename.Equals(PianoNotes[i]))
                {
                    startIndex = i;
                    break;
                }
            }
            // keep going down until the start note is a white key
            do
            {
                startIndex--;
                if (startIndex < 0)
                {
                    startIndex = PianoNotes.Length - 1;
                    startOctave--;
                }
            } while (!PianoKey.IsWhiteKey(PianoNotes[startIndex] + startOctave));
            string startingPitch = PianoNotes[startIndex] + startOctave;

            PianoKey start = PianoKey.LoadKey(startingPitch);

            // FIND THE END
            // find highest note's location in the array
            int endIndex = 0;
            int endOctave = highest[^1] - '0';
            for (int i = 0; i < PianoNotes.Length; i++)
            {
                string notename = highest[0..^1];
                if (notename.Equals(PianoNotes[i]))
                {
                    endIndex = i;
                    break;
                }
            }
            // keep going up until the start note is a white key
            do
            {
                endIndex++;
                if (endIndex >= PianoNotes.Length)
                {
                    endIndex = 0;
                    endOctave++;
                }
            } while (!PianoKey.IsWhiteKey(PianoNotes[endIndex] + endOctave));
            string endingPitch = PianoNotes[endIndex] + endOctave;

            PianoKey end = PianoKey.LoadKey(endingPitch);

            // Create notes until end
            PianoKey current = start;
            int index = startIndex;
            int octave = startOctave;
            while(!current.Note.Equals(endingPitch))
            {
                keys.Add(current);
                if (current.IsWhiteKey()) WhiteKeyCount++;
                index++;
                if(index >= PianoNotes.Length)
                {
                    index = 0;
                    octave++;
                }
                current = PianoKey.LoadKey(PianoNotes[index] + octave);
            }
            keys.Add(end);
            return keys;
        }

        /// <summary>
        /// Changes every PianoKey object's width and location based on the total amount of keys on the keyboard.
        /// </summary>
        public void AdjustKeySizesAndLocations()
        {
            int blackKeyWidth = WhiteKeyWidth / 2;
            int blackKeyHeight = Convert.ToInt32(this.Height * 0.6);

            int currentX = 0; //Current position of the key iteration
            PianoKey prev = null;
            foreach(PianoKey key in this._keys)
            {
                // Update location
                if(prev != null)
                {
                    if(prev.IsWhiteKey())
                    {
                        if (key.IsWhiteKey()) // white to white
                        {
                            currentX += WhiteKeyWidth;
                        }
                        else // white to black
                        {
                            currentX += WhiteKeyWidth - (blackKeyWidth / 2);
                        }
                    }
                    else // black to white
                    {
                        currentX += blackKeyWidth / 2;
                    }
                }
                key.Location = new Point(currentX, 0);

                // Update size
                if (key.IsWhiteKey())
                {
                    key.Width = WhiteKeyWidth;
                    key.Height = WhiteKeyHeight;
                    key.SendToBack();
                }
                else
                {
                    key.Width = blackKeyWidth;
                    key.Height = blackKeyHeight;
                }
                this.Controls.Add(key);
                prev = key;
            }
            // Puts all white keys behind black keys
            foreach (PianoKey key in this._keys)
            {
                if (key.IsWhiteKey())
                {
                    key.SendToBack();
                }
            }
        }

        /// <summary>
        /// Finds the appropriate PianoKey instance of the indicated note.
        /// </summary>
        /// <param name="qwerty">The name of the note to be found.</param>
        /// <returns>The PianoKey instance with the associated note. Null if not found.</returns>
        public PianoKey FindKey(string note)
        {
            return this._keys.Find(x => x.Note.Equals(note));
        }

        
    }
}