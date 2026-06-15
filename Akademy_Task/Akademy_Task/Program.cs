using Akademy_Task.Task3;
using System.Drawing;

namespace Akademy_Task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*Console.Write("Введите размер диагонали ");

            try
            {
                int n = int.Parse(Console.ReadLine());
                Task2_DiamondPrinter.PrintDiamond(n);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            catch (FormatException)
            {
                Console.WriteLine("Ошибка: Введите целое число.");
            }

            Console.ReadKey();*/

            try
            {
                Task3_Rectangle rect = new Task3_Rectangle(0, 0, 10, 5);
                rect.DisplayInfo();
                
                rect.Width = 7;
                rect.Height = 9;
                Console.WriteLine("После изменения:");
                rect.DisplayInfo();


                Console.WriteLine("После изменения:");
                Task3_Rectangle invalidRect = new Task3_Rectangle(0, 0, -5, 10); 
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
