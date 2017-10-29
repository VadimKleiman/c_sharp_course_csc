using System;
namespace Rougelike
{
    public class RenderWorld
    {
        public RenderWorld(World world, Player player)
        {
            _world = world;
            _player = player;
            View();
        }

        private void View()
        {
            for (int i = 0; i < _world.Map.Length; ++i)
            {
                for (int j = 0; j < _world.Map[i].Length; ++j)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(_world.Map[i][j]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(_player.Y, _player.X);
            Console.Write(_player.Icon);
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
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

        private void MoveTo(int dx, int dy)
        {
            _player.SetX(dx);
            _player.SetY(dy);
        }

        private World _world;
        private Player _player;
    }
}
