using System;
using System.Collections.Generic;
using System.Text;
using KeyboardLearner;
using Xunit;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("KeyboardLearner.Tests")]
namespace KeyboardLearner.Tests
{
    public class ScoreUnitTests
    {
        // GivePoints
        [Fact]
        public void GivePoints_NoDelay_GiveMaxPointValue()
        {
            Level l = new Level();
            l.Bpm = 100;
            Score s = new Score(0);
            s.Level = l;
            s.GivePoints(DateTime.Now, DateTime.Now);
            int result = s.Total;
            int expResult = Score.MaxPointValue;
            Assert.Equal(expResult, result);
        }

        [Fact]
        public void GivePoints_FiveHundredSecondDelay_GiveMinPointValue()
        {
            Level l = new Level();
            l.Bpm = 100;
            Score s = new Score(0);
            s.Level = l;
            s.GivePoints(DateTime.Now, DateTime.Now.AddSeconds(500));
            int result = s.Total;
            int expResult = Score.MinPointValue;
            Assert.Equal(expResult, result);
        }

        [Fact]
        public void GivePoints_EqualAboveAndBelowZeroSecondDelay_GiveSamePointValue()
        {
            Level l = new Level();
            l.Bpm = 100;
            Score s1 = new Score(0);
            Score s2 = new Score(0);
            s1.Level = l;
            s2.Level = l;
            DateTime arrival = DateTime.Now; //declare here so both scores have same arrival time
            s1.GivePoints(arrival, DateTime.Now.AddSeconds(-5));
            s2.GivePoints(arrival, DateTime.Now.AddSeconds(5));
            bool result = s1.Total == s2.Total;
            Assert.True(result);
        }

        // TakePoints
        [Fact]
        public void TakePoints_MoreThanCurrentTotal_SetToZero()
        {
            Score s = new Score(50);
            s.TakePoints(60);
            int result = s.Total;
            int expResult = 0;
            Assert.Equal(expResult, result);
        }

        [Fact]
        public void TakePoints_StandardMethodCall_TakesDefault10()
        {
            Score s = new Score(50);
            s.TakePoints();
            int result = s.Total;
            int expResult = 40;
            Assert.Equal(expResult, result);
        }

        // GetAccuracy
        [Fact]
        public void GetAccuracy_90PercentOfMax_Returns90Percent()
        {
            Level l = new Level();
            l.NoteCount = 20;
            int t = l.NoteCount;
            int total = Convert.ToInt32(Score.MaxPointValue * l.NoteCount * 0.9);
            Score s = new Score(total, null, l);
            double result = s.GetAccuracy();
            double expResult = 0.9;
            Assert.Equal(expResult, result);
        }
    }
}
