using hw01;
using Xunit;

namespace StringSetTest
{
    public class UnitTest
    {
        private readonly StringSet s;
        public UnitTest()
        {
            s = new StringSet();
        }

        [Fact]
        public void AddOneStringTest()
        {
            Assert.True(s.Add("string"));
            Assert.True(s.Contains("string"));
            Assert.Equal(1, s.Size());
        }

        [Fact]
        public void AddTwoStringTest()
        {
            Assert.True(s.Add("first"));
            Assert.True(s.Add("second"));
            Assert.True(s.Contains("first"));
            Assert.True(s.Contains("second"));
            Assert.Equal(2, s.Size());
        }

        [Fact]
        public void AddOneStringTwiceTest()
        {
            Assert.True(s.Add("string"));
            Assert.False(s.Add("string"));
            Assert.True(s.Contains("string"));
            Assert.Equal(1, s.Size());
        }

        [Fact]
        public void ContainsTest()
        {
            Assert.False(s.Contains("string"));
            Assert.True(s.Add("string"));
            Assert.True(s.Contains("string"));
            Assert.False(s.Contains("other"));
        }

        [Fact]
        public void RemoveTest()
        {
            Assert.False(s.Remove("string"));
            Assert.True(s.Add("string"));
            Assert.True(s.Remove("string"));
            Assert.False(s.Contains("string"));
        }

        [Fact]
        public void SizeTest()
        {
            Assert.Equal(0, s.Size());
            Assert.True(s.Add("First"));
            Assert.Equal(1, s.Size());
            Assert.True(s.Add("Second"));
            Assert.Equal(2, s.Size());
            Assert.True(s.Remove("First"));
            Assert.Equal(1, s.Size());
            Assert.True(s.Remove("Second"));
            Assert.Equal(0, s.Size());
        }

        [Fact]
        public void HowManyStartsWithPrefixTest()
        {
            Assert.True(s.Add("a"));
            Assert.True(s.Add("aa"));
            Assert.True(s.Add("ab"));
            Assert.True(s.Add("aaaa"));
            Assert.Equal(s.Size(), s.HowManyStartsWithPrefix(""));
            Assert.Equal(s.Size(), s.HowManyStartsWithPrefix("a"));
            Assert.Equal(2, s.HowManyStartsWithPrefix("aa"));
            Assert.Equal(1, s.HowManyStartsWithPrefix("ab"));
            Assert.Equal(1, s.HowManyStartsWithPrefix("aaaa"));
            Assert.Equal(0, s.HowManyStartsWithPrefix("aba"));
        }

        [Fact]
        public void EmptyTest()
        {
            Assert.False(s.Contains(""));
            Assert.True(s.Add(""));
            Assert.True(s.Contains(""));
            Assert.Equal(1, s.Size());
            Assert.True(s.Remove(""));
            Assert.False(s.Contains(""));
            Assert.Equal(0, s.Size());
        }

        [Fact]
        public void CyrillicTest()
        {
            Assert.True(s.Add("Строка"));
            Assert.False(s.Contains("строка"));
            Assert.Equal(1, s.Size());
        }
    }
}
