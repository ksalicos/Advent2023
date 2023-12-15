using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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

                OldPartOne();
                PartOne();
                PartTwo();
            }
            catch (Exception e)
            {
                Log("Exception: " + e.Message);
            }
        }

        private void ProcessLine(string line)
        {
            var s = line.Split(" ");
            _lines.Add((s[0], s[1].Split(",").Select(int.Parse).ToArray()));
        }

        private void PartOne()
        {
            Dictionary<(int, int), List<string>> patterns = new();

            long count = 0;

            foreach (var line in _lines)
            {
                var logpattern = "";
                log = line.pattern == logpattern;
                //log = true;

                var total = CanFitToo(line.pattern, line.key);
                Log($"{line.pattern} {string.Join(",", line.key)}: {total}", ConsoleColor.Cyan);
                Log("---------------------");
                var h = hash(line.pattern, line.key);
                var expected = _matches[h];
                if (total != expected)
                {
                    log = true;
                    _states.Clear();
                    OldPartOne(h);
                    CanFitToo(line.pattern, line.key);
                    throw new Exception($"Error in count for input: {line.pattern}, expected {expected}, received {total}");
                }

                count += total;
            }

            Log("Day Twelve Part One: " + count, true);
        }

        /* Part one test:
         ???.### 1,1,3 - 1 arrangement
        .??..??...?##. 1,1,3 - 4 arrangements
        ?#?#?#?#?#?#?#? 1,3,1,6 - 1 arrangement
        ????.#...#... 4,1,1 - 1 arrangement
        ????.######..#####. 1,6,5 - 4 arrangements
        ?###???????? 3,2,1 - 10 arrangements
         */

        private void OldPartOne(string test = null)
        {
            Dictionary<(int, int), List<string>> patterns = new();

            long count = 0;

            foreach (var line in _lines)
            {
                List<string> permutations = new() { "" };
                var brokenTotal = line.key.Sum();

                foreach (var t in line.pattern)
                {
                    var next = new List<string>();

                    foreach (var c in permutations)
                    {
                        if (t != '?')
                        {
                            next.Add(c + t);
                        }
                        else
                        {
                            var broke = c.Count(x => x is '#');

                            if (broke < brokenTotal)
                            {
                                next.Add(c + "#");
                            }

                            if (line.pattern.Length - c.Length >= brokenTotal - broke)
                            {
                                next.Add(c + ".");
                            }
                        }
                    }
                    permutations = next;
                }

                var v = permutations.Where(p => IsValid(p, line.key, 1)).ToList();
                var p = v.Count();
                if (hash(line.pattern, line.key) == test)
                    v.ForEach(a=>Log(a, always:true));
                _matches[hash(line.pattern, line.key)] = p;
                count += p;
            }

            if (test == null)
            {
                Log("Day Twelve Part One (correct): " + count, true);
            }
        }

        private Dictionary<string, int> _matches = new();

        private void PartTwo()
        {
            long count = 0;
            foreach (var line in _lines)
            {
                var pattern = $"{line.pattern}?{line.pattern}?{line.pattern}?{line.pattern}?{line.pattern}";
                var key = line.key.Concat(line.key.Concat(line.key.Concat(line.key.Concat(line.key)))).ToArray();

                var total = CanFitToo(pattern, key);
                Log($"{pattern} {string.Join(",", key)}: {total}", ConsoleColor.Cyan, true);
                Log("---------------------");
                count += total;
            }

            Console.WriteLine("Day Twelve Part Two: " + count);
        }

        private bool log;

        void Log(string s, bool always = false)
        {
            if (log || always) Console.WriteLine(s);
        }

        void Log(string s, ConsoleColor color, bool always = false)
        {
            if (log || always)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(s);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /*
         Test:

        ???.### 1,1,3 - 1 arrangement
        .??..??...?##. 1,1,3 - 16384 arrangements
        ?#?#?#?#?#?#?#? 1,3,1,6 - 1 arrangement
        ????.#...#... 4,1,1 - 16 arrangements
        ????.######..#####. 1,6,5 - 2500 arrangements
        ?###???????? 3,2,1 - 506250 arrangements
        After unfolding, adding all of the possible arrangement counts together produces 525152.
         
         */

        private readonly Dictionary<string, List<string>> _states = new();
        private static string hash(string s, int[] k) => $"{TrimDots(s)}|{string.Join(",", k)}";

        private List<string> losers = new();
        private List<string> winners = new();

        private void FailState(string s)
        {
            losers.Add(s);
            _states[s] = new List<string>();
        }

        private void WinState(string s)
        {
            winners.Add(s);
            _states[s] = new List<string>() { "Winner" };
        }

        private Dictionary<int, List<string>> perms = new();

        private List<string> GetSome(int i)
        {
            if (perms.ContainsKey(i)) return perms[i];

            if (i == 1) return new List<string>() { "?", "." };
            return GetSome(i - 1)
                .SelectMany(x => new List<string> { "?" + x, "." + x })
                .ToList();
        }

        private long CanFitToo(string s, int[] key)
        {
            try
            {
                List<string> list = new() { hash(s, key) };
                Log("trying: " + list[0]);
                var count = 0;
                while (list.Any())
                {
                    Log("List count: " + list.Count);
                    var state = list.First();
                    list.Remove(state);

                    var t = state.Split('|');
                    var pattern = t[0];
                    if (!int.TryParse(t[1].Split(',')[0], out var length))
                    {
                        throw new Exception("Bad input: " + state);
                    };

                    if (pattern == "")
                    {
                        Log("Stop!");
                    }

                    if (_states.ContainsKey(state))
                    {
                        if (_states[state].Contains("Winner"))
                        {
                            Log("Found a winner in the cache! " + state, ConsoleColor.DarkGreen);
                            count += _states[state].Count(c => c == "Winner");
                            var more = _states[state].Where(x => x != "Winner").ToList();
                            if (more.Any())
                            {
                                list.AddRange(more);
                                Log("Extras found: " + more.Count);
                                more.ForEach(f => Log("- " + f));
                            }

                            continue;
                        }

                        if (_states[state].Count == 0)
                        {
                            Log("Found a loser in the cache! " + state, ConsoleColor.DarkRed);
                            continue;
                        }

                        list.AddRange(_states[state]);
                        Log("Cache Hit: " + state, ConsoleColor.DarkCyan);
                        Log($"Added {_states[state].Count}, Now: {list.Count}");
                        _states[state].ForEach(re => Log(re));
                        Log("---");
                        continue;
                    }

                    Log($"{length}: {state}", ConsoleColor.Yellow);

                    if (pattern.Length < length)
                    {
                        FailState(state);
                        Log($"Too long: {state}", ConsoleColor.DarkRed);
                        continue;
                    }

                    var last = !t[1].Contains(',');
                    var canBite = !pattern[..length].Contains('.')
                        && (pattern.Length == length || pattern[length] != '#');

                    if (!last && length + 1 >= pattern.Length)
                    {
                        FailState(state);
                        Log($"Oops, ate it all: {state}", ConsoleColor.DarkRed);
                        continue;
                    }

                    if (pattern.Length == length)
                    {
                        if (canBite)
                        {
                            count++;
                            WinState(state);
                            Log($"Winner, same length: {state}", ConsoleColor.Green);
                            continue;
                        }

                        FailState(state);
                        Log($"Bite isn't tasty: {state}", ConsoleColor.DarkRed);
                        continue;
                    }

                    var blocked = pattern[length] == '#';
                    var next = last ? "" : t[1][(t[1].IndexOf(',') + 1)..];
                    var bitten = $"{TrimDots(pattern[(length + 1)..])}|{next}";
                    var skipped = $"{TrimDots(pattern[1..])}|{t[1]}";
                    var endingClear = pattern[length..^0].All(x => x != '#');
                    var mustBite = pattern.StartsWith('#');

                    if (mustBite && !canBite)
                    {
                        FailState(state);
                        Log($"So hungry: {state}", ConsoleColor.DarkRed);
                        continue;
                    }

                    if (last
                        && canBite
                        && endingClear)
                    {
                        count++;
                        if (mustBite)
                        {
                            WinState(state);
                            Log($"Winner, same length... almost: {state}", ConsoleColor.Green);
                        }
                        else
                        {
                            // We might be able to go on, so take the win and re-eval with a #
                            Log("Not having this code here made the progam not run");
                            list.Add(skipped);

                            _states[state] = new List<string>
                            {
                                skipped, "Winner"
                            };
                        }
                        continue;
                    }

                    if (last
                        && mustBite
                        && !endingClear)
                    {
                        FailState(state);
                        Log($"Can't eat it all: {state}", ConsoleColor.DarkRed);
                        continue;
                    }

                    if (mustBite)
                    {
                        list.Add(bitten);
                        Log($"We must bite, list: {list.Count} next: {bitten}");
                        continue;
                    }

                    var toAdd = new List<string>();

                    if (pattern.StartsWith('#'))
                    {
                        if (!canBite)
                        {
                            FailState(state);
                            Log($"Can't take bite: {state}", ConsoleColor.DarkRed);
                            continue;
                        }

                        if (last)
                        {
                            if (pattern[length..^0].Contains('#'))
                            {
                                FailState(state);
                                Log($"# in leftovers: {state}", ConsoleColor.DarkRed);
                                continue;
                            }

                            count++;
                            WinState(state);
                            Log($"Winner, safe after # bite: {state}", ConsoleColor.Green);
                            continue;
                        }

                        if (blocked)
                        {
                            FailState(state);
                            Log($"Blocked: {state}", ConsoleColor.DarkRed);
                            continue;
                        }

                        Log($"# adds {bitten}");
                        toAdd.Add(bitten);
                    }

                    if (pattern.StartsWith('?'))
                    {
                        if (last)
                        {
                            if (canBite && endingClear)
                            {
                                count++;
                                Log($"Winner, but might eat more: {state}", ConsoleColor.Green);
                                toAdd.Add(skipped);
                                list.Add(skipped);
                                Log("? adds " + skipped);
                                toAdd.Add("Winner");
                                _states[state] = toAdd;
                                continue;
                            }
                            else
                            {
                                toAdd.Add(skipped);
                                Log($"? can't win and skips {skipped}");
                            }
                        }
                        else
                        {
                            toAdd.Add(skipped);
                            Log($"? adds {skipped}");

                            if (blocked)
                            {
                                Log("? is blocked and doesn't bite");
                            }
                            else if (!canBite)
                            {
                                Log("? can't bite");
                            }
                            else
                            {
                                toAdd.Add(bitten);
                                Log($"? bites {bitten}");
                            }
                        }
                    }

                    _states[state] = toAdd;
                    list.AddRange(toAdd);
                    Log($"Added {toAdd.Count}, Now: {list.Count}");
                    Log("---");
                    list.Sort((a, b) => a.Length > b.Length ? 1 : 0);
                }

                return count;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Log(ex.StackTrace);
                throw;
            }
        }

        private static string TrimDots(string s)
        {
            while (s.StartsWith('.')) s = s[1..];
            while (s.EndsWith('.')) s = s[..^1];
            while (s.Contains("..")) s = s.Replace("..", ".");
            return s;
        }

        // Was used in original part one, keeping in case
        private bool IsValid(string pattern, int[] keys, int multilpier)
        {
            var count = 0;
            var broken = false;
            var k = 0;

            foreach (var t in pattern)
            {
                if (t == '#')
                {
                    if (k == keys.Length * multilpier) return false;
                    count++;
                    broken = true;
                }
                else
                {
                    if (!broken) continue;
                    if (keys[k % keys.Length] != count) return false;
                    count = 0;
                    broken = false;
                    k++;
                }
            }

            if (broken)
            {
                if (keys[k % keys.Length] != count) return false;
                k++;
            }

            return k == keys.Length * multilpier;
        }
    }
}
