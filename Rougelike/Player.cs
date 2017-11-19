namespace Rougelike
{
    public sealed class Player
    {
        public Player(World world)
        {
            _world = world;
            IsWin = false;
            InitPosition();
        }

        private void InitPosition()
        {
            var position = _world.GetFreeCell();
            X = position.Item1;
            Y = position.Item2;
            if (X == -1 && Y == -1)
            {
                throw new PlayerException("ERROR::PLAYER::POSITION");
            }
            Icon = '@';
        }

        public bool SetX(int dx)
        {
            if (X == _world.Map.Length - 1 || X == 0)
            {
                IsWin = true;
                return false;
            }
            if (_world.Map[X + dx][Y] != '#')
            {
                X = X + dx;
                return true;
            }
            return false;
        }

        public bool SetY(int dy)
        {
            if (Y == _world.Map[X].Length - 1 || Y == 0)
            {
                IsWin = true;
                return false;
            }
            if (_world.Map[X][Y + dy] != '#')
            {
                Y = Y + dy;
                return true;
            }
            return false;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public char Icon { get; private set; }

        private readonly World _world;
        public bool IsWin { get; set; }
    }
}
