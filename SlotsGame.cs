using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HomeworkApp
{
    class SlotsGame
    {
        public int Points { get; set; }

        private const int NUM_LISTS = 3;
        private const int MULTIPLIER = 1;
        private RandomSymbolList[] randomSymbolLists;

        public SlotsGame()
        {
            Points = 20;

            // Initialize all the objects
            randomSymbolLists =
                Enumerable.Repeat(0, NUM_LISTS).Select(x => RandomSymbolList.GetNewList()).ToArray();
        }

        public (char[] slots, int winnings) Draw()
        {
            // Deduct points for draw
            AddPointsWithMultiplier(-1);

            char[] values = new char[NUM_LISTS];

            for (int i = 0; i < NUM_LISTS; i++)
            {
                values[i] = randomSymbolLists[i].PickOutRandomSymbol();
            }

            int winnings = CalculateWinnings(values);

            return (values, winnings);
        }

        private int CalculateWinnings(char[] values)
        {
            // If all values are the same, we add points
            if (values.Distinct().Count() == 1)
            {
                int baseWin = 0;

                switch (values[0])
                {
                    case 'A':
                        baseWin = 20;
                        break;
                    case 'K':
                        baseWin = 8;
                        break;
                    case 'Q':
                        baseWin = 4;
                        break;
                    case 'J':
                        baseWin = 2;
                        break;
                }

                return AddPointsWithMultiplier(baseWin);
            }

            return 0;
        }

        private static bool IsNullOrEmpty<T>(IEnumerable<T> collection)
        {
            return (collection == null || !collection.Any());
        }

        private int AddPointsWithMultiplier(int amount)
        {
            int totalWin = amount * MULTIPLIER;
            Points += totalWin;

            return totalWin;
        }

        public int SymbolsRemaining
            => randomSymbolLists[0].Remaining;
    }
}
