using System;
using System.Collections.Generic;
using System.Linq;


namespace adventofcode
{
    public static class Extensions
    {
        public static bool NextIsValid(this IEnumerable<Int64> Source, Int64 NextValue)
        {
            foreach(var item in Source)
            {
                foreach(var item2 in Source)
                {
                    if(item + item2 == NextValue)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

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
}
