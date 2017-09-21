namespace hw01
{
	public class StringSetImpl : IStringSet
	{
		class Trie
		{
			public bool isTerminated;
			public int howManyStartsWithPrefix;
			public Trie[] nodes = new Trie[SYMBOL_COUNT];
		}

		static int START_INDEX = 26;
		static int SYMBOL_COUNT = 52;
		readonly Trie root;

		public StringSetImpl()
		{
			root = new Trie();
		}

		public bool Add(string element)
		{
			if (Contains(element))
			{
				return false;
			}
			Trie node = root;
			foreach (var ch in element)
			{
				int index = GetIndex(ch);
				if (node.nodes[index] == null)
				{
					node.nodes[index] = new Trie();
				}
				node = node.nodes[index];
				node.howManyStartsWithPrefix++;
			}

			node.isTerminated = true;
			root.howManyStartsWithPrefix++;
			return true;
		}

		public bool Contains(string element)
		{
			Trie node = Traverse(element);
			return node != null && node.isTerminated;
		}

		public int HowManyStartsWithPrefix(string prefix)
		{
			Trie node = Traverse(prefix);
			return node == null ? 0 : node.howManyStartsWithPrefix;
		}

		public bool Remove(string element)
		{
			if (!Contains(element))
			{
				return false;
			}
			Trie current = root;
			foreach (var ch in element)
			{
				current.howManyStartsWithPrefix--;
				Trie prev = current;
				current = current.nodes[GetIndex(ch)];
				if (current.howManyStartsWithPrefix == 1)
				{
					prev.nodes[GetIndex(ch)] = null;
				}
			}
			current.howManyStartsWithPrefix--;
			current.isTerminated = false;
			return true;
		}

		public int Size()
		{
			return root.howManyStartsWithPrefix;
		}

		static int GetIndex(char symbol)
		{
			if (symbol >= 'a' && symbol <= 'z')
			{
				return START_INDEX + symbol - 'a';
			}
			return symbol - 'A';
		}

		Trie Traverse(string element)
		{
			Trie node = root;
			foreach (var ch in element)
			{
				if (node == null)
				{
					return null;
				}
				node = node.nodes[GetIndex(ch)];
			}
			return node;
		}
	}
}