using System;
namespace Rougelike
{
    public class ConsoleView : IDisplay
    {
        public ConsoleView()
        {
        }

        public void View(World world, Player player)
        {
            for (int i = 0; i < world.Map.Length; ++i)
            {
                for (int j = 0; j < world.Map[i].Length; ++j)
                {
                    Console.SetCursorPosition(j, i);
                    Console.Write(world.Map[i][j]);
                }
            }
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(player.Y, player.X);
            Console.Write(player.Icon);
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
        }
    }
}
