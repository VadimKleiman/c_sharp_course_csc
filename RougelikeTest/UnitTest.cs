using System;
using Rougelike;
using Xunit;

namespace RougelikeTest
{
    public class UnitTest
    {
        private Player _player;
        private RenderWorld _rw;
        private class DisplayEmpty : IDisplay
        {
            public void View(World w, Player p)
            {
                
            }
        }

        public UnitTest()
        {
            string[] map = { "###",
                             "# #",
                             "###"};
            World world = new World(map);
            _player = new Player(world);
            IDisplay display = new DisplayEmpty();
            _rw = new RenderWorld(world, _player, display);
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
            Exception ex = Assert.Throws<MapException>(() => new World(map));
            Assert.Equal("ERROR::MAP", ex.Message);
        }

        [Fact]
        public void WrongPlayerTest()
        {
            string[] map = { "###",
                             "###",
                             "###"};
            World world = new World(map);
            Exception ex = Assert.Throws<PlayerException>(() => new Player(world));
            Assert.Equal("ERROR::PLAYER::POSITION", ex.Message);
        }
        [Fact]
        public void WinTest()
        {
            string[] map = { "###",
                             "#  ",
                             "###"};
            World world = new World(map);
            Player player = new Player(world);
            IDisplay display = new DisplayEmpty();
            RenderWorld rw = new RenderWorld(world, player, display);
            rw.OnRight();
            rw.OnRight();
            Assert.True(rw.IsWin());
        }
    }
}
