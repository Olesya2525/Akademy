using TrainingDbConsoleApp.Models;
using TrainingDbConsoleApp.Services;

namespace TrainingDbConsoleApp
{
    internal class Program
    {
        private static DatabaseService _dbService;

        static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.Title = "Training DB Console App";

            _dbService = new DatabaseService();

            Console.WriteLine("  Утилита для работы с БД TrainingDB");
            Console.WriteLine();

            bool exit = false;
            while (!exit)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await ShowAllUsersAdoNet();
                        break;
                    case "2":
                        await ShowAllUsersEf();
                        break;
                    case "3":
                        await CreateUserAdoNet();
                        break;
                    case "4":
                        await CreateUserEf();
                        break;
                    case "5":
                        await DeleteUserAdoNet();
                        break;
                    case "6":
                        await DeleteUserEf();
                        break;
                    case "7":
                        await GetUserByIdAdoNet();
                        break;
                    case "8":
                        await GetUserByIdEf();
                        break;
                    case "0":
                        exit = true;
                        Console.WriteLine("До свидания!");
                        break;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте снова.");
                        break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("  1. Показать всех пользователей (ADO.NET)");
            Console.WriteLine("  2. Показать всех пользователей (EF)");
            Console.WriteLine("  3. Добавить пользователя (ADO.NET)");
            Console.WriteLine("  4. Добавить пользователя (EF)");
            Console.WriteLine("  5. Удалить пользователя (ADO.NET)");
            Console.WriteLine("  6. Удалить пользователя (EF)");
            Console.WriteLine("  7. Найти пользователя по ID (ADO.NET)");
            Console.WriteLine("  8. Найти пользователя по ID (EF)");
            Console.WriteLine("  0. Выход");
            Console.Write("> ");
        }

        // ADO.NET Методы.
        static async Task ShowAllUsersAdoNet()
        {
            Console.WriteLine("\nВсе пользователи (ADO.NET)");
            var users = _dbService.GetAllUsersAdoNet();

            if (users.Count == 0)
            {
                Console.WriteLine("Пользователей нет.");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.UserId}");
                Console.WriteLine($"  Имя: {user.FullName}");
                Console.WriteLine($"  Логин: {user.Username}");
                Console.WriteLine($"  Email: {user.Email}");
                Console.WriteLine($"  Активен: {(user.IsActive ? "Да" : "Нет")}");
                Console.WriteLine();
            }
        }

        static async Task GetUserByIdAdoNet()
        {
            Console.Write("Введите ID пользователя (GUID): ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out var id))
            {
                Console.WriteLine("Неверный формат GUID.");
                return;
            }

            var user = _dbService.GetUserByIdAdoNet(id);
            if (user == null)
            {
                Console.WriteLine("Пользователь не найден.");
                return;
            }

            Console.WriteLine($"Найден: {user.FullName} ({user.Username})");
        }

        static async Task CreateUserAdoNet()
        {
            var user = CreateUserFromInput();
            if (user == null) return;

            var id = _dbService.CreateUserAdoNet(user);
            Console.WriteLine($"Пользователь создан с ID: {id}");
        }

        static async Task DeleteUserAdoNet()
        {
            Console.Write("Введите ID пользователя для удаления (GUID): ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out var id))
            {
                Console.WriteLine("Неверный формат GUID.");
                return;
            }

            if (_dbService.DeleteUserAdoNet(id))
                Console.WriteLine("Пользователь удален.");
            else
                Console.WriteLine("Пользователь не найден.");
        }

        // EF Методы
        static async Task ShowAllUsersEf()
        {
            Console.WriteLine("\nВсе пользователи (EF)");
            var users = await _dbService.GetAllUsersEfAsync();

            if (users.Count == 0)
            {
                Console.WriteLine("Пользователей нет.");
                return;
            }

            foreach (var user in users)
            {
                Console.WriteLine($"ID: {user.UserId}");
                Console.WriteLine($"  Имя: {user.FullName}");
                Console.WriteLine($"  Логин: {user.Username}");
                Console.WriteLine($"  Email: {user.Email}");
                Console.WriteLine($"  Активен: {(user.IsActive ? "Да" : "Нет")}");
                Console.WriteLine();
            }
        }

        static async Task GetUserByIdEf()
        {
            Console.Write("Введите ID пользователя (GUID): ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out var id))
            {
                Console.WriteLine("Неверный формат GUID.");
                return;
            }

            var user = await _dbService.GetUserByIdEfAsync(id);
            if (user == null)
            {
                Console.WriteLine("Пользователь не найден.");
                return;
            }

            Console.WriteLine($"Найден: {user.FullName} ({user.Username})");
        }

        static async Task CreateUserEf()
        {
            var user = CreateUserFromInput();
            if (user == null) return;

            var id = await _dbService.CreateUserEfAsync(user);
            Console.WriteLine($"Пользователь создан с ID: {id}");
        }

        static async Task DeleteUserEf()
        {
            Console.Write("Введите ID пользователя для удаления (GUID): ");
            var input = Console.ReadLine();

            if (!Guid.TryParse(input, out var id))
            {
                Console.WriteLine("Неверный формат GUID.");
                return;
            }

            if (await _dbService.DeleteUserEfAsync(id))
                Console.WriteLine("Пользователь удален.");
            else
                Console.WriteLine("Пользователь не найден.");
        }

        static User CreateUserFromInput()
        {
            try
            {
                Console.Write("Введите логин (Username): ");
                var username = Console.ReadLine();
                if (string.IsNullOrEmpty(username))
                {
                    Console.WriteLine("Логин не может быть пустым.");
                    return null;
                }

                Console.Write("Введите Email: ");
                var email = Console.ReadLine();
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("Email не может быть пустым.");
                    return null;
                }

                Console.Write("Введите полное имя: ");
                var fullName = Console.ReadLine();
                if (string.IsNullOrEmpty(fullName))
                {
                    Console.WriteLine("Имя не может быть пустым.");
                    return null;
                }

                return new User
                {
                    Username = username,
                    Email = email,
                    FullName = fullName,
                    IsActive = true,
                    CreatedDate = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
                return null;
            }
        }
    }
}