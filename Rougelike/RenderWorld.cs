using System;
namespace Rougelike
{
    public class RenderWorld
    {
        public RenderWorld(World world, Player player, IDisplay display)
        {
            _world = world;
            _player = player;
            _display = display;
            View();
        }

        private void View()
        {
            _display.View(_world, _player);
        }

        public void OnLeft()
        {
            MoveTo(0, -1);
            View();
        }

        public void OnRight()
        {
            MoveTo(0, 1);
            View();
        }

        public void OnUp()
        {
            MoveTo(-1, 0);
            View();
        }

        public void OnDown()
        {
            MoveTo(1, 0);
            View();
        }

        public bool IsWin()
        {
            return _player.IsWin;
        }

        private void MoveTo(int dx, int dy)
        {
            _player.SetX(dx);
            _player.SetY(dy);
        }

        private World _world;
        private Player _player;
        private IDisplay _display;
    }
}
