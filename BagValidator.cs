using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class BagValidator
    {
        internal const int maxRed = 12;
        internal const int maxGreen = 13;
        internal const int maxBlue = 14;

        internal static void Go()
        {
            string? line;
            int sum = 0;
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day2input.txt");
                line = sr.ReadLine();
                while (line != null)
                {
                    var v = ValidateLine(line);
                    if (v != -1)
                    {
                        sum += v;
                    }

                    line = sr.ReadLine();
                }
                sr.Close();
                Console.WriteLine(sum);
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
                    if (pair[1] == "red") max = BagValidator.maxRed;
                    else if (pair[1] == "green") max = BagValidator.maxGreen;
                    else if (pair[1] == "blue") max = BagValidator.maxBlue;
                    else throw new Exception("Invalid Color");
                    if (int.Parse(pair[0]) > max) return -1;
                }
            }
            var game = int.Parse(s.Substring(0, colon).Split(" ")[1]);
            return game;
        }
    }
}
