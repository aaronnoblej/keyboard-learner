using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KeyboardLearner
{
    public class Profile
    {
        // PROPERTIES
        private string _name;
        private string _color;
        private DateTime _creationDate;
        private int _levelsCompleted;
        private int _wpm;
        private string _fav_lvl;
        private int _role;
        public enum ProficiencyLevels {
            Slow = 0,
            Average = 25,
            Fluent = 45,
            Fast = 60,
            Pro = 80
        }

        // GETTERS AND SETTERS
        public string Name { get { return _name; } set { _name = value; } }
        public DateTime CreationDate { get { return _creationDate; } set { _creationDate = value; } }
        public string Color { get { return _color; } set { _color = value; } }
        public int Wpm { get { return _wpm; } set { _wpm = value; } }
        public string FavLvl { get { return _fav_lvl; } set { _fav_lvl = value; } }
        public int Role { get { return _role; } set { _role = value; } }

        // CONSTRUCTORS
        /// <summary>
        /// Constructor used for creating a new profile. Only requires the name and color.
        /// </summary>
        /// <param name="name">The name of the profile. Must be unique.</param>
        /// <param name="color">The six-digit hexadecimal color code. Will default to #FFFFFF if invalid color.</param>
        /// <param name="role">The role of the user. Defaults to 0 or "User". Use 1 for "Admin"</param>
        public Profile(string name, string color, int role = 0)
        {
            this._name = name;
            this._color = ValidateColor(color) ? color : "FFFFFF";
            this._creationDate = DateTime.Now;
            this._levelsCompleted = 0;
            this._wpm = 0;
            this._fav_lvl = "None";
            this._role = role;
        }

        /// <summary>
        /// Full constructor for a Profile object. Used for loading existing profiles.
        /// </summary>
        /// <param name="name">The name of the profile.</param>
        /// <param name="color">The six-digit hexadecimal color code.</param>
        /// <param name="creationDate">The date and time the profile was created.</param>
        /// <param name="levelsCompleted">The number of levels completed.</param>
        /// <param name="wpm">The estimated WPM of the profile.</param>
        /// <param name="fav_lvl">The most played level.</param>
        /// <param name="role">The group role of the user.</param>
        public Profile(string name, string color, DateTime creationDate, int levelsCompleted, int wpm, string fav_lvl, int role)
        {
            this._name = name;
            this._color = color;
            this._creationDate = creationDate;
            this._levelsCompleted = levelsCompleted;
            this._wpm = wpm;
            this._fav_lvl = fav_lvl;
            this._role = role;
        }

        // METHODS
        /// <summary>
        /// Gets a specified profile from the database.
        /// </summary>
        /// <param name="p_name">The name of the profile; also the primary key of the table.</param>
        /// <returns>A new instance of the loaded profile.</returns>
        public static Profile LoadProfile(string p_name)
        {
            string query = Database.LoadQuery("load_profile.sql");
            (string,dynamic)[] param =
            {
                ("@p_name", p_name)
            };
            Dictionary<string, dynamic> profile = Database.ExecuteSelectQuery(query, param).First();
            Profile temp = new Profile(
                (string) profile["p_name"],
                (string) profile["color"],
                (DateTime) profile["create_date"],
                (int) profile["lvls_completed"],
                (int) profile["wpm"],
                (string) profile["fav_lvl"],
                (int) profile["group_id"]);
            return temp;
        }

        /// <summary>
        /// Retrieves the name and color of every profile in the database.
        /// Other fields are excluded because they are only necessary when loading a singular profile.
        /// </summary>
        /// <returns>A list of profile instances retrieved from the database.</returns>
        public static List<Profile> GetAllProfiles()
        {
            List<Profile> results = new List<Profile>();
            string query = Database.LoadQuery("get_all_profiles.sql");
            List<Dictionary<string, dynamic>> profiles = Database.ExecuteSelectQuery(query);
            foreach (Dictionary<string, dynamic> profile in profiles)
            {
                Profile temp = new Profile(
                    (string)profile["p_name"],
                    (string)profile["color"],
                    DateTime.Now,
                    0,
                    0,
                    null,
                    (int)profile["group_id"]);
                results.Add(temp);
            }
            return results;
        }

        /// <summary>
        /// Inserts a profile record into the database.
        /// </summary>
        /// <param name="profile">The profile object to be saved.</param>
        public static void SaveProfile(Profile profile)
        {
            string query = Database.LoadQuery("save_profile.sql");
            (string,dynamic)[] param =
            {
                ("@p_name", profile._name),
                ("@color", profile._color),
                ("@create_date", profile._creationDate),
                ("@lvls_completed", profile._levelsCompleted),
                ("@wpm", profile._wpm),
                ("@fav_lvl", profile._fav_lvl),
                ("@group_id", profile._role)
            };
            Database.ExecuteQuery(query, param);
        }

        /// <summary>
        /// Deletes a profile record from the database, as well as any scores associated with it.
        /// </summary>
        /// <param name="profile">The profile to be deleted</param>
        public static void DeleteProfile(string p_name)
        {
            string query = Database.LoadQuery("delete_profile.sql");
            (string, dynamic)[] param =
            {
                ("@p_name", p_name)
            };
            Database.ExecuteQuery(query, param);
        }

        /// <summary>
        /// Checks if a profile already exists for the given name.
        /// </summary>
        /// <param name="pname">The profile name to verify.</param>
        /// <returns>True if a record with the given name already exists.</returns>
        public static bool ProfileAlreadyExists(string pname)
        {
            try
            {
                LoadProfile(pname);
            }
            catch(InvalidOperationException)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Assures that the string "_color" is in the correct format (6 characters, between 0 and F hexadecimal)
        /// </summary>
        /// <param name="color">The color string to be validated.</param>
        /// <returns>True if an appropriate color, false if not.</returns>
        private bool ValidateColor(string color)
        {
            return color.Length == 6 && color.ToCharArray().All(c => "0123456789abcdefABCDEF".Contains(c));
        }

        /// <summary>
        /// Fetches the user's accuracy scores for all levels, as well as the level's BPM, note count, and beat count.
        /// Also updates the profile property _wpm.
        /// </summary>
        /// <returns>The calculated WPM.</returns>
        public void CalculateWpm()
        {
            string query = Database.LoadQuery("get_wpm_info.sql");
            (string, dynamic)[] param =
            {
                ("@profile", this._name)
            };
            List<Dictionary<string, dynamic>> scores = Database.ExecuteSelectQuery(query, param);
            int[] wpms = new int[scores.Count()];
            int charsPerWord = 5;
            for (int i = 0; i < scores.Count(); i++)
            {
                Dictionary<string, dynamic> row = scores[i];
                int bpm = (int)row["bpm"];
                int noteCount = (int)row["note_cnt"];
                double beatCount = (double)row["beat_cnt"];
                double accuracy = (double)row["accuracy"];
                int wpm = (int)Math.Round((((noteCount / beatCount) * bpm) / charsPerWord) * accuracy);
                wpms[i] = wpm;
            }
            int est_wpm = Convert.ToInt32(wpms.Average());
            this._wpm = est_wpm;
        }

        /// <summary>
        /// Returns the proficiency level based on the user's WPM.
        /// </summary>
        /// <returns>The name of proficiency level.</returns>
        public string GetProficiencyLevel()
        {
            string proficiency;
            if(this._wpm >= (int)ProficiencyLevels.Pro)
            {
                proficiency = nameof(ProficiencyLevels.Pro);
            }
            else if (this._wpm >= (int)ProficiencyLevels.Fast)
            {
                proficiency = nameof(ProficiencyLevels.Fast);
            }
            else if (this._wpm >= (int)ProficiencyLevels.Fluent)
            {
                proficiency = nameof(ProficiencyLevels.Fluent);
            }
            else if (this._wpm >= (int)ProficiencyLevels.Average)
            {
                proficiency = nameof(ProficiencyLevels.Average);
            }
            else
            {
                proficiency = nameof(ProficiencyLevels.Slow);
            }
            return proficiency;
        }

        public void CalculateFavoriteLevel()
        {
            string query = Database.LoadQuery("get_favorite_level.sql");
            (string, dynamic)[] param =
            {
                ("@profile", this._name),
            };
            Dictionary<string, dynamic> result = Database.ExecuteSelectQuery(query, param).First();
            this._fav_lvl = result["lvl"];
        }

        public string GetBestScore(Level level)
        {
            string query = Database.LoadQuery("load_best_score.sql");
            (string, dynamic)[] param =
            {
                ("@profile", this._name),
                ("@lvl", level.Title)
            };
            Dictionary<string, dynamic> result = Database.ExecuteSelectQuery(query, param).First();
            int? score = (int?)result["score"];
            double? accuracy = (double?)result["accuracy"];
            if(score == null || accuracy == null)
            {
                return "- -";
            }
            string formattedString = $"{score} ({Convert.ToInt32(accuracy * 100)}%)";
            return formattedString;
        }

        public int GetLevelsCompletedCount()
        {
            string query = Database.LoadQuery("get_levels_completed.sql");
            (string, dynamic)[] param =
            {
                ("@profile", this._name),
            };
            Dictionary<string, dynamic> result = Database.ExecuteSelectQuery(query, param).First();
            return (int)result["num"];
        }

        public int GetProgress()
        {
            string query = Database.LoadQuery("get_levels_count.sql");
            (string, dynamic)[] param =
            {
                ("@profile", this._name),
            };
            Dictionary<string, dynamic> result = Database.ExecuteSelectQuery(query, param).First();
            double count = (int)result["num"];
            return Convert.ToInt32(((double)GetLevelsCompletedCount() / count) * 100);
        }
    }
}
