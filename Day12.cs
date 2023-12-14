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
                var sr = new StreamReader("C:\\code\\advent2023\\day12test.txt");
                // var sr = new StreamReader("C:\\code\\advent2023\\day12input.txt");

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
                Log("Exception: " + e.Message);
            }
        }

        private void ProcessLine(string line)
        {
            var s = line.Split(" ");
            _lines.Add((s[0], s[1].Split(",").Select(int.Parse).ToArray()));
        }

        private bool IsValid(string pattern, List<int> keys, int multilpier)
        {
            var count = 0;
            var broken = false;
            var k = 0;

            foreach (var t in pattern)
            {
                if (t == '#')
                {
                    if (k == keys.Count * multilpier) return false;
                    count++;
                    broken = true;
                }
                else
                {
                    if (!broken) continue;
                    if (keys[k % keys.Count] != count) return false;
                    count = 0;
                    broken = false;
                    k++;
                }
            }

            if (broken)
            {
                if (keys[k % keys.Count] != count) return false;
                k++;
            }

            return k == keys.Count * multilpier;
        }

        private void PartOne()
        {
            Dictionary<(int, int), List<string>> patterns = new();

            long count = 0;

            foreach (var line in _lines)
            {
                log = line.pattern == ".??..??...?##.";

                var total = CanFitToo(line.pattern, line.key);
                //Log($"{line.pattern} {string.Join(", ", line.key)} expands to");
                Log($"{line.pattern} {string.Join(",", line.key)}: {total}", ConsoleColor.Cyan, true);
                Log("---------------------");
                // Console.ReadLine();
                count += total;
            }

            //foreach (var line in _lines)
            //{
            //    List<string> permutations = new() { "" };
            //    var brokenTotal = line.key.Sum();

            //    foreach (var t in line.pattern)
            //    {
            //        var next = new List<string>();

            //        foreach (var c in permutations)
            //        {
            //            if (t != '?')
            //            {
            //                next.Add(c + t);
            //            }
            //            else
            //            {
            //                var broke = c.Count(x => x is '#');

            //                if (broke < brokenTotal)
            //                {
            //                    next.Add(c + "#");
            //                }

            //                if (line.pattern.Length - c.Length >= brokenTotal - broke)
            //                {
            //                    next.Add(c + ".");
            //                }
            //            }
            //        }
            //        permutations = next;
            //    }

            //    count += permutations.Count(p => IsValid(p, line.key, 1));
            //}

            Log("Day Twelve Part One: " + count, true);
        }

        private void PartTwo()
        {
            // Line One:
            // ..?????#?? 4,1

            long count = 0;
            foreach (var line in _lines)
            {
                var pattern = $"{line.pattern}?{line.pattern}?{line.pattern}?{line.pattern}?{line.pattern}";
                var key = line.key.Concat(line.key.Concat(line.key.Concat(line.key.Concat(line.key)))).ToArray();


                var total = CanFitToo(pattern, key);
                //Log($"{line.pattern} {string.Join(", ", line.key)} expands to");
                Log($"{pattern} {string.Join(",", key)}: {total}", ConsoleColor.Cyan, true);
                Log("---------------------");
                // Console.ReadLine();
                count += total;
            }

            Console.WriteLine("Day Twelve Part Two: " + count);
        }

        private bool log = false;

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
            Log("Found a loser: " + s, ConsoleColor.Red);
            // Console.ReadLine();
            losers.Add(s);
            _states[s] = new List<string>();
        }

        private void WinState(string s)
        {
            Log("Found a winner: " + s, ConsoleColor.Green);
            // Console.ReadLine();
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
                var count = 0;
                while (list.Any())
                {
                    var state = list.First();
                    list.Remove(state);

                    var t = state.Split('|');
                    var pattern = t[0];
                    var length = int.Parse(t[1].Split(',')[0]);

                    if (_states.ContainsKey(state))
                    {
                        if (_states[state].Contains("Winner"))
                        {
                            // Log("Found a winner in the cache! " + state);
                            count++;
                            continue;
                        }

                        if (_states[state].Count == 0)
                        {
                            // Log("Found a loser in the cache! " + state);
                            continue;
                        }

                        list.AddRange(_states[state]);
                        //Log("Cache Hit: " + state, ConsoleColor.DarkCyan);
                        //Log($"Added {_states[state].Count}, Now: {list.Count}");
                        //_states[state].ForEach(Log);
                        //Log("---");
                        continue;
                    }

                    Log($"{length}: {state}", ConsoleColor.Yellow);

                    if (pattern.Length < length || pattern[..length].Contains('.'))
                    {
                        FailState(state);
                        continue;
                    }

                    if (pattern.Length == length)
                    {
                        // We finished!
                        count++;
                        WinState(state);
                        continue;
                    }

                    var blocked = pattern[length] == '#';
                    var last = !t[1].Contains(',');
                    var next = last ? "" : t[1].Substring(t[1].IndexOf(',') + 1);

                    var toAdd = new List<string>();

                    if (pattern.StartsWith('#'))
                    {
                        if (last)
                        {
                            if (pattern[length..^0].Contains('#'))
                            {
                                FailState(state);
                                continue;
                            }

                            count++;
                            WinState(state);
                            continue;
                        }

                        if (blocked)
                        {
                            FailState(state);
                            continue;
                        }

                        toAdd.Add($"{TrimDots(pattern.Substring(length + 1))}|{next}");
                    }

                    if (pattern.StartsWith('?'))
                    {
                        if (last)
                        {
                            if (pattern[length..^0].Contains('#'))
                            {
                                toAdd.Add($"{TrimDots(pattern.Substring(1))}|{t[1]}");
                            }
                            else
                            {
                                count++;
                                toAdd.Add($"{TrimDots(pattern.Substring(1))}|{t[1]}");
                            }
                        }
                        else
                        {
                            if (!blocked)
                                toAdd.Add($"{TrimDots(pattern.Substring(length + 1))}|{next}");
                            toAdd.Add($"{TrimDots(pattern.Substring(1))}|{t[1]}");
                        }
                    }

                    _states[state] = toAdd;
                    list.AddRange(toAdd);
                    Log($"Added {toAdd.Count}, Now: {list.Count}");
                    toAdd.ForEach(a => Log(a));
                    Log("---");
                    // Console.ReadLine();
                }

                return count;
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                throw;
            }
        }


        private long CanFit(string s, int idx, int[] key)
        {
            // Log($"{s} {idx}");
            var chunk = TrimDots(s);
            if (idx >= key.Length) throw new Exception();
            var len = key[idx];

            if (len > chunk.Length) return 0;

            var potential = chunk.Substring(0, len);
            if (potential.Any(c => c == '.'))
            {
                return 0;
            }

            var last = idx == key.Length - 1;
            if (len == chunk.Length)
            {
                return last ? 1 : 0;
            }

            var blocked = chunk[len] == '#';

            if (chunk[0] == '#')
            {
                if (last)
                {
                    return chunk.Substring(len).All(x => x != '#') ? 1 : 0;
                }
                if (blocked) return 0;
                return CanFit(chunk.Substring(len + 1), idx + 1, key);
            }

            if (chunk[0] == '?')
            {
                if (last && chunk.Substring(len).All(x => x != '#')) return 1;
                var h = blocked ? 0 : CanFit(chunk.Substring(len + 1), idx + 1, key);
                var q = CanFit(chunk.Substring(1), idx, key);
                return h + q;
            }

            throw new Exception("huh");
        }

        private int GetFitsa(string pattern, List<int> key)
        {
            var list = new List<string> { pattern };
            for (int i = 0; i < 5; i++)
                foreach (var k in key)
                {
                    list = GetNext(list, k);
                };
            return list.Count(l => l == string.Empty);
        }

        private static string TrimDots(string s)
        {
            // TODO: What's range indexer?
            while (s.StartsWith('.')) s = s[1..];
            while (s.EndsWith('.')) s = s[..^1];
            while (s.Contains("..")) s = s.Replace("..", ".");
            return s;
        }

        List<string> GetNext(List<string> lists, int length)
        {
            var results = new List<string>();

            foreach (string s in lists)
            {
                if (s.Length == length && !s.Contains('.'))
                {
                    results.Add(string.Empty);
                    continue;
                }

                for (var i = 0; i + length < s.Length; i++)
                {
                    if (s[i] == '.') continue;
                    var idx = s.IndexOf('.', i);
                    if (idx != -1 && idx < i + length) break;
                    var next =
                        i + length == s.Length
                        ? '!'
                        : s[i + length];
                    if (s[i] == '#' && next != '#')
                    {
                        results.Add(s.Substring(i + length));
                        break;
                    }
                    if (s[i] == '?' && next != '#')
                    {
                        results.Add(s.Substring(i + length));
                    }
                }
            }

            return results;
        }

        private void PartTw3o()
        {
            // Line One:
            // ..?????#?? 4,1

            var sum = 0;
            foreach (var line in _lines)
            {
                Log(line.pattern);
                var chunks = line.pattern.Split('.').Where(x => x.Length > 0).ToList();
                var idxs = new List<(string, int)> { ("", 0) };
                var dots = "";
                var chunk = "";

                for (var i = 0; i < line.pattern.Length; i++)
                {
                    Log($"Paths: " + idxs.Count);
                    if (line.pattern[i] == '.')
                    {
                        dots += ".";
                    }
                    else
                    {
                        chunk += line.pattern[i];
                    }

                    if (chunk.Length > 0 && (line.pattern[i] == '.' || i == line.pattern.Length - 1))
                    {
                        Log("Found Chunk " + chunk);
                        List<(string, int)> results = new List<(string, int)>();
                        foreach (var idx in idxs)
                        {
                            results.AddRange(GetAllPerms(chunk, idx.Item2, line.key)
                                .Select(r => (dots + idx.Item1 + r.consumed, r.curIdx)));
                        }

                        chunk = "";
                        dots = "";
                        idxs = results;
                    }
                }

                sum += idxs.Count(i => i.Item2 == line.key.Length);
            }
            Log("Day Twelve Part Two: " + sum);
        }

        private List<(string consumed, int curIdx)> GetAllPerms(string s, int i, int[] key)
        {
            List<(string, int)> results = new();

            // We dug too deep.

            if (i >= key.Length)
            {
                Log($"Can't evaluate {s}, Too deep");
                return results;
            }

            var target = key[i];
            Log($"input {s} target {target}");

            // Bail if we can do no more
            if (target > s.Length)
            {
                Log("Bailin");
                return results;
            }

            // if all ? we could do nothing.
            if (s.All(x => x == '?'))
            {
                Log("Empty");
                results.Add((s.Replace('?', '.'), i));
            }

            var sub = s.Substring(0, target).Replace('?', '#');
            if (target == s.Length)
            {
                Log($"Returning {sub} and Bailin");
                results.Add((sub, i + i));
                return results;
            }

            if (i == key.Length - 1)
            {
                Log("Done here");
                return results;
            }

            // If the char after my target is a #, then we can't start here
            if (s[target] != '#')
            {
                sub = sub + ".";
                var n = s.Substring(target + 1);
                Log($"Trying {sub} in {n}");
                var next = GetAllPerms(n, i + 1, key);
                Log($"Found {next.Count} in {n} for substring {sub}, index {i + 1} ({key[i + 1]})");
                foreach (var perm in next)
                {
                    Log($"{sub}{perm.consumed}");
                    results.Add((sub + perm.consumed, perm.curIdx));
                }
            }

            // If there's a ?, try skipping it.
            if (s.StartsWith("?"))
            {
                var n = s.Substring(1);
                sub = ".";
                var next = GetAllPerms(n, i, key);
                Log($"Found {next.Count} in {n} for substring {sub}, index {i + 1} ({key[i]})");
                foreach (var perm in next)
                {
                    Log($"{sub}{perm.consumed}");
                    results.Add((sub + perm.consumed, perm.curIdx));
                }
            }

            Log($"Returning {results.Count} results");
            return results;
        }

        private void PartTwo2()
        {
            var sum = 0;

            foreach (var line in _lines)
            {
                List<Node> paths = new() { new Node() };

                foreach (var c in line.pattern)
                {
                    List<Node> add = new();
                    List<Node> remove = new();

                    switch (c)
                    {
                        case '?':
                            foreach (var path in paths)
                            {
                                if (path.pen)
                                {
                                    if (path.idx >= line.key.Length)
                                    {
                                        remove.Add(path);
                                    }

                                    else
                                    {
                                        if (path.count == line.key[path.idx])
                                        {
                                            add.Add(new Node() { idx = path.idx + 1 });
                                        }
                                        path.count++;

                                        if (path.count > line.key[path.idx])
                                        {
                                            remove.Add(path);
                                        }
                                    }
                                }
                                else
                                {
                                    add.Add(new Node() { idx = path.idx, pen = true, count = 1 });
                                }
                            }
                            break;
                        case '.':
                            foreach (var path in paths)
                            {
                                if (path.pen)
                                {
                                    if (path.idx >= line.key.Length)
                                    {
                                        remove.Add(path);
                                    }
                                    else if (path.count == line.key[path.idx])
                                    {
                                        path.idx++;
                                        path.pen = false;
                                        path.count = 0;
                                    }
                                    else
                                    {
                                        remove.Add(path);
                                    }
                                }
                            }
                            break;
                        case '#':
                            foreach (var path in paths)
                            {
                                if (path.idx >= line.key.Length)
                                {
                                    remove.Add(path);
                                }

                                else if (path.pen)
                                {
                                    path.count++;
                                    if (path.count > line.key[path.idx])
                                    {
                                        remove.Add(path);
                                    }
                                }
                                else
                                {
                                    path.count = 1;
                                    path.pen = true;
                                }
                            }
                            break;
                    }

                    paths.RemoveAll(p => remove.Any(r => r.idx == p.idx && r.count == p.count && r.pen == p.pen));
                    paths.AddRange(add);
                }

                paths.RemoveAll(p => p.pen && p.idx >= line.key.Length);
                paths.RemoveAll(p => p.pen && p.idx != line.key.Length - 1 && p.count != line.key[p.idx]);
                paths.RemoveAll(p => !p.pen && p.idx != line.key.Length);
                sum += paths.Count;
            }

            Log("Day Twelve Part Two: " + sum);
        }
        internal class Node
        {
            internal int idx = 0;
            internal int count = 0;
            internal bool pen = false;
        }
    }
}
