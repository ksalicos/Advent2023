using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Advent2023
{
    internal class Day07
    {
        private readonly List<Hand> _hands = new();
        internal static readonly Dictionary<char, short> Values = new()
        {
            {'2',2},
            {'3',3},
            {'4',4},
            {'5',5},
            {'6',6},
            {'7',7},
            {'8',8},
            {'9',9},
            {'T',10},
            {'J',11},
            {'Q',12},
            {'K',13},
            {'A',14},
        };

        internal static readonly Dictionary<char, short> Values2 = new()
        {
            {'J',1},
            {'2',2},
            {'3',3},
            {'4',4},
            {'5',5},
            {'6',6},
            {'7',7},
            {'8',8},
            {'9',9},
            {'T',10},
            {'Q',12},
            {'K',13},
            {'A',14},
        };

        internal void Go()
        {
            try
            {
                // var sr = new StreamReader("C:\\code\\advent2023\\day07test.txt");
                var sr = new StreamReader("C:\\code\\advent2023\\day07input.txt");
                var line = sr.ReadLine();
                while (line != null)
                {
                    ProcessLine(line);
                    line = sr.ReadLine();
                }
                sr.Close();

                PartOne();
                PartTwo();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private void ProcessLine(string line)
        {
            var h = new Hand(line);
            _hands.Add(h);
        }

        internal int Comparison(Hand left, Hand right)
        {
            if (left.GetHandType() > right.GetHandType()) return 1;
            if (left.GetHandType() < right.GetHandType()) return -1;
            for (var i = 0; i < left.Cards.Length; i++)
            {
                if (Values[left.Cards[i]] > Values[right.Cards[i]]) return 1;
                if (Values[left.Cards[i]] < Values[right.Cards[i]]) return -1;
            }

            return 0;
        }

        internal int Comparison2(Hand left, Hand right)
        {
            if (left.GetHandType2() > right.GetHandType2()) return 1;
            if (left.GetHandType2() < right.GetHandType2()) return -1;
            for (var i = 0; i < left.Cards.Length; i++)
            {
                if (Values2[left.Cards[i]] > Values2[right.Cards[i]]) return 1;
                if (Values2[left.Cards[i]] < Values2[right.Cards[i]]) return -1;
            }
            return 0;
        }

        private void PartOne()
        {
            long total = 0;

            _hands.Sort(Comparison);

            for (var i = 0; i < _hands.Count; i++)
            {
                var card = _hands[i];
                var rank = i + 1;
                var score = rank * card.Bid;
                total += score;
            }

            Console.WriteLine("Day Seven Part One: " + total);
        }

        private void PartTwo()
        {
            long total = 0;

            _hands.Sort(Comparison2);

            for (var i = 0; i < _hands.Count; i++)
            {
                var card = _hands[i];
                var rank = i + 1;
                var score = rank * card.Bid;
                total += score;
                // Console.WriteLine($"{rank}: {card} ({card.GetHandType2()}) score: {score} total: {total}");
            }

            Console.WriteLine("Day Seven Part Two: " + total);
        }
    }

    internal enum HandType
    {
        High,
        Pair,
        TwoPair,
        Three,
        FullHouse,
        Four,
        Five
    }

    internal class Hand
    {
        internal string Cards;
        internal long Bid { get; set; }

        public Hand(string s)
        {
            var l = s.Split(' ');
            Cards = l[0];
            Bid = int.Parse(l[1]);
        }

        public override string ToString()
        {
            var s = Cards.ToList<char>();
            s.Sort((r, l) => r == l ? 0 : Day07.Values2[r] > Day07.Values2[l] ? 1 : -1);
            return $"{Cards} ({string.Join("", s)}) {Bid}";
        }

        public Dictionary<char, int> ToDic()
        {
            Dictionary<char, int> cards = new Dictionary<char, int>();
            foreach (var c in Cards)
            {
                if (cards.ContainsKey(c))
                {
                    cards[c]++;
                }
                else
                {
                    cards[c] = 1;
                }
            }

            return cards;
        }

        public HandType GetHandType()
        {
            var c = ToDic();

            if (c.Values.Any(v => v == 5)) return HandType.Five;
            if (c.Values.Any(v => v == 4)) return HandType.Four;
            if (c.Values.Any(v => v == 3))
            {
                return c.Values.Any(v => v == 2) ? HandType.FullHouse : HandType.Three;
            }
            if (c.Values.All(v => v != 2)) return HandType.High;
            {
                return c.Values.Count(v => v == 2) == 2 ? HandType.TwoPair : HandType.Pair;
            }
        }

        public HandType GetHandType2()
        {
            var c = ToDic();

            var jokers = c.ContainsKey('J') ? c['J'] : 0;

            if (c.Values.Any(v => v == 5 || v + jokers == 5))
            {
                return HandType.Five;
            }

            if (c.Any(x => x.Value + jokers == 4 && x.Key != 'J'))
            {
                return HandType.Four;
            }

            if (c.Any(x => x.Value + jokers == 3 && x.Key != 'J'))
            {
                return (c.Values.Any(v => v == 3) && c.Values.Any(v => v == 2))
                    || c.Values.Count(v => v == 2) == 2
                    ? HandType.FullHouse
                    : HandType.Three;
            }

            if (c.Values.Count(v => v == 2) == 2)
            {
                return HandType.TwoPair;
            }

            return c.Any(x => x.Value + jokers == 2 && x.Key != 'J') ? HandType.Pair : HandType.High;
        }
    }
}
