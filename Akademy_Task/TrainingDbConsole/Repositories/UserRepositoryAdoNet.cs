using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Data.SqlClient;
using TrainingDbConsoleApp.Config;
using TrainingDbConsoleApp.Models;

namespace TrainingDbConsoleApp.Repositories
{
    public class UserRepositoryAdoNet
    {
        private readonly string _connectionString;

        public UserRepositoryAdoNet()
        {
            _connectionString = ConfigHelper.GetConnectionString();
        }

        // Добавление пользователя.
        public Guid Create(User user)
        {
            const string sql = @"
                INSERT INTO Users (UserId, Username, Email, FullName, IsActive, CreatedDate)
                VALUES (@UserId, @Username, @Email, @FullName, @IsActive, @CreatedDate)";

            user.UserId = Guid.NewGuid();
            user.CreatedDate = DateTime.Now;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@FullName", user.FullName);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);
            command.Parameters.AddWithValue("@CreatedDate", user.CreatedDate);

            connection.Open();
            command.ExecuteNonQuery();

            return user.UserId;
        }

        // Получение всех пользователей.
        public List<User> GetAll()
        {
            const string sql = "SELECT * FROM Users ORDER BY Username";
            var users = new List<User>();

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            connection.Open();
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                users.Add(new User
                {
                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                });
            }

            return users;
        }

        // Получение пользователя по ID.
        public User GetById(Guid userId)
        {
            const string sql = "SELECT * FROM Users WHERE UserId = @UserId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            using var reader = command.ExecuteReader();

            if (reader.Read())
            {
                return new User
                {
                    UserId = reader.GetGuid(reader.GetOrdinal("UserId")),
                    Username = reader.GetString(reader.GetOrdinal("Username")),
                    Email = reader.GetString(reader.GetOrdinal("Email")),
                    FullName = reader.GetString(reader.GetOrdinal("FullName")),
                    IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                    CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                };
            }

            return null;
        }

        // Обновление пользователя.
        public bool Update(User user)
        {
            const string sql = @"
                UPDATE Users 
                SET Username = @Username, Email = @Email, FullName = @FullName, 
                    IsActive = @IsActive
                WHERE UserId = @UserId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue("@UserId", user.UserId);
            command.Parameters.AddWithValue("@Username", user.Username);
            command.Parameters.AddWithValue("@Email", user.Email);
            command.Parameters.AddWithValue("@FullName", user.FullName);
            command.Parameters.AddWithValue("@IsActive", user.IsActive);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }

        // Удаление пользователя.
        public bool Delete(Guid userId)
        {
            const string sql = "DELETE FROM Users WHERE UserId = @UserId";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            return rowsAffected > 0;
        }
    }
}
