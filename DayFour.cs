using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class DayFour
    {
        private List<Card> _cards = new List<Card>();

        internal void Go()
        {
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day4input.txt");
                var line = sr.ReadLine();
                while (line != null)
                {
                    ProcessLine(line);
                    line = sr.ReadLine();
                }
                sr.Close();

                PartOne();
                // GetGearSum();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        void PartOne()
        {
            var winners = _cards.Select(c => c.Yours.Count(y => c.Winners.Contains(y)));
            var sum = winners.Select(w => w == 0
                ? 0
                : Math.Pow(2, w-1)
            ).Sum();

            Console.WriteLine("Day Four Part One: " + sum);
        }

        private void ProcessLine(string s)
        {
            s = s.Replace("  ", " ");
            var card = new Card();
            var colon = s.IndexOf(':');
            card.Number = GetCardNumber(s);
            var numbers = s.Substring(colon + 2).Split('|');
            card.Winners = numbers[0].Trim().Split(" ").Select(int.Parse).ToList();
            card.Yours = numbers[1].Trim().Split(" ").Select(int.Parse).ToList();
            _cards.Add(card);
        }

        private int GetCardNumber(string s)
        {
            int i = 0;
            var num = string.Empty;
            while (s[i] != ':')
            {
                if (char.IsDigit(s[i]))
                {
                    num += s[i];
                }

                i++;
            }

            return int.Parse(num);
        }

        internal class Card
        {
            internal int Number { get; set; }
            internal List<int> Winners { get; set; }
            internal List<int> Yours { get; set; }
        }
    }


}
