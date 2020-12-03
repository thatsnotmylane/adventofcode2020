using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Program
    {
        static void Main(string[] args)
        {
            // C:\Users\thats\source\repos\adventofcode
            Console.WriteLine("Hello World!");
            DayThree();

            var looper = true;
            while (looper)
            {
                var day_select = Console.ReadLine();

                switch (day_select)
                {
                    case "q":
                        looper = false;
                        break;
                    case "1":
                        DayOne();
                        break;
                    case "2":
                        DayTwo();
                        break;
                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }
            }
        }

        public static void DayThree()
        {
            var file = new StreamReader(@"C:\Users\thats\source\repos\adventofcode2020\day3input.txt");
            var tree_land = new List<List<char>>();

            while(file.Peek() >= 0)
            {
                tree_land.Add(file.ReadLine().ToCharArray().ToList());
            }

            //foreach (var line in tree_land)
            //{
            //    Console.WriteLine(new string(line.ToArray()));
            //}

            Console.WriteLine(" ");
            Console.WriteLine("-------------------------------");
            Console.WriteLine(" ");

            var width = tree_land.First().Count;
            var height = tree_land.Count;
            var tree_matrix = new char[height, width];
            var row_iter = 0;

            foreach (var row in tree_land)
            {
                var col_itter = 0;
                foreach (var letter in row)
                {
                    tree_matrix[row_iter, col_itter] = letter;
                    col_itter++;
                }
                row_iter++;
            }


            //for (int i = 0; i < tree_matrix.GetLength(0); i++)
            //{
            //    for (int j = 0; j < tree_matrix.GetLength(1); j++)
            //    {
            //        Console.Write(tree_matrix[i, j]);
            //    }
            //    Console.WriteLine();
            //}

            long part_two_answer = 1;
            part_two_answer = part_two_answer * ArborealTraverser(tree_matrix, 1, 1, height, width);
            part_two_answer = part_two_answer * ArborealTraverser(tree_matrix, 3, 1, height, width);
            part_two_answer = part_two_answer * ArborealTraverser(tree_matrix, 5, 1, height, width);
            part_two_answer = part_two_answer * ArborealTraverser(tree_matrix, 7, 1, height, width);
            part_two_answer = part_two_answer * ArborealTraverser(tree_matrix, 1, 2, height, width);

            Console.WriteLine($"Part Two Answer - {part_two_answer}");
        }

        public static int ArborealTraverser(char[,] Matrix, int RightSteps, int DownSteps, int Height, int Width)
        {
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
