using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;

namespace adventofcode
{
    public static class Extensions
    {
        public static char[,] FileToMatrix(this IEnumerable<string> Source, bool Debug = false)
        {
            if (Source == null || Source.Count() == 0)
            {
                return null;
            }
            var height = Source.Count();
            var width = Source.OrderByDescending(x => x.Length).Select(x => x.Length).First();
            if (Debug == true)
            {
                Console.WriteLine($"H: {height} W: {width}");
            }
            var matrix_result = new char[height, width];
            var row = 0;
            foreach (var line in Source)
            {
                var col = 0;
                foreach (var letter in line)
                {
                    matrix_result[row, col] = letter;
                    col++;
                }
                row++;
            }
            matrix_result.DebugMatrix(Debug);
            return matrix_result;
        }

        public static void DebugMatrix(this char[,] Source, bool Debug = false)
        {
            if (Debug == true)
            {
                for (int i = 0; i < Source.GetLength(0); i++)
                {
                    for (int j = 0; j < Source.GetLength(1); j++)
                    {
                        Console.Write(Source[i, j]);
                    }
                    Console.WriteLine();
                }
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello Advent of Code");
            
            var debug = false;
            DayThree(debug);

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
                        DayTwo();
                        break;
                    case "3":
                        DayThree(debug);
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

        public static void DayTwo()
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
