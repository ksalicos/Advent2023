using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class DayTwo
    {
        internal const int maxRed = 12;
        internal const int maxGreen = 13;
        internal const int maxBlue = 14;

        internal void Go()
        {
            string? line;
            int sum = 0;
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day2input.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    sum += GetLinePower(line);
                    line = sr.ReadLine();
                }
                sr.Close();
                Console.WriteLine("Day Two: " + sum);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

        }

        private static int ValidateLine(string s)
        {
            var colon = s.IndexOf(':');
            var pulls = s.Substring(colon + 2).Split("; ");
            foreach (var p in pulls)
            {
                var values = p.Split(", ");
                foreach (var v in values)
                {
                    int max;
                    var pair = v.Split(" ");
                    if (pair[1] == "red") max = DayTwo.maxRed;
                    else if (pair[1] == "green") max = DayTwo.maxGreen;
                    else if (pair[1] == "blue") max = DayTwo.maxBlue;
                    else throw new Exception("Invalid Color");
                    if (int.Parse(pair[0]) > max) return -1;
                }
            }
            var game = int.Parse(s.Substring(0, colon).Split(" ")[1]);
            return game;
        }

        private static int GetLinePower(string s)
        {
            var g = GetGame(s);
            var power = g.Pulls.Max(x=>x.Red)
                * g.Pulls.Max(x=>x.Green)
                * g.Pulls.Max(x=>x.Blue);

            return power;
        }

        private static Game GetGame(string s)
        {
            var game = new Game();
            var colon = s.IndexOf(':');
            var pulls = s.Substring(colon + 2).Split("; ");
            foreach (var p in pulls)
            {
                var pull = new Pull();
                var values = p.Split(", ");
                foreach (var v in values)
                {
                    var pair = v.Split(" ");
                    var value = int.Parse(pair[0]);
                    if (pair[1] == "red") pull.Red = value;
                    else if (pair[1] == "green") pull.Green = value;
                    else if (pair[1] == "blue") pull.Blue = value;
                    else throw new Exception("Invalid Color");
                }
                game.Pulls.Add(pull);
            }
            game.Number = int.Parse(s.Substring(0, colon).Split(" ")[1]);
            return game;
        }

    }

    internal class Game
    {
        internal int Number { get; set; }
        internal List<Pull> Pulls { get; set; } = new();
    }

    internal class Pull
    {
        internal int Red { get; set; }
        internal int Green { get; set; }
        internal int Blue { get; set; }
    }

}
