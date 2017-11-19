using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Rougelike
{
    public class World
    {
        private const char WALL = '#';

        private bool CheckMap()
        {
            int len = Map[0].Length;
            if (Map.Length < 3)
            {
                return false;
            }
            return Map.All(s => s.Length == len && s.Length > 2
                     && s.All(ch => ch == WALL || ch == ' '));
        }

        private void InitFreeCell()
        {
            _freeCell = new List<(int, int)>();
            for (int i = 0; i < Map.Length; ++i)
            {
                for (int j = 0; j < Map[i].Length; ++j)
                {
                    if (Map[i][j] == ' ')
                    {
                        _freeCell.Add((i, j));
                    }
                }
            }
        }

        public World(string path)
        {
            if (!File.Exists(path))
            {
                throw new IOException("Can't read file: " + path);
            }
            Map = File.ReadAllLines(path);
            if (!CheckMap())
            {
                throw new MapException("ERROR::MAP");
            }
            InitFreeCell();
        }

        public World(string[] map)
        {
            Map = map;
            if (!CheckMap())
            {
                throw new MapException("ERROR::MAP");
            }
            InitFreeCell();
        }

        public (int, int) GetFreeCell()
        {
            if (!_freeCell.Any())
            {
                return (-1, -1);
            }
            var index = _freeCell.Count - 1;
            var result = _freeCell[_rnd.Next(index)];
            _freeCell.RemoveAt(index);
            return result;
        }

        public string[] Map { get; }
        private List<(int, int)> _freeCell;
        private static readonly Random _rnd = new Random();
    }
}
