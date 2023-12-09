using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day9
    {
        private readonly List<List<int>> _histories = new();

        internal void Go()
        {
            try
            {
                // var sr = new StreamReader("C:\\code\\advent2023\\day9test.txt");
                var sr = new StreamReader("C:\\code\\advent2023\\day9input.txt");

                var line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Length > 0)
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
            var n = line.Split(" ").Select(int.Parse).ToList();
            _histories.Add(n);
        }

        private void PartOne()
        {
            var sum = 0;
            foreach (var n in _histories)
            {
                var current = n;
                List<List<int>> foo = new() { n };
                while (current.Any(x => x != 0))
                {
                    var differences = new List<int>();
                    for (int i = 1; i < current.Count; i++)
                    {
                        differences.Add(current[i] - current[i - 1]);
                    }

                    if (differences.Count == 0)
                    {
                        throw new Exception("huh");
                    }

                    foo.Add(differences);
                    current = differences;
                }

                var sum1 = foo.Sum(x => x.Last());
                sum += sum1;
            }

            Console.WriteLine("Day Nine Part One: " + sum);
        }

        private void PartTwo()
        {
            var sum = 0;
            foreach (var n in _histories)
            {
                var current = n;
                List<List<int>> foo = new() { n };
                while (current.Any(x => x != 0))
                {
                    var differences = new List<int>();
                    for (int i = 1; i < current.Count; i++)
                    {
                        differences.Add(current[i] - current[i - 1]);
                    }

                    if (differences.Count == 0)
                    {
                        throw new Exception("huh");
                    }

                    foo.Add(differences);
                    current = differences;
                }

                var result = 0;
                for (var i = foo.Count - 1; i >= 0; i-- )
                {
                    result = foo[i].First() - result;
                }

                sum += result;
            }

            Console.WriteLine("Day Nine Part Two: " + sum);
        }
    }
}
