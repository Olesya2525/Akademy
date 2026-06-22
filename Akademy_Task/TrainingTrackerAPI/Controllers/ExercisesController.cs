using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainingTrackerAPI.Data;
using TrainingTrackerAPI.Models;

namespace TrainingTrackerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExercisesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ExercisesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Exercises/byuser/{userId} 
        [HttpGet("byuser/{userId}")]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercisesByUser(Guid userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return NotFound("Пользователь не найден");

            var exercises = await _context.Exercises
                .Join(_context.TrainingPrograms,
                      exercise => exercise.ProgramId,
                      program => program.ProgramId,
                      (exercise, program) => new { exercise, program })
                .Where(x => x.program.UserId == userId)
                .Select(x => x.exercise)
                .Include(e => e.Program)
                .ToListAsync();

            return Ok(exercises);
        }

        // GET: api/Exercises 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercises()
        {
            return await _context.Exercises
                .Include(e => e.Program)
                .ToListAsync();
        }

        // GET: api/Exercises/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Exercise>> GetExercise(int id)
        {
            var exercise = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.ExerciseId == id);

            if (exercise == null) return NotFound();
            return exercise;
        }

        // GET: api/Exercises/byprogram/5
        [HttpGet("byprogram/{programId}")]
        public async Task<ActionResult<IEnumerable<Exercise>>> GetExercisesByProgram(int programId)
        {
            return await _context.Exercises
                .Where(e => e.ProgramId == programId)
                .Include(e => e.Program)
                .ToListAsync();
        }

        // POST: api/Exercises 
        [HttpPost]
        public async Task<ActionResult<Exercise>> PostExercise(Exercise exercise, [FromQuery] Guid userId)
        {
            var program = await _context.TrainingPrograms
                .FirstOrDefaultAsync(p => p.ProgramId == exercise.ProgramId);

            if (program == null)
                return BadRequest("Программа не найдена");

            if (program.UserId != userId)
                return BadRequest("Вы не можете создавать упражнения в чужой программе");

            exercise.CreatedDate = DateTime.Now;
            _context.Exercises.Add(exercise);
            await _context.SaveChangesAsync();

            await _context.Entry(exercise)
                .Reference(e => e.Program)
                .LoadAsync();

            return CreatedAtAction(nameof(GetExercise), new { id = exercise.ExerciseId }, exercise);
        }

        // PUT: api/Exercises/5 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutExercise(int id, Exercise exercise, [FromQuery] Guid userId)
        {
            if (id != exercise.ExerciseId) return BadRequest();

            var existing = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.ExerciseId == id);

            if (existing == null) return NotFound();

            if (existing.Program.UserId != userId)
                return BadRequest("Вы не можете изменять упражнения в чужой программе");

            if (existing.ProgramId != exercise.ProgramId)
            {
                var newProgram = await _context.TrainingPrograms
                    .FirstOrDefaultAsync(p => p.ProgramId == exercise.ProgramId);

                if (newProgram == null)
                    return BadRequest("Новая программа не найдена");

                if (newProgram.UserId != userId)
                    return BadRequest("Вы не можете переносить упражнение в чужую программу");
            }

            existing.Name = exercise.Name;
            existing.ProgramId = exercise.ProgramId;
            existing.IsActive = exercise.IsActive;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Exercises/5 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExercise(int id, [FromQuery] Guid userId)
        {
            var exercise = await _context.Exercises
                .Include(e => e.Program)
                .FirstOrDefaultAsync(e => e.ExerciseId == id);

            if (exercise == null) return NotFound();

            if (exercise.Program.UserId != userId)
                return BadRequest("Вы не можете удалять упражнения в чужой программе");

            _context.Exercises.Remove(exercise);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}