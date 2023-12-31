﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent2023
{
    internal class Day01
    {
        internal void Go()
        {
            var sum = 0;
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day01input.txt");
                var line = sr.ReadLine();
                while (line != null)
                {
                    sum += ReadCalibrationValue(line);
                    line = sr.ReadLine();
                }
                sr.Close();
                Console.WriteLine("Day One: " + sum);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        internal static int ReadCalibrationValue(string s)
        {
            int? firstValue = null;
            int? secondValue = null;

            var i = 0;
            while (i < s.Length)
            {
                var c = s[i];
                if (char.IsDigit(c))
                {
                    var v = (int)char.GetNumericValue(c);
                    firstValue ??= v;
                    secondValue = v;
                }
                else
                {
                    var j = i + 1;
                    while (j < s.Length && !char.IsDigit(s[j]))
                    {
                        var v = GetNumericValue(s.Substring(i, j - i + 1));
                        if (v != -1)
                        {
                            firstValue ??= v;
                            secondValue = v;
                        }
                        j++;
                    }
                }
                i++;
            }

            if (firstValue == null)
            {
                throw new Exception("No values found");
            }

            var value = firstValue * 10 + secondValue;
            return value.Value;
        }

        private static int GetNumericValue(string s)
        {
            return s.ToLower() switch
            {
                "one" => 1,
                "two" => 2,
                "three" => 3,
                "four" => 4,
                "five" => 5,
                "six" => 6,
                "seven" => 7,
                "eight" => 8,
                "nine" => 9,
                "zero" => 0,
                _ => -1
            };
        }
    }
}
