using Xunit;

namespace OptionTest
{
    public class UnitTest1
    {

        [Fact]
        public void SimpleTest()
        {
            const int check = 2;
            var o = Option.Option<int>.Some(check);
            Assert.Equal(check, o.Value);
        }

        [Fact]
        public void MapTest()
        {
            const int check = 4;
            Assert.Equal(Option.Option<int>
                .Some(check).Map(x => x * x), Option.Option<int>.Some(16));
        }

        [Fact]
        public void FlattenTest()
        {
            const int check = 1;
            var a = Option.Option<int>.Some(check);
            var b = Option.Option<Option.Option<int>>.Some(a);
            var c = Option.Option<int>.Flatten(b);
            Assert.Equal(c, a);
        }

        [Fact]
        public void EmptyTest()
        {
            Assert.Equal(Option.Option<int>.None().Map(x => x),
                         Option.Option<int>.None());
        }
    }
}
