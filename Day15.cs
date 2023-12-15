using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Advent2023.Shared;

namespace Advent2023
{
    internal class Day15
    {
        private List<string> _steps;

        internal void Go()
        {
            try
            {
                var sr = new StreamReader("C:\\code\\advent2023\\day15input.txt");

                var line = sr.ReadLine();
                _steps = line.Split(',').ToList();
                sr.Close();


                Test();
                PartOne();
                TestTwo();
                PartTwo();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }

        int hash(string s)
        {
            var cur = 0;
            foreach (var c in s)
            {
                var i = (int)c;
                cur = ((cur + i) * 17) % 256;
            }

            return cur;
        }

        private const string TestString = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";
        private void Test()
        {
            var t = TestString.Split(',').Select(hash).Sum();
            Console.WriteLine("Test Value: " + t);
        }

        private void PartOne()
        {
            var t = _steps.Select(hash).Sum();
            Console.WriteLine("Day Fifteen Part One: " + t);
        }

        private void TestTwo()
        {
            LogActive = true;
            var testAnswer = 145;
            var instructions = TestString.Split(',').ToList();

            var sum = DoTwo(instructions);
            if (sum != testAnswer) Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Test Two: " + sum);
            Console.ForegroundColor = ConsoleColor.White;
            LogActive = false;
        }

        class Lens
        {
            internal string Label { get; set; }
            internal int Focus { get; set; }

            public override string ToString()
            {
                return $"[{Label}:{Focus}]";
            }
        }

        int DoTwo(List<string> inputs)
        {
            var boxes = new Dictionary<int, List<Lens>>();

            foreach (var i in inputs)
            {
                var instruction = i.Split("=");
                if (instruction.Length == 1)
                {
                    instruction[0] = instruction[0][..^1];
                }

                var h = hash(instruction[0]);
                if (instruction.Length == 1)
                {
                    Log($"Box {h}, Label: {instruction[0]}");
                }
                else
                {
                    Log($"Box {h}, Label: {instruction[0]}, focus: {instruction[1]}");
                }

                if (!boxes.ContainsKey(h))
                {
                    boxes[h] = new List<Lens>();
                }

                var lens = boxes[h].FirstOrDefault(x => x.Label == instruction[0]);

                if (instruction.Length == 1)
                {
                    boxes[h].Remove(lens);
                }
                else
                {
                    if (lens == null) boxes[h].Add(new Lens { Label = instruction[0], Focus = int.Parse(instruction[1]) });
                    else
                    {
                        Log($"Lens {lens.Label}, Old Focus: " + lens.Focus);
                        lens.Focus = int.Parse(instruction[1]);
                        Log($"Focus changed to " + lens.Focus);
                    }
                }

                foreach (var b in boxes)
                {
                    Log($"Box {b.Key}: {string.Join(", ", b.Value.Select(x => x.ToString()))}");
                }

            }

            // return boxes.SelectMany(x => x.Value.Select((v, i) => (x.Key + 1) * i * v.Focus)).Sum();
            var total = 0;

            Log("----");

            foreach (var b in boxes)
            {
                var boxTotal = 0;
                // Thanks bcahill
                foreach (var (lens, i) in b.Value.Select((value, i) => (value, i)))
                {

                    var lensValue = (b.Key + 1) * (i + 1) * lens.Focus;
                    Log($"Lens {lens.Label}: Box {b.Key + 1}, Position {i + 1}, Focus {lens.Focus} = {lensValue}", ConsoleColor.Yellow);
                    boxTotal += lensValue;
                }

                Log($"Box {b.Key} total: {boxTotal}", ConsoleColor.Cyan);

                total += boxTotal;
            }

            return total;
        }

        private void PartTwo()
        {
            var sum = DoTwo(_steps);
            Console.WriteLine("Day Fifteen Part Two: " + sum);
        }
    }
}
