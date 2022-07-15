using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace KeyboardLearner
{
    class QwertyKey : PictureBox
    {
        // PROPERTIES
        private PianoKey _associatedKey;
        private char _associatedChar;
        private int _distance;
        private DateTime? _arrivalTime = null;
        private DateTime? _pressTime = null;

        // GETTERS AND SETTERS
        public PianoKey AssociatedKey { get { return _associatedKey; } set { _associatedKey = value; } }
        public char AssociatedChar { get { return _associatedChar; } set { _associatedChar = value; } }
        public int Distance { get { return _distance; } set { _distance = value; } }
        public DateTime? ArrivalTime { get { return _arrivalTime; } set { _arrivalTime = value; } }
        public DateTime? PressTime { get { return _pressTime; } set { _pressTime = value; } }

        // CONSTRUCTORS
        public QwertyKey(PianoKey key, char associatedChar, int distance)
        {
            this._associatedKey = key;
            this._associatedChar = associatedChar;
            this._distance = distance;

            // Image intialization
            this.Image = (Bitmap)Properties.Resources.ResourceManager.GetObject(associatedChar.ToString().ToLower());
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Size = new Size(key.Width, key.Width);
            this.BackColor = Color.Transparent;
        }
    }
}
