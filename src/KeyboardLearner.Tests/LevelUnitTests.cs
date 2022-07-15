using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using KeyboardLearner;

namespace KeyboardLearner.Tests
{
    public class LevelUnitTests
    {
        // ValidateStrings
        [Fact]
        public void ValidateStrings_MatchingLengths_ReturnsTrue()
        {
            Level l = new Level();
            l.Rhythm = "1234";
            l.Pitches = "c1,c2,b4,d5";
            bool result = l.ValidateStrings(l.Rhythm, l.Pitches);
            Assert.True(result);
        }

        [Fact]
        public void ValidateStrings_MismatchedLengths_ReturnsFalse()
        {
            Level l = new Level();
            l.Rhythm = "123411144";
            l.Pitches = "c1,c2,b4,d5";
            bool result = l.ValidateStrings(l.Rhythm, l.Pitches);
            Assert.False(result);
        }

        // ForceStringMatch
        [Fact]
        public void ForceStringMatch_RhythmLonger_ShortensRhythm()
        {
            Level l = new Level();
            l.Rhythm = "123411144";
            l.Pitches = "c1,c2,b4,d5";
            (string, string) result = l.ForceStringMatch(l.Rhythm, l.Pitches);
            string expRhythm = "1234";
            string expPitches = "c1,c2,b4,d5";
            Assert.Equal(expRhythm, result.Item1);
            Assert.Equal(expPitches, result.Item2);
        }

        [Fact]
        public void ForceStringMatch_PitchesLonger_ShortensPitches()
        {
            Level l = new Level();
            l.Rhythm = "1234";
            l.Pitches = "c1,c2,b4,d5,c1,c4";
            (string, string) result = l.ForceStringMatch(l.Rhythm, l.Pitches);
            string expRhythm = "1234";
            string expPitches = "c1,c2,b4,d5";
            Assert.Equal(expRhythm, result.Item1);
            Assert.Equal(expPitches, result.Item2);
        }

        // GetDifficultyName
        [Fact]
        public void GetDifficultyName_Value1_ReturnsEasy()
        {
            Level l = new Level();
            l.Difficulty = 1;
            string expResult = "Easy";
            string result = l.GetDifficultyName();
            Assert.Equal(expResult, result);
        }

        [Fact]
        public void GetDifficultyName_Value2_ReturnsMedium()
        {
            Level l = new Level();
            l.Difficulty = 2;
            string expResult = "Medium";
            string result = l.GetDifficultyName();
            Assert.Equal(expResult, result);
        }

        [Fact]
        public void GetDifficultyName_Value3_ReturnsHard()
        {
            Level l = new Level();
            l.Difficulty = 3;
            string expResult = "Hard";
            string result = l.GetDifficultyName();
            Assert.Equal(expResult, result);
        }

        [Fact]
        public void GetDifficultyName_OtherValue_ReturnsUnknown()
        {
            Level l = new Level();
            l.Difficulty = -1;
            string expResult = "Unknown";
            string result = l.GetDifficultyName();
            Assert.Equal(expResult, result);
        }

        // CountNotes
        [Fact]
        public void CountNotes_ValidRhythmString_ReturnsStringLength()
        {
            Level l = new Level();
            l.Rhythm = "1234";
            int expResult = 4;
            int result = l.CountNotes();
            Assert.Equal(expResult, result);
        }

        // BeatsPerSecond
        [Fact]
        public void BeatsPerSecond_100bpm_DividesBpmBy60()
        {
            Level l = new Level();
            l.Bpm = 100;
            double expResult = 100.0/60.0;
            double result = l.BeatsPerSecond();
            Assert.Equal(expResult, result);
        }

        // ParseRhythm
        [Fact]
        public void ParseRhythm_ParseOneOfEachRhythm_AllSuccessfullyParsed()
        {
            Level l = new Level();
            l.Rhythm = "123468";
            double[] expResult = { 1, 2, 0.333, 4, 0.25, 0.5 };
            double[] result = l.ParseRhythm();
            Assert.Equal(expResult, result);
        }

        // SortPitches
        [Fact]
        public void SortPitches_MixedNotes_SortAndEliminateDuplicates()
        {
            string pitches = "e4,e4,c2,a1,b7,c2";
            string[] expResult = { "a1", "c2", "e4", "b7" };
            string[] result = Level.SortPitches(pitches);
            Assert.Equal(expResult, result);
        }
    }
}
