using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeworkApp
{
    class RandomSymbolList
    {
        public int Remaining => symbols.Count;

        private List<char> symbols;
        private Random random;

        private RandomSymbolList()
        {
            symbols = new List<char>();

            symbols.Add('A');
            symbols.AddRange(Enumerable.Repeat('K', 2));
            symbols.AddRange(Enumerable.Repeat('Q', 4));
            symbols.AddRange(Enumerable.Repeat('J', 6));

            random = new Random();
            symbols = symbols.OrderBy(x => random.Next()).ToList();
        }

        public static RandomSymbolList GetNewList()
            => new RandomSymbolList();

        // Removes the symbol from the list
        public char PickOutRandomSymbol(bool removeCard)
        {
            int i = random.Next(0, symbols.Count);
            char symbol = symbols[i];

            if (removeCard)
                symbols.RemoveAt(i);

            return symbol;
        }

        public override string ToString()
        {
            return String.Join(',', symbols.ToArray());
        }
    }
}
