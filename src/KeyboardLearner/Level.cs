using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.CompilerServices;
using System.Drawing;

[assembly: InternalsVisibleTo("KeyboardLearner.Tests")]
namespace KeyboardLearner
{
    public class Level
    {
        // CONSTANTS
        public static readonly string[] DifficultyNames = { "Easy", "Medium", "Hard" };

        // PROPERTIES
        private string _title;
        private int _difficulty;
        private int _bpm;
        private int _noteCount;
        private double _beatCount;
        private string _rhythm;
        private string _pitches;
        private List<Mapping> _mappings;
        public enum Difficulties
        {
            Easy = 1,
            Medium = 2,
            Hard = 3
        };

        // GETTERS AND SETTERS
        public string Title { get { return _title; } set { _title = value; } }
        public int Difficulty { get { return _difficulty; } set { _difficulty = value; } }
        public int Bpm { get { return _bpm; } set { _bpm = value; } }
        public int NoteCount { get { return _noteCount; } set { _noteCount = value; } }
        public double BeatCount { get { return _beatCount; } set { _beatCount = value; } }
        public string Rhythm { get { return _rhythm; } set { _rhythm = value; } }
        public string Pitches { get { return _pitches; } set { _pitches = value; } }
        public List<Mapping> Mappings { get { return _mappings; } set { _mappings = value; } }

        // CONSTRUCTORS
        /// <summary>
        /// Default constructor. Should ONLY be used for testing purposes.
        /// </summary>
        public Level()
        {
            this._title = "";
            this._difficulty = 0;
            this._bpm = 0;
            this._noteCount = 0;
            this._rhythm = "";
            this._pitches = "";
            this._mappings = null;
        }
        /// <summary>
        /// Constructor for creating a new level. Does not require the note count or beat count, as this is calculated.
        /// </summary>
        /// <param name="title">The name of the level.</param>
        /// <param name="difficulty">the difficulty rating of the level (1-3).</param>
        /// <param name="bpm">The BPM of the level.</param>
        /// <param name="rhythm">The string of rhythms. Key is in the ParseRhythm doc.</param>
        /// <param name="pitches">The string of pitches. Key is in the ParsePitches doc.</param>
        public Level(string title, int difficulty, int bpm, string rhythm, string pitches)
        {
            this._title = title;
            this._difficulty = difficulty;
            this._bpm = bpm;
            if (!ValidateStrings(rhythm, pitches))
            {
                (string,string) corrected = ForceStringMatch(rhythm, pitches);
                rhythm = corrected.Item1;
                pitches = corrected.Item2;
            }
            this._rhythm = rhythm;
            this._pitches = pitches;
            this._noteCount = CountNotes();
            this._beatCount = GetBeatCount();
            this._mappings = new List<Mapping>();
        }

        /// <summary>
        /// Full constructor for a Level object. Used for loading existing levels.
        /// Note that mappings are not loaded.
        /// </summary>
        /// <param name="title">The name of the level.</param>
        /// <param name="difficulty">The difficulty rating of the level (1-3).</param>
        /// <param name="bpm">The BPM of the level.</param>
        /// <param name="noteCount">The number of notes in the level.</param>
        /// <param name="rhythm">The string of rhythms. Key is in the ParseRhythm doc.</param>
        /// <param name="pitches">The string of pitches. Key is in the ParsePitches doc.</param>
        public Level(string title, int difficulty, int bpm, int noteCount, double beatCount, string rhythm, string pitches)
        {
            this._title = title;
            this._difficulty = difficulty;
            this._bpm = bpm;
            this._noteCount = noteCount;
            this._beatCount = beatCount;
            this._rhythm = rhythm;
            this._pitches = pitches;
            this._mappings = new List<Mapping>();
        }

        // METHODS
        /// <summary>
        /// Inserts a level record into the database.
        /// </summary>
        /// <param name="level">The level object to be saved.</param>
        /// <param name="saveMappings">Indicates if the level mappings should also be saved. Defaulted to true.</param>
        public static void SaveLevel(Level level, bool saveMappings = true)
        {
            string query = Database.LoadQuery("save_level.sql");
            (string, dynamic)[] param =
            {
                ("@title", level._title),
                ("@difficulty", level._difficulty),
                ("@bpm", level._bpm),
                ("@note_cnt", level._noteCount),
                ("@beat_cnt", level._beatCount),
                ("@rhythm_str", level._rhythm),
                ("@pitch_str", level._pitches)
            };
            Database.ExecuteQuery(query, param);
            if (saveMappings)
            {
                foreach (Mapping map in level._mappings)
                {
                    Mapping.SaveMapping(map, level);
                }
            }
        }

