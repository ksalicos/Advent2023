using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class SchematicReader
    {
        private List<(int, int)> _symbols = new List<(int, int)>();
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

                var sum = 0;
                foreach (var n in _numbers)
                {
                    if (IsAdjacent(n))
                    {
                        sum += int.Parse(n.Item3);
                    }
                    else
                    {
                        Console.WriteLine($"Line {n.Item1}, {n.Item3}");
                    }
                }

                Console.WriteLine($"Day Three Part One: {sum}");
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private bool IsAdjacent((int, int, string) n)
        {
            return _symbols.Any(s =>
                (s.Item1 >= n.Item1 - 1 && s.Item1 <= n.Item1 + 1)
                && (s.Item2 >= n.Item2 - 1 && s.Item2 <= n.Item2 + n.Item3.Length)
            );
        }

        private void ProcessLine(string s, int num)
        {
            var i = 0;
            while (i < s.Length)
            {
                if (char.IsDigit(s[i]))
                {
                    var j = 0;
                    while (i + j < s.Length && char.IsDigit(s[i+j]))
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
                        _symbols.Add((num, i));
                    }
                    i++;
                }
            }
        }
    }
}
