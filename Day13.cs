using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent2023.Shared;

namespace Advent2023
{
    internal class Day13
    {
        private readonly List<List<string>> _patterns = new();

        internal void Go()
        {
            Console.Clear();
            try
            {
                // var sr = new StreamReader("C:\\code\\advent2023\\day13test.txt");
                var sr = new StreamReader("C:\\code\\advent2023\\day13input.txt");

                var line = sr.ReadLine();
                var pattern = new List<string>();
                while (line != null)
                {
                    if (line.Length > 0)
                    {
                        pattern.Add(line);
                    }
                    else
                    {
                        _patterns.Add(pattern);
                        pattern = new List<string>();
                    }
                    line = sr.ReadLine();
                }
                if (pattern.Count > 0) _patterns.Add(pattern);
                sr.Close();

                PartOne();
                PartTwo();
            }
            catch (Exception e)
            {
                Log("Exception: " + e.Message);
            }
        }

        private void ProcessLine(string line)
        {
            throw new NotImplementedException();
        }

        private void PartOne()
        {
            var sum = 0;

            foreach (var pattern in _patterns)
            {
                var h = GetReflection(pattern);
                var v = GetReflection(Rotate(pattern));
                if (h != -1) sum += h * 100;
                if (v != -1) sum += v;
            }

            Console.WriteLine("Day 13 Part One: " + sum);
        }

        private static List<string> Rotate(List<string> pattern)
        {
            try
            {
                var results = new List<string>();

                for (var i = 0; i < pattern[0].Length; ++i)
                {
                    results.Add(new string(pattern.Select(x => x[i]).ToArray()));
                }

                return results;
            }
            catch (Exception ex)
            {
                Console.WriteLine("E in R");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                throw;
            }
        }

        private static void DrawPattern(List<string> s)
        {
            Console.WriteLine("---");
            s.ForEach(Console.WriteLine);
            Console.WriteLine("---");
        }

        private int GetReflection(List<string> pattern)
        {
            try
            {
                for (var i = 0; i < pattern.Count - 1; ++i)
                {
                    bool congruent = true;
                    if (pattern[i] == pattern[i + 1])
                    {
                        for (var j = 1; j <= i; j++)
                        {
                            if (i + j + 1 >= pattern.Count) break;
                            if (pattern[i - j] != pattern[i + j + 1])
                            {
                                congruent = false;
                                break;
                            }
                        }
                        if (congruent) return i + 1;
                    }
                }
                return -1;
            }
            catch
            {
                Console.WriteLine("E in GR");
                throw;
            }
        }

        private void PartTwo()
        {
            var sum = 0;

            foreach (var pattern in _patterns)
            {
                DrawPattern(pattern);
                var h = GetSmudgedReflection(pattern);
                Console.WriteLine("Smudged: " + h);
                var rotated = Rotate(pattern);
                DrawPattern(rotated);
                var v = GetSmudgedReflection(rotated);
                Console.WriteLine("Smudged: " + v);
                if (h != -1) sum += h * 100;
                if (v != -1) sum += v;
            }

            Console.WriteLine("Day 13 Part Two: " + sum);
        }

        private static int StringDiff(string a, string b)
        {
            return a.Where((c, idx) => c != b[idx]).Count();
        }

        private int GetSmudgedReflection(List<string> pattern)
        {
            try
            {
                for (var i = 0; i < pattern.Count - 1; ++i)
                {
                    var count = 0;
                    for (var j = 0; j <= i; j++)
                    {
                        if (i + j + 1 >= pattern.Count) break;
                        count += StringDiff(pattern[i - j], pattern[i + j + 1]);
                    }
                    if (count == 1) return i + 1;
                }
                return -1;
            }
            catch
            {
                Console.WriteLine("E in GR");
                throw;
            }
        }
    }
}
