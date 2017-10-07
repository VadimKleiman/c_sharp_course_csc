using System;
namespace Rougelike
{
    public class RenderWorld
    {
        public RenderWorld(World world, Player player)
        {
            _World = world;
            _Player = player;
            View();
        }

        private void View()
        {
            for (int i = 0; i < _World.Map.Length; ++i)
            {
                for (int j = 0; j < _World.Map[i].Length; ++j)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(_World.Map[i][j]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(_Player.Y, _Player.X);
            Console.Write(_Player.Icon);
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
            _Player.SetX(dx);
            _Player.SetY(dy);
        }

        private World _World;
        private Player _Player;
    }
}
