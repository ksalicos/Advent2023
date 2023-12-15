using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using static Advent2023.Shared;

namespace Advent2023
{
    internal class Day12
    {
        private readonly List<(string pattern, int[] key)> _lines = new();

        internal void Go()
        {
            try
            {
                // var sr = new StreamReader("C:\\code\\advent2023\\day12test.txt");
                var sr = new StreamReader("C:\\code\\advent2023\\day12input.txt");

                var line = sr.ReadLine();
                while (line != null)
                {
                    if (line.Length > 0)
                        ProcessLine(line);
                    line = sr.ReadLine();
                }
                sr.Close();

                PartOne();
                // PartTwo();
            }
            catch (Exception e)
            {
                Log("Exception: " + e.Message);
            }
        }

        private void PartOne()
        {
            var sum = 0;
            foreach (var line in _lines)
            {
                sum += Solve(line.pattern, line.key);
            }

            Console.WriteLine(sum);
        }

        private int Solve(string pattern, int[] key)
        {
            var chunks = pattern.Split('.').Select(c => (c, key[0]));


            while (chunks.Any())
            {
                for (var i = 0; i < chunks.Count(); i++)
                {
                    for (var j = 0; j < key.Length; j++)
                    {

                    }
                }
            }

            return -1;
        }

        private Dictionary<(string chunk, int length), List<(string chunk, int length)>> _cache = new();
        private List<(string chunk, int length)> GetChunks(string chunk, int length)
        {
            if (_cache.ContainsKey((chunk, length))) return _cache[(chunk, length)];
            List<(string chunk, int length)> result = new List<(string chunk, int length)>();

            // If all ?, we can just do nothing.
            if (chunk.All(c => c == '?')) result.Add(("", length));

            // Rejects

            if (length > chunk.Length) return result; // Bite too long.
            // We're done, but there's still some to eat.
            if (length == 0 && chunk.Any(c => c == '#')) return result;

            // Tests
            var bite = (chunk[length..], -1);
            var blocked = chunk.Length > length && chunk[length] == '#';

            // if we have room to bite, do it
            if (!blocked)
            {
                result.Add(bite);
            }

            if (chunk.StartsWith('?'))
            {
                // We can skip if there's room
                if (chunk.Length > length)
                {
                    result.Add((chunk[1..], length));
                }
            }

            return result;
        }

        private void PartTwo()
        {
            throw new NotImplementedException();
        }

        private void ProcessLine(string line)
        {
            var s = line.Split(" ");
            _lines.Add((s[0], s[1].Split(",").Select(int.Parse).ToArray()));
        }

    }
}
