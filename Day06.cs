using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day06
    {
        private List<long> _times;
        private List<long> _distances;

        internal void Go()
        {
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day06input.txt");
                var line = sr.ReadLine();
                _times = Shared.GetNumbers(line);
                line = sr.ReadLine();
                _distances = Shared.GetNumbers(line);
                sr.Close();

                PartOne();
                PartTwo();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private long GetWinCount(long time, long distance)
        {
            var count = 0;
            for (var j = 0; j < time; j++)
            {
                if (j * (time - j) > distance)
                {
                    count++;
                }
            }

            return count;
        }

        private void PartOne()
        {
            long total = 1;
            for (var i = 0; i < _times.Count; i++)
            {
                var time = _times[i];
                var distance = _distances[i];
                var count = GetWinCount(time, distance);
                total *= count;
            }

            Console.WriteLine("Day Six Part One: " + total);
        }

        private void PartTwo()
        {
            var time = long.Parse(string.Join("",_times.Select(s => s.ToString())));
            var distance = long.Parse(string.Join("",_distances.Select(s => s.ToString())));
            var total = GetWinCount(time, distance);
            Console.WriteLine("Day Six Part Two: " + total);
        }
    }
}
