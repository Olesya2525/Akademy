using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingTrackerAPI.Data;
using TrainingTrackerAPI.Models;

namespace TrainingTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivitiesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ActivitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Activities
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivities([FromQuery] Guid userId)
        {
            return await _context.Activities
                .Where(a => a.UserId == userId)
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Program)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync();
        }

        // GET: api/Activities/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Activity>> GetActivity(Guid id)
        {
            var activity = await _context.Activities
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Program)
                .FirstOrDefaultAsync(a => a.ActivityId == id);

            if (activity == null) return NotFound();
            return activity;
        }

        // GET: api/Activities/byday?date=2026-06-19
        [HttpGet("byday")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivitiesByDay( [FromQuery] DateTime date, [FromQuery] Guid userId)
        {
            
            return await _context.Activities
                .Where(a => a.ActivityDate.Date == date.Date && a.UserId == userId)
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Program)
                .OrderBy(a => a.ActivityDate)
                .ToListAsync();
        }

        // GET: api/Activities/bymonth?year=2026&month=6
        [HttpGet("bymonth")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivitiesByMonth( [FromQuery] int year, [FromQuery] int month, [FromQuery] Guid userId)
        {
            var startDate = new DateTime(year, month, 1);
            var endDate = startDate.AddMonths(1).AddDays(-1);

            return await _context.Activities
                .Where(a => a.ActivityDate >= startDate && a.ActivityDate <= endDate && a.UserId == userId)
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Program)
                .OrderBy(a => a.ActivityDate)
                .ToListAsync();
        }

        // GET: api/Activities/daily-report?date=2026-06-19
        [HttpGet("daily-report")]
        public async Task<IActionResult> GetDailyReport([FromQuery] DateTime date, [FromQuery] Guid userId)
        {
            // Проверяем, что пользователь существует
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return BadRequest("Пользователь не найден");

            var activities = await _context.Activities
                .Where(a => a.ActivityDate.Date == date.Date && a.UserId == userId)
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Program)
                .ToListAsync();

            var totalMinutes = activities.Sum(a => a.DurationMinutes);

            string stickerColor;
            string stickerMessage;

            if (totalMinutes < 30)
            {
                stickerColor = "Yellow";
                stickerMessage = "Низкая активность";
            }
            else if (totalMinutes >= 30 && totalMinutes <= 90)
            {
                stickerColor = "Green";
                stickerMessage = "Активность в норме";
            }
            else
            {
                stickerColor = "Red";
                stickerMessage = "Высокая активность, возможное переутомление";
            }

            return Ok(new
            {
                Date = date,
                TotalMinutes = totalMinutes,
                Sticker = stickerColor,
                StickerMessage = stickerMessage,
                Activities = activities
            });
        }

        // POST: api/Activities
        [HttpPost]
        public async Task<ActionResult<Activity>> PostActivity(Activity activity)
        {
            var exercise = await _context.Exercises.FindAsync(activity.ExerciseId);
            if (exercise == null)
                return BadRequest("Упражнение не найдено");

            if (!exercise.IsActive)
                return BadRequest("Нельзя выбрать неактивное упражнение");

            var user = await _context.Users.FindAsync(activity.UserId);
            if (user == null)
                return BadRequest("Пользователь не найден");

            if (activity.DurationMinutes <= 0 || activity.DurationMinutes > 1440)
                return BadRequest("Длительность должна быть от 1 до 1440 минут");

            var totalMinutesForDay = await _context.Activities
                .Where(a => a.ActivityDate.Date == activity.ActivityDate.Date
                            && a.UserId == activity.UserId)
                .SumAsync(a => a.DurationMinutes);

            if (totalMinutesForDay + activity.DurationMinutes > 1440)
                return BadRequest("Суммарная длительность активностей за день не может превышать 1440 минут");

            activity.ActivityId = Guid.NewGuid();
            activity.CreatedDate = DateTime.Now;

            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();

            await UpdateStatistics(activity.UserId, activity.ExerciseId, activity.DurationMinutes, activity.ActivityDate);

            await UpdateDailyNorm(activity.UserId, activity.ActivityDate, activity.DurationMinutes);

            await _context.Entry(activity)
                .Reference(a => a.Exercise)
                .LoadAsync();

            return CreatedAtAction(nameof(GetActivity), new { id = activity.ActivityId }, activity);
        }

        // PUT: api/Activities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActivity(Guid id, Activity activity)
        {
            if (id != activity.ActivityId) return BadRequest();

            var existingActivity = await _context.Activities.FindAsync(id);
            if (existingActivity == null) return NotFound();

            if (existingActivity.ExerciseId != activity.ExerciseId)
            {
                var newExercise = await _context.Exercises.FindAsync(activity.ExerciseId);
                if (newExercise == null)
                    return BadRequest("Упражнение не найдено");

                if (!newExercise.IsActive)
                    return BadRequest("Нельзя выбрать неактивное упражнение");

                var oldExercise = await _context.Exercises.FindAsync(existingActivity.ExerciseId);
                if (!oldExercise.IsActive)
                    return BadRequest("Нельзя изменить упражнение, так как оно стало неактивным");
            }

            if (activity.DurationMinutes <= 0 || activity.DurationMinutes > 1440)
                return BadRequest("Длительность должна быть от 1 до 1440 минут");

            existingActivity.ExerciseId = activity.ExerciseId;
            existingActivity.ActivityDate = activity.ActivityDate;
            existingActivity.DurationMinutes = activity.DurationMinutes;
            existingActivity.Note = activity.Note;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActivity(Guid id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity == null) return NotFound();

            _context.Activities.Remove(activity);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        private async Task UpdateStatistics(Guid userId, int exerciseId, int durationMinutes, DateTime activityDate)
        {
            var statistics = await _context.Statistics
                .FirstOrDefaultAsync(s => s.UserId == userId && s.ExerciseId == exerciseId);

            if (statistics == null)
            {
                statistics = new Statistics
                {
                    UserId = userId,
                    ExerciseId = exerciseId,
                    TotalMinutes = durationMinutes,
                    LastActivityDate = activityDate
                };
                _context.Statistics.Add(statistics);
            }
            else
            {
                statistics.TotalMinutes += durationMinutes;
                statistics.LastActivityDate = activityDate;
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateDailyNorm(Guid userId, DateTime activityDate, int durationMinutes)
        {
            var dailyNorm = await _context.DailyNorms
                .FirstOrDefaultAsync(d => d.UserId == userId && d.NormDate.Date == activityDate.Date);

            if (dailyNorm == null)
            {
                dailyNorm = new DailyNorm
                {
                    UserId = userId,
                    NormDate = activityDate.Date,
                    PlannedMinutes = 30,
                    ActualMinutes = durationMinutes
                };
                _context.DailyNorms.Add(dailyNorm);
            }
            else
            {
                dailyNorm.ActualMinutes += durationMinutes;
            }

            await _context.SaveChangesAsync();
        }

        // GET: api/Activities/byuser/{userId} 
        [HttpGet("byuser/{userId}")]
        public async Task<ActionResult<IEnumerable<Activity>>> GetActivitiesByUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            return await _context.Activities
                .Where(a => a.UserId == userId)
                .Include(a => a.Exercise)
                .ThenInclude(e => e.Program)
                .OrderByDescending(a => a.ActivityDate)
                .ToListAsync();
        }
    }
}
