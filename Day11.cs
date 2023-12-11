using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day11
    {
        private readonly List<List<char>> _map = new();
        private readonly List<(int, int)> _stars = new();
        private readonly List<int> _emptyColumns = new();
        private readonly List<int> _emptyRows = new();
        internal void Go()
        {
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day11input.txt");

                var line = sr.ReadLine();
                var count = 0;
                while (line != null)
                {
                    if (!line.Contains('#'))
                    {
                        _emptyRows.Add(count);
                    }

                    if (line.Length > 0)
                    {
                        ProcessLine(line);
                    }

                    line = sr.ReadLine();
                    count++;
                }
                sr.Close();

                for (var i = _map[0].Count - 1; i >= 0; i--)
                {
                    if (_map.All(m => m[i] == '.'))
                    {
                        _emptyColumns.Add(i);
                    }
                }

                for (var x = 0; x < _map[0].Count; x++)
                    for (var y = 0; y < _map.Count; y++)
                    {
                        if (_map[y][x] == '#')
                            _stars.Add((y, x));
                    }

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
            _map.Add(line.ToList());
        }

        private void PartOne()
        {
            long sum = 0;
            for (var i = 0; i < _stars.Count; i++)
                for (var j = i + 1; j < _stars.Count; j++)
                {
                    sum += Math.Abs(_stars[i].Item1 - _stars[j].Item1)
                           + Math.Abs(_stars[i].Item2 - _stars[j].Item2);
                    sum += _emptyColumns.Count(c => c > Math.Min(_stars[i].Item2, _stars[j].Item2)
                                                    && c < Math.Max(_stars[i].Item2, _stars[j].Item2)
                    );
                    sum += _emptyRows.Count(r => r > Math.Min(_stars[i].Item1, _stars[j].Item1)
                                                    && r < Math.Max(_stars[i].Item1, _stars[j].Item1)
                    );
                }

            Console.WriteLine("Day Eleven Part One: " + sum);
        }

        private void PartTwo()
        {
            long sum = 0;
            for (var i = 0; i < _stars.Count; i++)
            for (var j = i + 1; j < _stars.Count; j++)
            {
                sum += Math.Abs(_stars[i].Item1 - _stars[j].Item1)
                       + Math.Abs(_stars[i].Item2 - _stars[j].Item2);
                sum += 999999 * _emptyColumns.Count(c => c > Math.Min(_stars[i].Item2, _stars[j].Item2)
                                                        && c < Math.Max(_stars[i].Item2, _stars[j].Item2) 
                );
                sum += 999999 * _emptyRows.Count(r => r > Math.Min(_stars[i].Item1, _stars[j].Item1)
                                                      && r < Math.Max(_stars[i].Item1, _stars[j].Item1)
                );
            }

            Console.WriteLine("Day Eleven Part Two: " + sum);
        }
    }
}
