﻿namespace TexasHoldem.AI.ThreeOfAKind.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Logic.Cards;

    internal static class Analyzers
    {
        public static Risk HasFlushRisk(IEnumerable<Card> cards)
        {
            var suitedCards = cards.Select(c => c.Suit)
                                   .GroupBy(c => c)
                                   .OrderByDescending(g => g)
                                   .FirstOrDefault()
                                   .Count();

            switch (suitedCards)
            {
                // case 4 - flush is on - handle elsewhere
                case 4:
                    return Risk.High;
                case 3:
                    return Risk.Moderate;
                case 2:
                    return Risk.Low;
                default:
                    return Risk.No;
            }
        }

        public static Risk HasStraightRisk(ICollection<Card> cards)
        {
            var sortedCards = cards.Select(c => (int)c.Type)
                 .OrderByDescending(t => t)
                 .ToList();

            if (sortedCards.Contains(14))
            {
                sortedCards.Add(1);
            }

            var holesCount = 0;
            var notConnectedCards = 0;

            for (var i = 1; i < sortedCards.Count; i++)
            {
                if (sortedCards[i] - sortedCards[i - 1] <= 3)
                {
                    holesCount += sortedCards[i - 1] - sortedCards[i] + 1;
                }
                else
                {
                    notConnectedCards++;
                }

                if (holesCount > 2)
                {
                    holesCount -= 2;
                    notConnectedCards++;
                }
            }

            var result = cards.Count - holesCount - notConnectedCards - 1;
            if (result > 3)
            {
                result = 3;
            }

            if (result < 0)
            {
                result = 0;
            }

            // 4 - straight in community cards - handle it elsewhere
            // 3 - very big chance; 2 - likely to have, 1 - possible if very lucky, 0 or less - no possible - i think :)
            return (Risk)result;
        }
    }
}
