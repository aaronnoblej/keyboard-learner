using System;
using System.Linq;
using KeyboardLearner;
using Xunit;

namespace KeyboardLearner.Tests
{
    public class ProfileUnitTests
    {
        [Theory]
        [InlineData("123ABC")]
        [InlineData("abcdef")]
        public void ValidateColor_ValidHexColors_ReturnsTrue(string color)
        {
            Assert.True(color.Length == 6 && color.ToCharArray().All(c => "0123456789abcdefABCDEF".Contains(c)));
        }

        [Theory]
        [InlineData("ABCGRT")]
        [InlineData("1234567890")]
        [InlineData("abcr1234")]
        public void ValidateColor_InvalidHexColors_ReturnsFalse(string color)
        {
            Assert.False(color.Length == 6 && color.ToCharArray().All(c => "0123456789abcdefABCDEF".Contains(c)));
        }
    }
}
