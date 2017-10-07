using System;
namespace Rougelike
{
    public sealed class Player
    {
        public Player(World world)
        {
            _world = world;
            InitPosition();
        }

        private void InitPosition()
        {
            var position = _world.getFreeCell();
            X = position.Item1;
            Y = position.Item2;
            if (X == -1 && Y == -1)
            {
                throw new ExceptionPlayer("ERROR::PLAYER::POSITION");
            }
            Icon = '@';
        }

        public void SetX(int dx)
        {
            if (_world.Map[X + dx][Y] != '#')
            {
                X = X + dx;
            }
        }

        public void SetY(int dy)
        {
			if (_world.Map[X][Y + dy] != '#')
			{
				Y = Y + dy;
			}
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public char Icon { get; set; }

        private readonly World _world;
    }
}
