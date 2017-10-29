using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rougelike
{
    public class World
    {
        private bool CheckMap()
        {
            int len = Map[0].Length;
            return Map.All(s => s.StartsWith("#", StringComparison.CurrentCulture)
                     && s.EndsWith("#", StringComparison.CurrentCulture)
                     && s.Length == len
                     && s.All(ch => ch == '#' || ch == ' '));
        }

        private void InitFreeCell()
        {
            _freeCell = new List<Tuple<int, int>>();
            for (int i = 0; i < Map.Length; ++i)
            {
                for (int j = 0; j < Map[i].Length; ++j)
                {
                    if (Map[i][j] == ' ')
                    {
                        _freeCell.Add(new Tuple<int, int>(i, j));
                    }
                }
            }
        }

        public World(string path)
        {
            Map = File.ReadAllLines(@path);
            if (!CheckMap())
            {
                throw new ExceptionMap("ERROR::MAP");
            }
            InitFreeCell();
        }

        public World(string[] map)
        {
            Map = map;
            if (!CheckMap())
            {
                throw new ExceptionMap("ERROR::MAP");
            }
            InitFreeCell();
        }

        public Tuple<int, int> getFreeCell()
        {
            if (!_freeCell.Any())
            {
                return new Tuple<int, int>(-1, -1);
            }
            Random rnd = new Random();
            int index = _freeCell.Count() - 1;
            var result = _freeCell[rnd.Next(index)];
            _freeCell.RemoveAt(index);
            return result;
        }

        public string[] Map { get; }
        private List<Tuple<int, int>> _freeCell;
    }
}
