using Xunit;
using OptionNS;

namespace OptionTest
{
    public class OptionTest
    {

        [Fact]
        public void SimpleTest()
        {
            const int check = 2;
            var o = Option<int>.Some(check);
            Assert.Equal(check, o.Value);
        }

        [Fact]
        public void MapTest()
        {
            const int check = 4;
            Assert.Equal(Option<int>
                .Some(check).Map(x => x * x), Option<int>.Some(16));
        }

        [Fact]
        public void FlattenTest()
        {
            const int check = 1;
            var a = Option<int>.Some(check);
            var b = Option<Option<int>>.Some(a);
            var c = Option<int>.Flatten(b);
            Assert.Equal(c, a);
            Assert.Equal(Option<int>.Flatten(null), Option<int>.None());
        }

        [Fact]
        public void EmptyTest()
        {
            Assert.Equal(Option<int>.None().Map(x => x), Option<int>.None());
        }

        [Fact]
        public void IsNoneTest()
        {
            Assert.True(Option<int>.None().Map(x => x).IsNone);
            Assert.False(Option<int>.Some(42).IsNone);
        }
    }
}
