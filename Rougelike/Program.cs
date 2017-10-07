using System;

namespace Rougelike
{
    class EventLoop
    {
		public event Action LeftHandler;
		public event Action RightHandler;
        public event Action UpHandler;
        public event Action DownHandler;

        public void Run()
        {
            while(true)
            {
                var key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.LeftArrow:
                        LeftHandler?.Invoke();
                        break;
                    case ConsoleKey.RightArrow:
                        RightHandler?.Invoke();
                        break;
                    case ConsoleKey.UpArrow:
                        UpHandler?.Invoke();
                        break;
                    case ConsoleKey.DownArrow:
                        DownHandler?.Invoke();
                        break;
                    case ConsoleKey.Escape:
                        Environment.Exit(0);
                        break;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please enter path to file!");
                Environment.Exit(-1);
            }
            World world = new World(@args[0]);
            Player player = new Player(world);
            RenderWorld e = new RenderWorld(world, player);
            var eventLoop = new EventLoop();
            eventLoop.LeftHandler += e.OnLeft;
            eventLoop.RightHandler += e.OnRight;
            eventLoop.UpHandler += e.OnUp;
            eventLoop.DownHandler += e.OnDown;
            eventLoop.Run();
        }
    }
}
