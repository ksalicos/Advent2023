using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal static class Shared
    {
        internal static List<long> GetNumbers(string s)
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

        internal static List<int> GetInts(string s)
        {
            // Strip digits from a string, any non-digit is considered a separator.
            var r = new StringBuilder();
            var result = new List<int>();
            foreach (var c in s)
            {
                if (char.IsDigit(c)) r.Append(c);
                if (c != ' ' || r.Length == 0) continue;
                result.Add(int.Parse(r.ToString()));
                r.Clear();
            }

            if (r.Length != 0)
            {
                result.Add(int.Parse(r.ToString()));
            }
            return result;
        }

        internal static List<string> GetPermutations(string s)
        {
            var result = new List<string>();
            heaps_algorithm(s.ToCharArray(), s.Length, result, 0);
            return result;
        }

        private static int maxd = 0;
        private static int counter = 0;

        private static void heaps_algorithm(char[] s, int size, List<string> target, int depth)
        {
            counter++;

            void Swap(int i, int j)
            {
                (s[i], s[j]) = (s[j], s[i]);
            }

            if (size == 1)
            {
                var n = new string(s);
                if (target.Contains(n)) return;
                Console.WriteLine(n);
                target.Add(n);
                return;
            }
            Console.WriteLine($"{new string(s)} {depth} {counter}");

            for (var i = 0; i < size; i++)
            {
                heaps_algorithm(s, size - 1, target, depth + 1);
                Swap(size % 2 == 0 ? i : 0, size - 1);
                heaps_algorithm(s, size - 1, target, depth + 1);
            }
        }
    }
}