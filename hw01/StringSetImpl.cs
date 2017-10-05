using System.Collections.Generic;

namespace hw01
{
    public class StringSet : IStringSet
    {
        public StringSet()
        {
            _root = new Trie();
        }

        public bool Add(string element)
        {
            if (Contains(element))
            {
                return false;
            }
            Trie node = _root;
            foreach (var ch in element)
            {
                if (!node.Nodes.ContainsKey(ch))
                {
                    node.Nodes.Add(ch, new Trie());
                }
                node.Nodes.TryGetValue(ch, out node);
                ++node.HowManyStartsWithPrefix;
            }

            node.IsTerminal = true;
            ++_root.HowManyStartsWithPrefix;
            return true;
        }

        public bool Contains(string element)
            => (Traverse(element)?.IsTerminal).GetValueOrDefault(false);

        public int HowManyStartsWithPrefix(string prefix)
            => (Traverse(prefix)?.HowManyStartsWithPrefix).GetValueOrDefault(0);

        public bool Remove(string element)
        {
            if (!Contains(element))
            {
                return false;
            }
            Trie current = _root;
            foreach (var ch in element)
            {
                --current.HowManyStartsWithPrefix;
                Trie prev = current;
                current.Nodes.TryGetValue(ch, out current);
                if (current.HowManyStartsWithPrefix == 1)
                {
                    prev.Nodes.Remove(ch);
                }
            }
            --current.HowManyStartsWithPrefix;
            current.IsTerminal = false;
            return true;
        }

        public int Size() => _root.HowManyStartsWithPrefix;

        private Trie Traverse(string element)
        {
            Trie node = _root;
            foreach (var ch in element)
            {
                if (node == null)
                {
                    return null;
                }
                node.Nodes.TryGetValue(ch, out node);
            }
            return node;
        }

        private class Trie
        {
            public bool IsTerminal { get; set; }
            public int HowManyStartsWithPrefix { get; set; }
            public Dictionary<char, Trie> Nodes { get; }
            public Trie()
            {
                Nodes = new Dictionary<char, Trie>();
            }
        }

        private readonly Trie _root;
    }
}