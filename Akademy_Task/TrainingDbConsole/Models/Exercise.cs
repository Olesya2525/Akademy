using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingDbConsoleApp.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public int ProgramId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public TrainingProgram Program { get; set; }
    }
}
