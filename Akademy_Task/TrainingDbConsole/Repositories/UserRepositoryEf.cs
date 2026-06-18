using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using TrainingDbConsoleApp.Data;
using TrainingDbConsoleApp.Models;

namespace TrainingDbConsoleApp.Repositories
{
    public class UserRepositoryEf
    {
        private readonly ApplicationDbContext _context;

        public UserRepositoryEf(ApplicationDbContext context)
        {
            _context = context;
        }

        // CREATE
        public async Task<Guid> CreateAsync(User user)
        {
            user.UserId = Guid.NewGuid();
            user.CreatedDate = DateTime.Now;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.UserId;
        }

        // READ - все пользователи
        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        // READ - пользователь по ID
        public async Task<User> GetByIdAsync(Guid userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        // UPDATE
        public async Task<bool> UpdateAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser == null) return false;

            existingUser.Username = user.Username;
            existingUser.Email = user.Email;
            existingUser.FullName = user.FullName;
            existingUser.IsActive = user.IsActive;

            await _context.SaveChangesAsync();
            return true;
        }

        // DELETE
        public async Task<bool> DeleteAsync(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}