namespace Akademy_Task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Введите размер диагонали ");

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

            Console.ReadKey();
        }
    }
}
