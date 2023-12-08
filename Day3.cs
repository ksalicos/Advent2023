using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day3
    {
        private List<(int, int, char)> _symbols = new List<(int, int, char)>();
        private List<(int, int, string)> _numbers = new List<(int, int, string)>();

        internal void Go()
        {
            var lineNum = 0;
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day3input.txt");
                var line = sr.ReadLine();
                while (line != null)
                {
                    ProcessLine(line, lineNum);
                    lineNum++;
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
            var sum = 0;
            foreach (var n in _numbers)
            {
                if (_symbols.Any(s=> IsAdjacent(n,s)))
                {
                    sum += int.Parse(n.Item3);
                }
            }
            Console.WriteLine($"Day Three Part One: {sum}");
        }

        private void PartTwo()
        {
            var sum = _symbols.Where(s => s.Item3 == '*')
                .Select(GetAdjacentParts)
                .Where(p=>p.Count == 2)
                .Select(p => int.Parse(p.First())
                             * int.Parse(p.Last()))
                .Sum();

            Console.WriteLine($"Day Three Part Two: {sum}");
        }

        private List<string> GetAdjacentParts((int, int, char) s)
        {
            var parts = _numbers.Where(n =>
                IsAdjacent(n, s))
                .Select(n=>n.Item3)
                .ToList();
            return parts;
        }

        private bool IsAdjacent((int, int, string) n, (int, int, char) s)
        {
            return
                (s.Item1 >= n.Item1 - 1 && s.Item1 <= n.Item1 + 1)
                && (s.Item2 >= n.Item2 - 1 && s.Item2 <= n.Item2 + n.Item3.Length);
        }

        private void ProcessLine(string s, int num)
        {
            var i = 0;
            while (i < s.Length)
            {
                if (char.IsDigit(s[i]))
                {
                    var j = 0;
                    while (i + j < s.Length && char.IsDigit(s[i + j]))
                    {
                        j++;
                    }
                    var value = s.Substring(i, j);
                    _numbers.Add((num, i, value));
                    i += j;
                }
                else
                {
                    if (s[i] != '.')
                    {
                        _symbols.Add((num, i, s[i]));
                    }
                    i++;
                }
            }
        }
    }
}
