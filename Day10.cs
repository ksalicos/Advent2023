using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day10
    {
        private const int height = 140;
        private const int width = 140;
        private char[][] map = new char[height][];
        private int StartX, StartY;

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
                        map[i] = line.ToCharArray();
                        if (line.Contains('S'))
                        {
                            StartX = line.IndexOf('S');
                            StartY = i;
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
            Func<int, int, (int, int, char)> North = (y, x) => (y - 1, x, 'n');
            Func<int, int, (int, int, char)> South = (y, x) => (y + 1, x, 's');
            Func<int, int, (int, int, char)> East = (y, x) => (y, x + 1, 'e');
            Func<int, int, (int, int, char)> West = (y, x) => (y, x - 1, 'w');

            switch (map[y][x])
            {
                case '|': return d == 'n' ? North(y, x) : South(y, x);
                case '-': return d == 'e' ? East(y, x) : West(y, x);
                case 'L': return d == 'w' ? North(y, x) : East(y, x);
                case 'J': return d == 'e' ? North(y, x) : West(y, x);
                case '7': return d == 'e' ? South(y, x) : West(y, x);
                case 'F': return d == 'w' ? South(y, x) : East(y, x);

                default: throw new Exception("Unknown Direction");
            }
        }

        private void PartOne()
        {
            // looking at input, path must start north and end east
            var current = (StartY - 1, StartX, 'n');
            var steps = 1;

            while (map[current.Item1][current.Item2] != 'S')
            {
                current = GetNext(current.Item1, current.Item2, current.Item3);
                steps++;
            }

            Console.WriteLine("Day 10 Part One: " + steps / 2);
        }

        private void PartTwo()
        {
            var current = (StartY - 1, StartX, 'n');
            var pipe = new List<(int, int, char)> { (StartY, StartX, 'n'), (current.Item1, current.Item2, 'n') };

            while (map[current.Item1][current.Item2] != 'S')
            {
                current = GetNext(current.Item1, current.Item2, current.Item3);
                pipe.Add(current);
            }

            var left = new List<char> { 'L', 'F', 'S' };
            var right = new List<char> { 'J', '7' };
            var all = new List<char> { 'J', 'L', 'F', '7', '|' };
            var count = 0;
            for (var y = 0; y < width; y++)
            {
                var inLoop = false;
                for (var x = 0; x < height; x++)
                {
                    var m = map[y][x];

                    if (pipe.Any(p => p.Item1 == y && p.Item2 == x))
                    {
                        Console.ForegroundColor = ConsoleColor.Cyan;

                        var d = pipe.First(p => p.Item1 == y && p.Item2 == x).Item3;

                        if (m == '|')
                        {
                            inLoop = d == 'n';
                        }
                        //else if (left.Contains(m))
                        //{
                        //    inLoop = d == 'e';
                        //}
                        if (m == 'J') inLoop = d == 'e';
                        if (m == '7') inLoop = d == 'n';

                        if (m == 'S') Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(m);
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
                        Console.Write('*');

                    }
                }

                Console.Write('\n');
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Day Ten Part Two: " + count);
        }
    }

    internal class Pipe
    {
        private List<Pipe> Adjacent = new();
    }
}
