using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class DayFive
    {
        private List<long>? _seeds;
        private readonly Dictionary<string, List<Map>> _maps = new();
        private Dictionary<long, long>? _seedMap;

        internal void Go()
        {
            try
            {
                // var sr = new StreamReader("C:\\code\\advent2023\\day5test.txt");
                var sr = new StreamReader("C:\\code\\advent2023\\day5input.txt");
                var line = sr.ReadLine();
                _seeds = GetNumbers(line);
                _seedMap = _seeds.ToDictionary(s => s, s => s);
                var mapKey = string.Empty;
                line = sr.ReadLine();

                void UpdateSeeds(string mk)
                {
                    foreach (var k in _seedMap.Keys)
                    {
                        _seedMap[k] = SearchMaps(_seedMap[k], mk);
                    }
                }

                while (line != null)
                {
                    // New header detected, close old one and update maps.
                    if (line.EndsWith("map:"))
                    {
                        // First time?
                        if (mapKey != string.Empty)
                        {
                            UpdateSeeds(mapKey);
                        }
                        mapKey = line.Split(" ")[0];
                        _maps[mapKey] = new List<Map>();
                    }
                    else
                    {
                        var map = GetNumbers(line);
                        if (map.Count == 3)
                        {
                            _maps[mapKey].Add(new Map(map));
                        }
                        // We are just ignoring blank lines.
                    }
                    line = sr.ReadLine();
                };
                UpdateSeeds(mapKey);

                sr.Close();

                PartOne();
                // PartTwo();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private static List<long> GetNumbers(string s)
        {
            // Strip digits from a string, any non-digit is considered a separator.
            var r = new StringBuilder();
            var result = new List<long>();
            foreach (var c in s)
            {
                if (char.IsDigit(c)) r.Append(c);
                if (c != ' ' || r.Length == 0) continue;
                result.Add(long.Parse(r.ToString()));
                r.Clear();
            }

            if (r.Length != 0)
            {
                result.Add(long.Parse(r.ToString()));
            }
            return result;
        }

        private long SearchMaps(long k, string m)
        {
            var map = _maps[m].FirstOrDefault(x => k >= x.Key && k <= x.Key + x.Length);
            if (map == null) return k;
            var v = map.Start + (k - map.Key);
            return v;
        }

        private void PartOne()
        {
            var l = _seeds.Select(s => SeedToLocation(s)).Prepend(long.MaxValue).Min();

            Console.WriteLine("Day Five Part One: " + l);
        }

        private void PartTwo()
        {

        }

        long SeedToLocation(long s)
        {
            var soil = SearchMaps(s, "seed-to-soil");
            var fertilizer = SearchMaps(soil, "soil-to-fertilizer");
            var water = SearchMaps(fertilizer, "fertilizer-to-water");
            var light = SearchMaps(water, "water-to-light");
            var temperature = SearchMaps(light, "light-to-temperature");
            var humidity = SearchMaps(temperature, "temperature-to-humidity");
            var location = SearchMaps(humidity, "humidity-to-location");

            // Console.WriteLine($"Seed {s}, soil {soil}, fertilizer {fertilizer}, water {water}, light {light}, temperature {temperature}, humidity {humidity}, location {location}.");

            return location;
        }

        internal class Map
        {
            internal Map(List<long> src)
            {
                if (src.Count != 3) throw new Exception("Wrong number of numbers");
                Start = src[0];
                Key = src[1];
                Length = src[2];
            }
            internal long Key { get; set; }
            internal long Start { get; set; }
            internal long Length { get; set; }
        }
    }
}
