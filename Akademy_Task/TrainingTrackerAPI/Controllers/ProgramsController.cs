using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingTrackerAPI.Data;
using TrainingTrackerAPI.Models;

namespace TrainingTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProgramsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Programs/byuser/{userId}
        [HttpGet("byuser/{userId}")]
        public async Task<ActionResult<IEnumerable<TrainingProgram>>> GetProgramsByUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            var programs = await _context.TrainingPrograms
                .Where(p => p.UserId == userId)
                .Include(p => p.Exercises)
                .ToListAsync();

            return Ok(programs);
        }

        // GET: api/Programs 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainingProgram>>> GetPrograms()
        {
            return await _context.TrainingPrograms
                .Include(p => p.Exercises)
                .ToListAsync();
        }

        // GET: api/Programs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainingProgram>> GetProgram(int id)
        {
            var program = await _context.TrainingPrograms
                .Include(p => p.Exercises)
                .FirstOrDefaultAsync(p => p.ProgramId == id);

            if (program == null) return NotFound();
            return program;
        }

        // POST: api/Programs 
        [HttpPost]
        public async Task<ActionResult<TrainingProgram>> PostProgram(TrainingProgram program)
        {
            var user = await _context.Users.FindAsync(program.UserId);
            if (user == null)
                return BadRequest("Пользователь не найден");

            program.CreatedDate = DateTime.Now;
            _context.TrainingPrograms.Add(program);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProgram), new { id = program.ProgramId }, program);
        }

        // PUT: api/Programs/5 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProgram(int id, TrainingProgram program)
        {
            if (id != program.ProgramId)
                return BadRequest();

            var existing = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.ProgramId == id && p.UserId == program.UserId);

            if (existing == null)
                return NotFound("Программа не найдена или не принадлежит вам");

            existing.Name = program.Name;
            existing.Type = program.Type;
            existing.IsActive = program.IsActive;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Programs/5 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(int id, [FromQuery] Guid userId)
        {
            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.ProgramId == id && p.UserId == userId);

            if (program == null)
                return NotFound("Программа не найдена или не принадлежит вам");

            _context.TrainingPrograms.Remove(program);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}