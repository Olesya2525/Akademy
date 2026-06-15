using Akademy_Task.Task3;
using Akademy_Task.Task4;
using System.Drawing;

namespace Akademy_Task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Задание2.
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

            // Задание 3.
            /*try
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
            */
            // Задание 4.
            try
            {
                // Базовые операции
                SmartStack<int> stack = new SmartStack<int>();
                stack.Push(10);
                stack.Push(20);
                stack.Push(30);

                Console.WriteLine($"Peek: {stack.Peek()}");      
                Console.WriteLine($"Pop: {stack.Pop()}");        
                Console.WriteLine($"После Pop: Count = {stack.Count}");

                // PushRange
                stack.PushRange(new[] { 40, 50, 60 });
                Console.WriteLine($"PushRange: Count = {stack.Count}, Capacity = {stack.Capacity}");

                Console.WriteLine($"Contains 20: {stack.Contains(20)}");
                Console.WriteLine($"Вершина: {stack[0]}, глубина 1: {stack[1]}");

                Console.Write("Стек (сверху вниз): ");
                foreach (var item in stack) Console.Write($"{item} ");
                Console.WriteLine();

                SmartStack<char> stack2 = new SmartStack<char>(new[] { 'a', 'b', 'c' });
                Console.Write("Из коллекции: ");
                foreach (var item in stack2) Console.Write($"{item} ");
                Console.WriteLine();

                SmartStack<int> empty = new SmartStack<int>();
                empty.Pop(); // вызовет исключение
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"\nОшибка (ожидаемо): {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}
