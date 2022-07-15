using System;
using System.Collections.Generic;
using System.Text;

namespace KeyboardLearner
{
    public class Score
    {
        // CONSTANTS
        public const int MaxPointValue = 100;
        public const int MinPointValue = 1;

        // PROPERTIES
        private int _total;
        private Profile _profile;
        private Level _level;

        // GETTERS AND SETTERS
        public int Total { get { return _total; } set { _total = value; } }
        public Profile Profile { get { return _profile; } set { _profile = value; } }
        public Level Level { get { return _level; } set { _level = value; } }

        // CONSTRUCTORS
        /// <summary>
        /// Default constructor. Mainly used for testing purposes.
        /// </summary>
        public Score(int total)
        {
            this._total = total;
            this._profile = null;
            this._level = null;
        }

        /// <summary>
        /// Full constructor.
        /// </summary>
        /// <param name="score">The initial score.</param>
        /// <param name="profile">The profile in play.</param>
        /// <param name="level">The level being played.</param>
        public Score(int score, Profile profile, Level level)
        {
            this._total = score;
            this._profile = profile;
            this._level = level;
        }

        // METHODS
        /// <summary>
        /// Increase the score amount by a given amount.
        /// </summary>
        /// <param name="amount">The amount to increase the score by. Should be between 0 and 100.</param>
        public void IncreaseScore(int amount)
        {
            this._total += amount;
        }

        /// <summary>
        /// Calculates the appropriate number of point to add to the overall score.
        /// Will stay within the range of the Min and Max point values.
        /// </summary>
        /// <param name="arrivalTime">The time at which the next key arrived at the keyboard.</param>
        /// <param name="keyPressedTime">The time at which the correct key was pressed.</param>
        /// <returns>The amount of points given.</returns>
        public int GivePoints(DateTime arrivalTime, DateTime keyPressedTime)
        {
            double timePassed = (double)Math.Abs((keyPressedTime - arrivalTime).TotalSeconds);
            int increase = ScoreFunction(1 / this._level.BeatsPerSecond(), timePassed);
            if(increase > MaxPointValue)
            {
                increase = MaxPointValue;
            }
            else if(increase < MinPointValue)
            {
                increase = MinPointValue;
            }
            IncreaseScore(increase);
            return increase;
        }

        /// <summary>
        /// The function used to calculate the score.
        /// </summary>
        /// <param name="secondsPerBeat">The number of seconds equal to one beat.</param>
        /// <param name="timePassed">The amount of time passed since the note arrived.</param>
        /// <returns>The appropriate score.</returns>
        private int ScoreFunction(double secondsPerBeat, double timePassed)
        {
            return Convert.ToInt32(MaxPointValue / ((1 / secondsPerBeat) * timePassed + 1));
        }

        /// <summary>
        /// Takes away the given amount from the score.
        /// Score cannot go below zero.
        /// </summary>
        /// <param name="amount">The amount of points taken away from the score.</param>
        /// <returns>The amount taken away from the score.</returns>
        public int TakePoints(int amount = 10)
        {
            if(this._total < amount)
            {
                this._total = 0;
            } else
            {
                this._total -= amount;
            }
            return -amount;
        }

        /// <summary>
        /// Inserts a scores record into the database.
        /// Also updates the user's
        /// </summary>
        /// <param name="score">The score object to be saved.</param>
        public static void SaveScore(Score score)
        {
            string query = Database.LoadQuery("save_score.sql");
            (string, dynamic)[] param =
            {
                ("@profile", score._profile.Name),
                ("@lvl", score._level.Title),
                ("@score", score._total),
                ("@accuracy", score.GetAccuracy())
            };
            Database.ExecuteQuery(query, param);
            //Update profile stats
            score._profile.CalculateWpm(); //Updates the profile WPM
            score._profile.CalculateFavoriteLevel(); //Updates the profile favorite level
            Profile.SaveProfile(score.Profile); //Saves the profile to the database to update the WPM
        }

        /// <summary>
        /// Return the percent accuracy of the total score. Calculated by dividing the score by the maximum possible score.
        /// </summary>
        /// <returns>The percentage between 0 and 1.</returns>
        public double GetAccuracy()
        {
            return (double)this._total / (MaxPointValue * this._level.NoteCount);
        }

    }
}
