using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day10
    {
        private const int Height = 140;
        private const int Width = 140;
        private readonly char[][] _map = new char[Height][];
        private int _startX, _startY;

        internal void Go()
        {
            try
            {
                // var sr = new StreamReader("C:\\code\\advent2023\\day10test.txt");
                var sr = new StreamReader("C:\\code\\advent2023\\day10input.txt");

                var line = sr.ReadLine();
                var i = 0;
                while (line != null)
                {
                    if (line.Length > 0)
                    {
                        _map[i] = line.ToCharArray();
                        if (line.Contains('S'))
                        {
                            _startX = line.IndexOf('S');
                            _startY = i;
                        }
                    }
                    line = sr.ReadLine();
                    i++;
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

        private (int, int, char) GetNext(int y, int x, char d)
        {
            (int, int, char) North(int y, int x) => (y - 1, x, 'n');
            (int, int, char) South(int y, int x) => (y + 1, x, 's');
            (int, int, char) East(int y, int x) => (y, x + 1, 'e');
            (int, int, char) West(int y, int x) => (y, x - 1, 'w');

            return _map[y][x] switch
            {
                '|' => d == 'n' ? North(y, x) : South(y, x),
                '-' => d == 'e' ? East(y, x) : West(y, x),
                'L' => d == 'w' ? North(y, x) : East(y, x),
                'J' => d == 'e' ? North(y, x) : West(y, x),
                '7' => d == 'e' ? South(y, x) : West(y, x),
                'F' => d == 'w' ? South(y, x) : East(y, x),
                _ => throw new Exception("Unknown Direction")
            };
        }

        private void PartOne()
        {
            // looking at input, path must start north and end east
            var current = (_startY - 1, StartX: _startX, 'n');
            var steps = 1;

            while (_map[current.Item1][current.Item2] != 'S')
            {
                current = GetNext(current.Item1, current.Item2, current.Item3);
                steps++;
            }

            Console.WriteLine("Day Ten Part One: " + steps / 2);
        }

        private void PartTwo()
        {
            var current = (_startY - 1, StartX: _startX, 'n');
            var pipe = new List<(int, int, char)> { (_startY, _startX, 'n'), (current.Item1, current.Item2, 'n') };

            while (_map[current.Item1][current.Item2] != 'S')
            {
                current = GetNext(current.Item1, current.Item2, current.Item3);
                pipe.Add(current);
            }

            var count = 0;
            for (var y = 0; y < Width; y++)
            {
                var inLoop = false;
                for (var x = 0; x < Height; x++)
                {
                    var m = _map[y][x];

                    if (pipe.Any(p => p.Item1 == y && p.Item2 == x))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        var d = pipe.First(p => p.Item1 == y && p.Item2 == x).Item3;

                        switch (m)
                        {
                            case '|':
                                inLoop = d == 'n';
                                break;
                            case 'J':
                                inLoop = d == 'e';
                                break;
                            case '7':
                                inLoop = d == 'n';
                                break;
                            case 'S':
                                Console.ForegroundColor = ConsoleColor.Yellow;
                                break;
                        }

                        // Console.Write(m);
                    }
                    else
                    {
                        if (inLoop)
                        {
                            count++;
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Black;
                        }
                        // Console.Write('*');

                    }
                }

                // Console.Write('\n');
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Day Ten Part Two: " + count);
        }
    }
}
