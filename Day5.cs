using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day5
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
                _seeds = Shared.GetNumbers(line);
                _seedMap = _seeds.ToDictionary(s => s, s => s);
                var mapKey = string.Empty;
                line = sr.ReadLine();

                void UpdateSeeds(string mk)
                {
                    foreach (var k in _seedMap.Keys)
                    {
                        _seedMap[k] = MapValue(_seedMap[k], _maps[mk]);
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
                        var map = Shared.GetNumbers(line);
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
                PartTwo();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        private long MapValue(long k, List<Map> maps)
        {
            var map = maps.FirstOrDefault(x => k >= x.Source && k <= x.Source + x.Length);
            if (map == null) return k;
            var v = map.Destination + (k - map.Source);
            return v;
        }

        private long SeedToLocation(long seed)
        {
            var soil = MapValue(seed, _maps["seed-to-soil"]);
            var fertilizer = MapValue(soil, _maps["soil-to-fertilizer"]);
            var water = MapValue(fertilizer, _maps["fertilizer-to-water"]);
            var light = MapValue(water, _maps["water-to-light"]);
            var temperature = MapValue(light, _maps["light-to-temperature"]);
            var humidity = MapValue(temperature, _maps["temperature-to-humidity"]);
            var location = MapValue(humidity, _maps["humidity-to-location"]);

            return location;
        }

        private void PartOne()
        {
            var l = _seeds.Select(SeedToLocation).Min();

            Console.WriteLine("Day Five Part One: " + l);
        }

        private void PartTwo()
        {
            var l = long.MaxValue;
            for (var i = 0; i < _seeds.Count; i += 2)
            {
                var r = new Range(_seeds[i], _seeds[i + 1]);
                var location = SeedsToLocation(r);
                if (location < l)
                {
                    l = location;
                }
            }

            Console.WriteLine("Day Five Part Two: " + l);
        }

        private long SeedsToLocation(Range r)
        {
            var seeds = new List<Range> { r };
            var soil = MapValue(seeds, _maps["seed-to-soil"]);
            var fertilizer = MapValue(soil, _maps["soil-to-fertilizer"]);
            var water = MapValue(fertilizer, _maps["fertilizer-to-water"]);
            var light = MapValue(water, _maps["water-to-light"]);
            var temperature = MapValue(light, _maps["light-to-temperature"]);
            var humidity = MapValue(temperature, _maps["temperature-to-humidity"]);
            var location = MapValue(humidity, _maps["humidity-to-location"]);

            return location.Min(s => s.Start);
        }

        private List<Range> MapValue(List<Range> ranges, List<Map> maps)
        {
            var holding = ranges.Select(r => new Range(r.Start, r.Length)).ToList();

            foreach (var map in maps)
            {
                holding = DivideRangesByMap(holding, map);
            }

            var result = new List<Range>();
            foreach (var range in holding)
            {
                result.Add(new Range(MapValue(range.Start, maps), range.Length));
            }

            return result;
        }

        List<Range> DivideRangesByMap(List<Range> r, Map m, int depth = 0)
        {
            return r.SelectMany(x => DivideRangeByMap(x, m)).ToList();
        }

        List<Range> DivideRangeByMap(Range r, Map m, int depth = 0)
        {
            var result = new List<Range>();
            // Range doesn't intersect map
            if (r.End < m.Source || r.Start > m.SourceEnd)
            {
                return new List<Range> { r };
            }

            // left intersect
            if (r.Start < m.Source)
            {
                return DivideRangesByMap(r.DivideAt(m.Source), m, depth + 1);
            }

            // right intersect
            if (r.End > m.SourceEnd)
            {
                return DivideRangesByMap(r.DivideAt(m.SourceEnd+1), m, depth + 1);
            }

            return new List<Range> { r };
        }

        internal class Map
        {
            internal Map(List<long> src)
            {
                if (src.Count != 3) throw new Exception("Wrong number of numbers");
                Destination = src[0];
                Source = src[1];
                Length = src[2];
                SourceEnd = Source + Length - 1;
            }

            public override string ToString()
            {
                return $"[{Source}-{SourceEnd}]->[{Destination}-{Destination + Length - 1}";
            }

            internal long Destination { get; }
            internal long Source { get; }
            internal long SourceEnd { get; }
            internal long Length { get; }
        }

        internal class Range
        {
            internal Range(long start, long length)
            {
                Start = start;
                Length = length;
                End = start + length - 1;
            }

            internal long Start { get; }
            internal long Length { get; }
            internal long End { get; }

            public override string ToString()
            {
                return $"[{Start}-{End}]({Length})";
            }

            internal List<Range> DivideAt(long newStart)
            {
                if (newStart < Start || newStart > End)
                {
                    return new List<Range> { this };
                }

                var l1 = newStart - Start;
                var l2 = End - newStart + 1;

                return new List<Range>
                {
                    new Range(Start, l1),
                    new Range(newStart, l2)
                };
            }
        }

    }
}
