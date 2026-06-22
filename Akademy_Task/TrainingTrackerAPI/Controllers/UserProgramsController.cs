using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingTrackerAPI.Data;
using TrainingTrackerAPI.Models;

namespace TrainingTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProgramsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserPrograms
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserProgram>>> GetUserPrograms()
        {
            return await _context.UserPrograms
                .Include(up => up.User)
                .Include(up => up.Program)
                .ToListAsync();
        }

        // GET: api/UserPrograms/byuser/5
        [HttpGet("byuser/{userId}")]
        public async Task<ActionResult<IEnumerable<UserProgram>>> GetUserProgramsByUser(Guid userId)
        {
            return await _context.UserPrograms
                .Where(up => up.UserId == userId)
                .Include(up => up.Program)
                .ToListAsync();
        }

        // POST: api/UserPrograms
        [HttpPost]
        public async Task<ActionResult<UserProgram>> PostUserProgram(UserProgram userProgram)
        {
            var user = await _context.Users.FindAsync(userProgram.UserId);
            if (user == null) return BadRequest("Пользователь не найден");

            var program = await _context.TrainingPrograms.FindAsync(userProgram.ProgramId);
            if (program == null) return BadRequest("Программа не найдена");

            var exists = await _context.UserPrograms
                .AnyAsync(up => up.UserId == userProgram.UserId && up.ProgramId == userProgram.ProgramId);

            if (exists) return BadRequest("Эта программа уже назначена пользователю");

            userProgram.AssignedDate = DateTime.Now;
            _context.UserPrograms.Add(userProgram);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserProgramsByUser), new { userId = userProgram.UserId }, userProgram);
        }

        // DELETE: api/UserPrograms
        [HttpDelete]
        public async Task<IActionResult> DeleteUserProgram([FromQuery] Guid userId, [FromQuery] int programId)
        {
            var userProgram = await _context.UserPrograms
                .FirstOrDefaultAsync(up => up.UserId == userId && up.ProgramId == programId);

            if (userProgram == null) return NotFound();

            _context.UserPrograms.Remove(userProgram);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}