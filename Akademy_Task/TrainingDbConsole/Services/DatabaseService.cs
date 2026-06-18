using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using TrainingDbConsoleApp.Config;
using TrainingDbConsoleApp.Data;
using TrainingDbConsoleApp.Models;
using TrainingDbConsoleApp.Repositories;

namespace TrainingDbConsoleApp.Services
{
    public class DatabaseService
    {
        private readonly UserRepositoryAdoNet _adoNetRepo;
        private readonly UserRepositoryEf _efRepo;

        public DatabaseService()
        {
            // ADO.NET репозиторий.
            _adoNetRepo = new UserRepositoryAdoNet();

            // EF репозиторий.
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(ConfigHelper.GetConnectionString());
            var context = new ApplicationDbContext(optionsBuilder.Options);
            _efRepo = new UserRepositoryEf(context);
        }

        // ADO.NET методы.
        public Guid CreateUserAdoNet(User user) => _adoNetRepo.Create(user);
        public List<User> GetAllUsersAdoNet() => _adoNetRepo.GetAll();
        public User GetUserByIdAdoNet(Guid id) => _adoNetRepo.GetById(id);
        public bool UpdateUserAdoNet(User user) => _adoNetRepo.Update(user);
        public bool DeleteUserAdoNet(Guid id) => _adoNetRepo.Delete(id);

        // EF методы.
        public async Task<Guid> CreateUserEfAsync(User user) => await _efRepo.CreateAsync(user);
        public async Task<List<User>> GetAllUsersEfAsync() => await _efRepo.GetAllAsync();
        public async Task<User> GetUserByIdEfAsync(Guid id) => await _efRepo.GetByIdAsync(id);
        public async Task<bool> UpdateUserEfAsync(User user) => await _efRepo.UpdateAsync(user);
        public async Task<bool> DeleteUserEfAsync(Guid id) => await _efRepo.DeleteAsync(id);
    }
}
