using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day04
    {
        private readonly List<Card> _cards = new List<Card>();

        internal void Go()
        {
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day04input.txt");
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

        private void PartOne()
        {
            var winners = _cards.Select(c => c.Yours.Count(y => c.Winners.Contains(y)));
            var sum = winners.Select(w => w == 0
                ? 0
                : Math.Pow(2, w-1)
            ).Sum();

            Console.WriteLine("Day Four Part One: " + sum);
        }

        private void PartTwo()
        {
            foreach (var card in _cards)
            {
                if (card.Count <= 0) continue;
                foreach (var target in _cards.Where(c=>c.Number > card.Number && c.Number <= card.Number + card.Count))
                {
                    target.Copies += card.Copies;
                }
            }

            var sum = _cards.Sum(c => c.Copies);
            Console.WriteLine("Day Four Part Two: " + sum);
        }

        private void ProcessLine(string s)
        {
            s = s.Replace("  ", " ");
            var card = new Card(s);
            
            _cards.Add(card);
        }

        internal class Card
        {
            internal Card(string s)
            {
                var colon = s.IndexOf(':');
                Number = (int)Shared.GetNumbers(s)[0];
                var numbers = s.Substring(colon + 2).Split('|');
                Winners = numbers[0].Trim().Split(" ").Select(int.Parse).ToList();
                Yours = numbers[1].Trim().Split(" ").Select(int.Parse).ToList();
            }

            internal int Number { get; set; }
            internal List<int> Winners { get; set; }
            internal List<int> Yours { get; set; }
            internal int Copies { get; set; } = 1;
            internal int Count => Yours.Count(c => this.Winners.Contains(c));

        }
    }
}
