using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using KeyboardLearner;
using System.Linq;

namespace KeyboardLearner.Tests
{
    public class PianoKeyUnitTests
    {
        [Fact]
        public void GetRangeValue_NotesOneApart_SortedCorrectly()
        {
            string pk1 = "c4";
            string pk2 = "c#4";
            int rangeVal1 = PianoKey.GetRangeValue(pk1);
            int rangeVal2 = PianoKey.GetRangeValue(pk2);
            Assert.True(rangeVal1 < rangeVal2);
        }

        [Fact]
        public void GetRangeValue_MultipleNotes_SortedCorrectly()
        {
            int c1 = PianoKey.GetRangeValue("c1");
            int b4 = PianoKey.GetRangeValue("b4");
            int cs4 = PianoKey.GetRangeValue("c#4");
            int d7 = PianoKey.GetRangeValue("d7");
            int c4 = PianoKey.GetRangeValue("c4");

            int[] notes = { b4, c1, cs4, d7, c4 };
            Array.Sort(notes);
            int[] expResult = { c1, c4, cs4, b4, d7 };
            bool result = Enumerable.SequenceEqual(expResult, notes);

            Assert.True(result);
        }

        [Fact]
        public void IsWhiteKey_KeyIsWhite_ReturnsTrue()
        {
            string pk = "c4";
            bool result = PianoKey.IsWhiteKey(pk);
            Assert.True(result);
        }

        [Fact]
        public void IsWhiteKey_KeyIsBlack_ReturnsFalse()
        {
            string pk = "c#4";
            bool result = PianoKey.IsWhiteKey(pk);
            Assert.False(result);
        }
    }
}
