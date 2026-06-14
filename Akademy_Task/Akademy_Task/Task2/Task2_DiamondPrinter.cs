using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Akademy_Task
{
    internal class Task2_DiamondPrinter
    {
        /// <summary>
        /// Выводит на экран ромб из символов X с пустым центром.
        /// </summary>
        /// <param name="n">Длина диагонали ромба (положительное нечётное целое число).</param>
        public static void PrintDiamond(int n)
        {
            if (n <= 0)
            {
                throw new ArgumentException("Длина диагонали должна быть положительным числом.", nameof(n));
            }

            if (n % 2 == 0)
            {
                throw new ArgumentException("Длина диагонали должна быть нечётным числом.", nameof(n));
            }

            int center = n / 2;

            for (int row = 0; row < n; row++)
            {
                for (int col = 0; col < n; col++)
                {
                    int verticalDist = Math.Abs(row - center);
                    int horizontalDist = Math.Abs(col - center);

                    if (verticalDist + horizontalDist == center && !(verticalDist == 0 && horizontalDist == 0))
                    {
                        Console.Write('X');
                    }
                    else
                    {
                        Console.Write(' ');
                    }
                }
                Console.WriteLine();
            }
        }
    }
}

