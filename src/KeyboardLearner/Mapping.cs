using System;
using System.Collections.Generic;
using System.Text;

namespace KeyboardLearner
{
    public class Mapping
    {
        // PROPERTIES
        private char _qwerty;
        private string _key;

        // GETTERS AND SETTERS
        public char Qwerty { get { return _qwerty; } set { _qwerty = value; } }
        public string Key { get { return _key; } set { _key = value; } }

        // CONSTRUCTORS
        /// <summary>
        /// Full constructor for a Mapping object. Used for loading existing mappings.
        /// </summary>
        /// <param name="qwerty">The computer keyboard character to be mapped.</param>
        /// <param name="key">The associated PianoKey instance.</param>
        public Mapping(char qwerty, string key)
        {
            this._qwerty = qwerty;
            this._key = key;
        }

        // METHODS
        /// <summary>
        /// Inserts a mapping record into the database.
        /// </summary>
        /// <param name="map">The mapping object to be saved.</param>
        /// <param name="level">The level for which the map belongs</param>
        public static void SaveMapping(Mapping map, Level level)
        {
            string query = Database.LoadQuery("save_mapping.sql");
            (string, dynamic)[] param =
            {
                ("@lvl", level.Title),
                ("@key", map._key),
                ("@qwerty", map._qwerty)
            };
            Database.ExecuteQuery(query, param);
        }
    }
}