        /// <summary>
        /// Checks if a level already exists for the given name.
        /// </summary>
        /// <param name="title">The profile name to verify.</param>
        /// <returns>True if a record with the given name already exists.</returns>
        public static bool LevelAlreadyExists(string title)
        {
            try
            {
                LoadLevel(title);
            }
            catch (InvalidOperationException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Retrieves the name, difficulty, BPM, and note count of every level in the database.
        /// Rhythm and pitches are excluded because they are only necessary when loading a singular level.
        /// </summary>
        /// <returns>A list of level instances retrieved from the database.</returns>
        public static List<Level> GetAllLevels()
        {
            List<Level> results = new List<Level>();
            string query = Database.LoadQuery("get_all_levels.sql");
            List<Dictionary<string, dynamic>> lvls = Database.ExecuteSelectQuery(query);
            foreach(Dictionary<string, dynamic> level in lvls)
            {
                Level temp = new Level(
                    (string)level["title"],
                    (int)level["difficulty"],
                    (int)level["bpm"],
                    (int)level["note_cnt"],
                    0,
                    null,
                    null);
                results.Add(temp);
            }
            return results;
        }

        /// <summary>
        /// Gets a specified level from the database.
        /// </summary>
        /// <param name="title">The name of the level; also the primary key of the table.</param>
        /// <returns>A new instance of the loaded level.</returns>
        public static Level LoadLevel(string title)
        {
            string query = Database.LoadQuery("load_level.sql");
            (string, dynamic)[] param =
            {
                ("@title", title)
            };
            Dictionary<string, dynamic> level = Database.ExecuteSelectQuery(query, param).First();
            Level temp = new Level(
                (string) level["title"],
                (int) level["difficulty"],
                (int) level["bpm"],
                (int) level["note_cnt"],
                (double) level["beat_cnt"],
                (string) level["rhythm_str"],
                (string)level["pitch_str"]);
            temp.LoadMappings();
            return temp;
        }

        /// <summary>
        /// Converts a rhythm string into an array of integers, signifying the number of beats for each note
        /// 1 = quarter note, 2 = half note, 4 = whole note, 8 = eighth note, 6 = sixteenth note, 3 = triplet
        /// Example: "1121884"
        /// </summary>
        public double[] ParseRhythm()
        {
            double[] results = new double[this._rhythm.Length];
            for(int i = 0; i < results.Length; i++)
            {
                switch(this._rhythm[i])
                {
                    case '1':
                        results[i] = 1;
                        break;
                    case '2':
                        results[i] = 2;
                        break;
                    case '3':
                        results[i] = 0.333;
                        break;
                    case '4':
                        results[i] = 4;
                        break;
                    case '6':
                        results[i] = 0.25;
                        break;
                    case '8':
                        results[i] = 0.5;
                        break;
                    default:
                        throw new Exception("Invalid rhythm character.");
                }
            }
            return results;
        }

        /// <summary>
        /// Static version of ParseRhythm for a rhythm string. See non-static method for details.
        /// </summary>
        /// <param name="rhythms">The string to parse.</param>
        /// <returns>An array of rhythm lengths.</returns>
        public static double[] ParseRhythm(string rhythms)
        {
            double[] results = new double[rhythms.Length];
            for (int i = 0; i < results.Length; i++)
            {
                switch (rhythms[i])
                {
                    case '1':
                        results[i] = 1;
                        break;
                    case '2':
                        results[i] = 2;
                        break;
                    case '3':
                        results[i] = 0.333;
                        break;
                    case '4':
                        results[i] = 4;
                        break;
                    case '6':
                        results[i] = 0.25;
                        break;
                    case '8':
                        results[i] = 0.5;
                        break;
                    default:
                        throw new Exception("Invalid rhythm character.");
                }
            }
            return results;
        }

        /// <summary>
        /// Gets the number of beats in the level.
        /// </summary>
        /// <returns>The number of beats.</returns>
        public double GetBeatCount()
        {
            double[] beats = ParseRhythm();
            double sum = 0;
            foreach(double beat in beats)
            {
                sum += beat;
            }
            return sum;
        }

        /// <summary>
        /// Counts the total number of notes from the instance's pitches.
        /// </summary>
        /// <returns></returns>
        public int CountNotes()
        {
            return this._rhythm.Length;
        }

        /// <summary>
        /// Calculates the number of beats per second of the level's tempo.
        /// </summary>
        /// <returns>The beats per second calculation.</returns>
        public double BeatsPerSecond()
        {
            return (double)this._bpm / 60;
        }

        /// <summary>
        /// Adds a mapping to the Level.
        /// </summary>
        /// <param name="map">The mapping to be added.</param>
        public void AddMapping(Mapping map)
        {
            this._mappings.Add(map);
        }

        /// <summary>
        /// Adds multiple mappings to a level. Must be in an enumerable object.
        /// </summary>
        /// <param name="maps">The collection of maps to be added.</param>
        public void AddMappings(IEnumerable<Mapping> maps)
        {
            foreach(Mapping map in maps)
            {
                AddMapping(map);
            }
        }

        /// <summary>
        /// Loads the level key mappings from the database and sets them to the instance's _mapping var.
        /// </summary>
        public void LoadMappings()
        {
            string query = Database.LoadQuery("load_level_mappings.sql");
            (string, dynamic)[] param =
            {
                ("@lvl", this._title)
            };
            List<Dictionary<string, dynamic>> mappings = Database.ExecuteSelectQuery(query, param);
            foreach (Dictionary<string, dynamic> map in mappings)
            {
                Mapping temp = new Mapping(
                    Convert.ToChar((Convert.ToInt32(map["qwerty"]))),
                    (string)map["key"]);
                this.AddMapping(temp);
            }
        }

        /// <summary>
        /// Ensures that the rhythm and pitches are of the same length.
        /// </summary>
        /// <param name="rhythm">The rhythm string to be checked.</param>
        /// <param name="pitches">The pitch string to be checked.</param>
        /// <returns>True if the strings are the same length.</returns>
        public bool ValidateStrings(string rhythm, string pitches)
        {
            return rhythm.Length == pitches.Split(',').Length;
        }

        /// <summary>
        /// If rhythm  and pitches are different lengths, makes them match by shortening the longer string.
        /// </summary>
        /// <param name="rhythm">The rhythm string to be forced.</param>
        /// <param name="pitches">The pitches string to be forced.</param>
        /// <returns>The rhythm and pitches, adjusted.</returns>
        public (string,string) ForceStringMatch(string rhythm, string pitches)
        {
            string adjRhythm = rhythm;
            string adjPitches = pitches;
            if (rhythm.Length < pitches.Split(',').Length)
            {
                int minLen = rhythm.Length;
                adjPitches = String.Join(',', pitches.Split(',').Take(minLen));
            }
            else
            {
                int minLen = pitches.Split(',').Length;
                adjRhythm = rhythm.Substring(0, minLen);
            }
            return (adjRhythm, adjPitches);
        }

        /// <summary>
        /// Translates the numerical difficulty into a name.
        /// </summary>
        /// <returns></returns>
        public string GetDifficultyName()
        {
            return this._difficulty switch
            {
                1 => "Easy",
                2 => "Medium",
                3 => "Hard",
                _ => "Unknown",
            };
        }

        /// <summary>
        /// Translates a difficulty name into an integer.
        /// </summary>
        /// <param name="difficulty">The name of the difficulty.</param>
        /// <returns>The integer associated with the given difficulty.</returns>
        public static int GetDifficultyValue(string difficulty)
        {
            return difficulty switch
            {
                "Easy" => 1,
                "Medium" => 2,
                "Hard" => 3,
                _ => 0,
            };
        }

        /// <summary>
        /// Gets the associated color to a difficulty.
        /// </summary>
        /// <returns>The color associated with the level difficulty.</returns>
        public Color GetDifficultyColor()
        {
            return this._difficulty switch
            {
                1 => Color.FromArgb(194, 255, 194),
                2 => Color.FromArgb(255, 255, 194),
                3 => Color.FromArgb(255, 194, 194),
                _ => Color.White,
            };
        }

        /// <summary>
        /// Calculates the number of notes per second.
        /// </summary>
        /// <returns>Notes per second.</returns>
        public double GetNotesPerSecond()
        {
            double notes = this._noteCount;
            double beats = this._beatCount;
            double notesPerBeat = notes / beats;
            double bps = this.BeatsPerSecond();
            return notesPerBeat * bps;
        }

        /// <summary>
        /// Sorts a pitch string by location on keyboard.
        /// </summary>
        /// <returns>An array of pitch names in the correct order</returns>
        public static string[] SortPitches(string pitches)
        {
            var sorted = PianoKey.ParsePitches(pitches)
                .Distinct()
                .OrderBy(PianoKey.GetRangeValue)
                .ToArray();
            return sorted;
        }
    }
}