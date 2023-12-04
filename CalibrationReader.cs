using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal static class CalibrationReader
    {
        internal static int ReadCalibrationValue(string s)
        {
            int? firstValue = null;
            int? secondValue = null;

            foreach (var t in s)
            {
                var v = (int)char.GetNumericValue((t));
                if (v == -1) continue;
                firstValue ??= v;
                secondValue = v;
            }

            if (firstValue == null)
            {
                throw new Exception("No values found");
            }
            var value = firstValue * 10 + secondValue;
            return value.Value;
        }
    }
}
