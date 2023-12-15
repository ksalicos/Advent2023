using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Advent2023
{
    internal class Day14
    {
        private List<char[]> lines = new();
        private bool test = false;
        private string Hash() => string.Join(",", lines.Select(x => new string(x)));

        internal void Go()
        {
            try
            {
                var sr = test
                    ? new StreamReader("C:\\code\\advent2023\\day14test.txt")
                    : new StreamReader("C:\\code\\advent2023\\day14input.txt");

                var line = sr.ReadLine();
                while (line != null)
                {
                    lines.Add(line.ToCharArray());
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
            const int testAnswer = 136;
            // Save lines so we can restore for part two
            var temp = lines.Select(x => x.Select(y => y).ToArray()).ToList();

            Shift();
            var sum = GetCost();
            if (test && sum != testAnswer)
                throw new Exception("You Broke Something");
            Console.WriteLine("Day Fourteen Part One: " + sum);

            lines = temp;
            _shifts = 0;
        }

        private void PartTwo()
        {
            var targetCount = 1000000000;
            Dictionary<string, (int pathLen, string nextHash)> steps = new();
            var currentHash = Hash();

            for (var i = 1; i < targetCount; i++) // Could be a while if I'm an optimist.
            {
                Cycle();
                var cost = GetCost();
                if (test) Console.WriteLine($"Step {i} Weight: {cost}");

                var nextHash = Hash();

                if (steps.ContainsKey(nextHash))
                {
                    steps[currentHash] = (i, nextHash);

                    var cycleStart = steps.First(v => v.Value.nextHash == nextHash)
                        .Value.pathLen;
                    var remainingShifts = targetCount - cycleStart;
                    var cycleLen = i - cycleStart;
                    if (test) Console.WriteLine($"Found cycle from {cycleStart} to {i} with length {cycleLen}.");
                    var afterCycles = remainingShifts % cycleLen;
                    if (test) Console.WriteLine($"{afterCycles} remain.");

                    for (var j = 0; j < afterCycles; j++)
                    {
                        Cycle();
                        if (test)
                        {
                            if (j == afterCycles) Console.ForegroundColor = ConsoleColor.DarkRed;
                            Console.WriteLine($"After: {i} Weight: {GetCost()}");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }

                    break;
                }

                steps[currentHash] = (i, nextHash);
                currentHash = nextHash;
            }

            var sum = GetCost();
            Console.WriteLine("Day Fourteen Part Two: " + sum);
        }

        private int GetCost() => lines.Select((x, i) => x.Count(y => y == 'O') * (lines.Count - i)).Sum();

        // And here is my kludge after realizing I misread the instructions.
        private void Cycle()
        {
            Shift();
            Shift();
            Shift();
            Shift();
        }

        private static readonly char[] Directions = new char[] { 'N', 'W', 'S', 'E' };
        private static char Direction => Directions[_shifts % 4];
        private static int _shifts = 0;

        private void Shift()
        {
            var go = true;
            while (go)
            {
                go = false;
                for (var i = 0; i < lines.Count; i++)
                {
                    for (var j = 0; j < lines[i].Length; j++)
                    {
                        int a = i, b = j;
                        switch (Direction)
                        {
                            case 'N':
                                if (i == 0) continue;
                                a = i - 1;
                                break;
                            case 'W':
                                if (j == 0) continue;
                                b = j - 1;
                                break;
                            case 'S':
                                if (i == lines.Count - 1) continue;
                                a = i + 1;
                                break;
                            case 'E':
                                if (j == lines[i].Length - 1) continue;
                                b = j + 1;
                                break;
                        }

                        if (lines[i][j] == 'O' && lines[a][b] == '.')
                        {
                            go = true;
                            lines[i][j] = '.';
                            lines[a][b] = 'O';
                        }
                    }
                }
            }

            _shifts++;
        }
    }
}
