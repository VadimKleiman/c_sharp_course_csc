using System;
using System.Threading.Tasks;

namespace Rougelike
{
    class Program
    {

        static void Main(string[] args)
        {
            string path = null;
            if (args.Length < 1)
            {
                path = "Map.txt";
            }
            else
            {
                path = args[0];
            }
            Console.CancelKeyPress += Console_CancelKeyPress;
            World world = new World(path);
            Player player = new Player(world);
            IDisplay display = new ConsoleView();
            RenderWorld e = new RenderWorld(world, player, display);
            var taskKeys = new Task(() => ReadKeys(e));
            taskKeys.Start();
            var tasks = new[] { taskKeys };
            Task.WaitAll(tasks);
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Environment.Exit(0);
        }
        private static void ReadKeys(RenderWorld rw)
        {
            ConsoleKeyInfo key = new ConsoleKeyInfo();
            while (!Console.KeyAvailable && key.Key != ConsoleKey.Escape)
            {
                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        rw.OnUp();
                        break;
                    case ConsoleKey.DownArrow:
                        rw.OnDown();
                        break;

                    case ConsoleKey.RightArrow:
                        rw.OnRight();
                        break;

                    case ConsoleKey.LeftArrow:
                        rw.OnLeft();
                        break;

                    case ConsoleKey.Escape:
                        break;
                }
                if (rw.IsWin())
                {
                    Console.Clear();
                    Console.WriteLine("You Win!");
                    break;
                }
            }
        }
    }
}
