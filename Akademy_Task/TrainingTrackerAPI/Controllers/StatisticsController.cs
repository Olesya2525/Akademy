using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingTrackerAPI.Data;
using TrainingTrackerAPI.Models;

namespace TrainingTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StatisticsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Statistics/byuser/5
        [HttpGet("byuser/{userId}")]
        public async Task<IActionResult> GetUserStatistics(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            var statistics = await _context.Statistics
                .Where(s => s.UserId == userId)
                .Include(s => s.Exercise)
                .ThenInclude(e => e.Program)
                .ToListAsync();

            var totalMinutes = statistics.Sum(s => s.TotalMinutes);

            return Ok(new
            {
                UserId = userId,
                UserName = user.FullName,
                TotalMinutes = totalMinutes,
                Statistics = statistics
            });
        }

        // GET: api/Statistics/top
        [HttpGet("top")]
        public async Task<IActionResult> GetTopUsers([FromQuery] int count = 5)
        {
            var topUsers = await _context.Statistics
                .GroupBy(s => s.UserId)
                .Select(g => new
                {
                    UserId = g.Key,
                    TotalMinutes = g.Sum(s => s.TotalMinutes)
                })
                .OrderByDescending(x => x.TotalMinutes)
                .Take(count)
                .ToListAsync();

            return Ok(topUsers);
        }
    }
}