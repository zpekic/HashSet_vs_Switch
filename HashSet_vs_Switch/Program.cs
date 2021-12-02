using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace HashSet_vs_Switch
{
    class Program
    {
        static readonly HashSet<char> CsvInjectableChars = new HashSet<char> { '=', '-', '"', '@', '+' };
        static List<string> randomStrings = new List<string>();
        static List<string> sanitizedByHashSet = new List<string>();
        static List<string> sanitizedBySwitch = new List<string>();

        static Random randomizer = new Random(DateTime.Now.Second);

        static string SanitizeByHashSet(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                if (CsvInjectableChars.Contains(input[0]))
                {
                    input = "'" + input;
                }
            }
            return input;
        }

        static string SanitizeBySwitch(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
            {
                switch (input[0])
                {
                    case '=':
                    case '-':
                    case '"':
                    case '@':
                    case '+':
                        return "'" + input;
                    default:
                        break;
                }
            }
            return input;
        }

        static void Main(string[] args)
        {
            // generate random strings
            int randCount = int.Parse(args[0]);

            Console.WriteLine($"Generating {randCount} random strings");
            for (int i = 0; i <= randCount; i++)
            {
                StringBuilder sb = new StringBuilder();
                int len = 10 + randomizer.Next(100 - 10 + 1); // they will be 10 - 100 chars long

                for (int j = 0; j < len; j++)
                {
                    sb.Append(Convert.ToChar(32 + randomizer.Next(126 - 32 + 1)));
                }
                randomStrings.Add(sb.ToString());
            }

            // run HashSet sanitize algorithm on random strings
            Stopwatch hashSetWatch = new Stopwatch();
            Console.Write($"HashSet algorithm: ");
            hashSetWatch.Start();
            foreach(string randomString in randomStrings)
            {
                string sanitized = SanitizeByHashSet(randomString);
                if (sanitized.Length != randomString.Length)
                {
                    sanitizedByHashSet.Add(randomString);
                }
            }
            hashSetWatch.Stop();
            Console.WriteLine($"{sanitizedByHashSet.Count} strings escaped in {hashSetWatch.ElapsedMilliseconds} ms.");
            
            // run Switch escape algorithm on random strings
            Stopwatch switchWatch = new Stopwatch();
            Console.Write($"Switch  algorithm: ");
            switchWatch.Start();
            foreach (string randomString in randomStrings)
            {
                string sanitized = SanitizeBySwitch(randomString);
                if (sanitized.Length != randomString.Length)
                {
                    sanitizedBySwitch.Add(randomString);
                }
            }
            switchWatch.Stop();
            Console.WriteLine($"{sanitizedBySwitch.Count} strings escaped in {switchWatch.ElapsedMilliseconds} ms.");

        }
    }
}
