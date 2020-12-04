﻿using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace adventofcode
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Advent of Code");
            
            var debug = true;
            DayFour(debug);

            var cki = new ConsoleKeyInfo();

            do
            {
                while (Console.KeyAvailable == false)
                {
                    Thread.Sleep(250);
                }
                cki = Console.ReadKey(true);
                var day_select = cki.KeyChar.ToString();
                switch (day_select)
                {
                    case "d":
                        debug = debug == true ? false : true;
                        Console.WriteLine(debug == true ? "Debug On" : "Debug Off");
                        break;
                    case "1":
                        DayOne();
                        break;
                    case "2":
                        DayTwo(debug);
                        break;
                    case "3":
                        DayThree(debug);
                        break;
                    case "4":
                        DayFour(debug);
                        break;
                    default:
                    case "?":
                        Console.WriteLine($"Usage: ");

                        Console.WriteLine("d - Toggle Debug Mode");
                        Console.WriteLine("q - Quit");
                        Console.WriteLine("1, 2, 3... etc - Run the Solution for that day");
                        break;
                }
            }
            while (cki.Key != ConsoleKey.Q);
        }

        public static void DayFour(bool Debug)
        {
            Console.WriteLine();
            Console.WriteLine("1100101 Day Four 1100101");
            Console.WriteLine();
            var path_4 = @"C:\Users\thats\source\repos\adventofcode2020\day4input.txt";

            var passports = new List<Passport>();


            var regex_dict = Passport.PartOneRegex;
            var i = 0;
            var working_passport = new Passport(regex_dict);
            foreach(var line in File.ReadLines(path_4))
            {
                if(line == string.Empty)
                {
                    passports.Add(working_passport);
                    working_passport = new Passport(regex_dict);
                    if (Debug == true)
                    {
                    }
                }
                else
                {
                    working_passport.ParseLine(line);
                }
                i++;
            }
            passports.Add(working_passport);

            Console.WriteLine($"Part One Answer - {passports.Where(x => x.IsValidPartOne()).Count()}"); // 228
            Console.WriteLine($"Part Two Answer - {passports.Where(x => x.IsValidPartTwo()).Count()}"); // 175
        }

        public class Passport
        {
            public Passport(Dictionary<string, Regex> RegexesToUse)
            {
                Regexes = RegexesToUse;
            }

            public static Dictionary<string, Regex> PartOneRegex
            {
                get
                {
                    var result = new Dictionary<string, Regex>();
                    foreach (var prop in typeof(Passport).GetProperties().Where(x => x.PropertyType == typeof(string)))
                    {
                        result.Add(prop.Name, new Regex($"{prop.Name}[:]([^ ]*)"));
                    }
                    return result;
                }
            }

            public bool IsValidPartTwo()
            {
                var valid = byr.Length == 4  && int.TryParse(byr, out var byr_int) && 1920 <= byr_int && byr_int <= 2002;
                if (valid == true)
                {
                    valid = iyr.Length == 4 &&  int.TryParse(iyr, out var iyr_int) && 2010 <= iyr_int && iyr_int <= 2020;
                }

                if (valid == true)
                {
                    valid = eyr.Length == 4 && int.TryParse(eyr, out var eyr_int) && 2020 <= eyr_int && eyr_int <= 2030;
                }

                if (valid == true)
                {
                    if (hgt.Contains("cm"))
                    {
                        valid = int.TryParse(hgt.Substring(0, hgt.Length - 2), out var hgt_int) == true && 150 <= hgt_int && hgt_int <= 193;
                    }
                    else if (hgt.Contains("in"))
                    {
                        valid = int.TryParse(hgt.Substring(0, hgt.Length - 2), out var hgt_int) == true && 59 <= hgt_int && hgt_int <= 76;
                    }
                    else
                    {
                        valid = false;
                    }
                }

                if(valid == true)
                {
                    var hair_regex = new Regex("[#][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f][0-9a-f]");
                    valid = hcl.Length == 7 && hair_regex.Match(hcl).Success;
                }
                
                if(valid == true)
                {
                    valid = ecl.Length == 3 && ecl.Contains("amb") || ecl.Contains("blu") || ecl.Contains("brn") || ecl.Contains("gry") || ecl.Contains("grn") || ecl.Contains("hzl") || ecl.Contains("oth");
                }

                if(valid == true)
                {
                    var pid_int = 0;
                    valid = pid.Length == 9 && int.TryParse(pid, out pid_int);
                }
                
                // 175

                return valid;
            }

            public bool IsValidPartOne()
            {
                if(string.IsNullOrEmpty(byr) == false &&
                   string.IsNullOrEmpty(iyr) == false &&
                   string.IsNullOrEmpty(eyr) == false &&
                   string.IsNullOrEmpty(hgt) == false &&
                   string.IsNullOrEmpty(hcl) == false &&
                   string.IsNullOrEmpty(ecl) == false &&
                   string.IsNullOrEmpty(pid) == false 
                   )
                {
                    return true;
                }
                return false;
            }

            private Dictionary<string, Regex> Regexes
            { get; set; } = new Dictionary<string, Regex>();

            public void ParseLine(string Line)
            {
                foreach (var regex in Regexes)
                {
                    var matches = regex.Value.Match(Line);
                    if (matches.Success == true)
                    {
                        this.GetType().GetProperty(regex.Key).SetValue(this, matches.Groups[1].Value);
                    }
                }
            }

            public string byr
            { get; set; } = "";

            public string iyr
            { get; set; } = "";

            public string eyr
            { get; set; } = "";

            public string hgt
            { get; set; } = "";

            public string hcl
            { get; set; } = "";

            public string ecl
            { get; set; } = "";

            public string pid
            { get; set; } = "";

            public string cid
            { get; set; } = "";

            /*
             
    byr (Birth Year)
    iyr (Issue Year)
    eyr (Expiration Year)
    hgt (Height)
    hcl (Hair Color)
    ecl (Eye Color)
    pid (Passport ID)
    cid (Country ID)

             */
        }

        public static void DayThree(bool Debug)
        {
            Console.WriteLine();
            Console.WriteLine("-*- Day Three -*-");
            Console.WriteLine();
            var path_3 = @"C:\Users\thats\source\repos\adventofcode2020\day3input.txt";

            var matrix = File.ReadLines(path_3)
                .FileToMatrix(Debug);

            Console.WriteLine($"Part One Answer - {ArborealTraverser(matrix, 3, 1)}");

            long part_two_answer = 1;
            part_two_answer = part_two_answer * ArborealTraverser(matrix, 1, 1);
            part_two_answer = part_two_answer * ArborealTraverser(matrix, 3, 1);
            part_two_answer = part_two_answer * ArborealTraverser(matrix, 5, 1);
            part_two_answer = part_two_answer * ArborealTraverser(matrix, 7, 1);
            part_two_answer = part_two_answer * ArborealTraverser(matrix, 1, 2);

            Console.WriteLine($"Part Two Answer - {part_two_answer}");
            // Part One Answer - 292
            // Part Two Answer - 9354744432
        }

        public static int ArborealTraverser(char[,] Matrix, int RightSteps, int DownSteps)
        {
            var Height = Matrix.GetLength(0);
            var Width = Matrix.GetLength(1);
            var tree_count = 0;
            var open_count = 0;
            var col_keeper = RightSteps;
            var row_keeper = DownSteps;


            for (; row_keeper < Height;)
            {
                var this_square = Matrix[row_keeper, col_keeper];
                if (this_square == '.')
                {
                    open_count++;
                }
                else if (this_square == '#')
                {
                    tree_count++;
                }

                col_keeper = (col_keeper + RightSteps) % Width;
                row_keeper = row_keeper + DownSteps;

            }

            Console.WriteLine($"Right {RightSteps} Down {DownSteps} - Tree Count: {tree_count} \t Open Count: {open_count}");
            return tree_count;
        }

        public static void DayTwo(bool Debug)
        {
            var file = new StreamReader(@"C:\Users\thats\source\repos\adventofcode2020\day2input.txt");
            var rx = new Regex("([0-9]*)[-]([0-9]*) ([a-z])[:] ([a-z]*)");

            var fail_count = 0;
            var total_passwords = 0;
            var part_two_success = 0;

            while (file.Peek() >= 0)
            {
                total_passwords++;
                var this_line = file.ReadLine();

                var matches = rx.Matches(this_line);
                foreach(Match match in matches)
                {
                    var groups = match.Groups;

                    var min_occ = 0;
                    int.TryParse(groups[1].Value, out min_occ);
                    var first_index = min_occ;
                    var max_occ = 0;
                    int.TryParse(groups[2].Value, out max_occ);
                    var second_index = max_occ;
                    char letter = 'a';
                    char.TryParse(groups[3].Value, out letter);
                    var password = groups[4].Value;

                    Console.WriteLine($"Min {min_occ}\tMax {max_occ}\t Letter: {letter}   Password: {password}");

                    var occurrances = password.Count(x => letter == x);
                    if(occurrances < min_occ)
                    {
                        fail_count++;
                    }
                    else if(occurrances > max_occ)
                    {
                        fail_count++;
                    }

                    if(password[first_index - 1] == letter ^ password[second_index - 1] == letter)
                    {
                        part_two_success++;
                    }

                }
            }

            Console.WriteLine($"Part One - Failed Passwords: {total_passwords - fail_count}");
            Console.WriteLine($"Part Two - Valid Passwords: {part_two_success}");
        }

        public static void DayOne()
        {
            var dict1 = new HashSet<int>();
            var dict2 = new HashSet<int>();
            var dict3 = new HashSet<int>();

            var file = new StreamReader(@"C:\Users\thats\source\repos\adventofcode2020\day1input.txt");
            while (file.Peek() >= 0)
            {
                var this_int = 0;
                int.TryParse(file.ReadLine(), out this_int);
                if (this_int != 0)
                {
                    dict1.Add(this_int);
                    dict2.Add(this_int);
                    dict3.Add(this_int);
                }
            }
            var part_one_answer = new List<AnswerClass>();
            var part_two_answer = new List<AnswerClass>();
            foreach (var ii in dict1)
            {
                foreach (var jj in dict2)
                {
                    if (ii + jj == 2020)
                    {
                        part_one_answer.Add(new AnswerClass()
                        {
                            One = ii,
                            Two = jj,
                        });
                    }
                    foreach (var kk in dict3)
                    {
                        if (ii + jj + kk == 2020)
                        {
                            part_two_answer.Add(new AnswerClass()
                            {
                                One = ii,
                                Two = jj,
                                Three = kk,
                            });
                        }
                    }
                }
            }
            foreach (var ans in part_one_answer)
            {
                Console.WriteLine($"Part One Answer: {ans.One * ans.Two}");
            }
            foreach (var ans in part_two_answer)
            {
                Console.WriteLine($"Part Two Answer: {ans.One * ans.Two * ans.Three}");
            }
        }
    }

    public class AnswerClass
    {
        public int One
        { get; set; }

        public int Two
        { get; set; }

        public int Three
        { get; set; }
    }
}
