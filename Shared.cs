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
    }
}
