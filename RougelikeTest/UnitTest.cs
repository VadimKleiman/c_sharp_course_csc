using System;
using Rougelike;
using Xunit;

namespace RougelikeTest
{
    public class UnitTest
    {
        private World _world;
        private Player _player;
        private RenderWorld _rw;

        public UnitTest()
        {
            string[] map = { "###",
                             "# #",
                             "###"};
            _world = new World(map);
            _player = new Player(_world);
            _rw = new RenderWorld(_world, _player);
        }

        [Fact]
        public void WallTest()
        {
            int x = _player.X;
            int y = _player.Y;
            _rw.OnLeft();
            Assert.Equal(x, _player.X);
            Assert.Equal(y, _player.Y);
            _rw.OnRight();
            Assert.Equal(x, _player.X);
            Assert.Equal(y, _player.Y);
            _rw.OnUp();
            Assert.Equal(x, _player.X);
            Assert.Equal(y, _player.Y);
            _rw.OnDown();
            Assert.Equal(x, _player.X);
            Assert.Equal(y, _player.Y);
        }

        [Fact]
        public void WrongMapTest()
        {
            string[] map = { "###",
                             "# ",
                             "##"};
            Exception ex = Assert.Throws<ExceptionMap>(() => new World(map));
            Assert.Equal("ERROR::MAP", ex.Message);
        }

        [Fact]
        public void WrongPlayerTest()
        {
            string[] map = { "###",
                             "###",
                             "###"};
            World world = new World(map);
            Exception ex = Assert.Throws<ExceptionPlayer>(() => new Player(world));
            Assert.Equal("ERROR::PLAYER::POSITION", ex.Message);
        }
    }
}
