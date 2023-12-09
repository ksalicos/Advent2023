using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day8
    {
        private readonly Dictionary<string, (string, string)> _nodes = new();
        private string _directions;

        internal void Go()
        {
            try
            {
                // var sr = new StreamReader("C:\\code\\advent2023\\day8test.txt");
                var sr = new StreamReader("C:\\code\\advent2023\\day8input.txt");
                _directions = sr.ReadLine();

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
            _nodes[line.Substring(0, 3)] =
                (line.Substring(7, 3), line.Substring(12, 3));
        }

        private void PartOne()
        {
            var location = "AAA";
            var steps = 0;

            while (location != "ZZZ")
            {
                location = _directions[steps % _directions.Length] == 'L'
                    ? _nodes[location].Item1
                    : _nodes[location].Item2;

                steps++;
            }

            Console.WriteLine($"Day Eight Part One: " + steps);
        }


        void foo()
        {

        }

        private void PartTwo()
        {
            var toNextZ = new Dictionary<string, (string, int)>();
            var toProcess = _nodes.Keys.Where(x => x.EndsWith("A")).Select(x => (x, 0)).ToList();

            while (toProcess.Any())
            {
                var toAdd = new List<(string, int)>();

                foreach (var s in toProcess)
                {
                    var location = s.Item1;
                    var idx = s.Item2;

                    // This runs forever on bad data.
                    do
                    {
                        location = _directions[idx % _directions.Length] == 'L'
                            ? _nodes[location].Item1
                            : _nodes[location].Item2;
                        idx++;
                    } while (!location.EndsWith("Z"));

                    var steps = idx - s.Item2;

                    if (!toNextZ.ContainsKey(location))
                    {
                        toAdd.Add((location, steps % _directions.Length));
                    }

                    toNextZ[s.Item1] = (location, steps);
                }

                toProcess = toAdd;
            }

            // By observation, in this data there are no cycles containing multiple Zs.
            var list = toNextZ.Where(k => k.Key.EndsWith("A"))
                .Select(x => (x.Value.Item2, toNextZ[x.Key].Item2)).ToList();

            // BRUTE FORCE - runs fast enough for advent.
            long k = 1;
            while (list.Any(v => (list[0].Item1 + k * list[0].Item2 - v.Item1) % v.Item2 != 0))
            {
                k++;
            }

            var stepsTaken = list[0].Item1 + k * list[0].Item2;

            Console.WriteLine($"Day Eight Part Two: " + stepsTaken);
        }

        private void PartTwoBrute()

        {
            var locations = _nodes.Keys.Where(n => n.EndsWith("A")).ToList();
            long steps = 0;

            while (locations.Any(v => !v.EndsWith("Z")))
            {
                var d = steps % _directions.Length;
                locations = locations.Select(l => d == 'L' ? _nodes[l].Item1 : _nodes[l].Item2).ToList();
                steps++;
            }

            Console.WriteLine($"Day Eight Part Two: " + steps);
        }
    }

    internal class Node
    {
        internal string Name { get; set; }
        internal string Left { get; set; }
        internal string Right { get; set; }
    }

}
