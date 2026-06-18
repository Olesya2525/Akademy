using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingDbConsoleApp.Models
{
    public class TrainingProgram
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<Exercise> Exercises { get; set; } = new();
    }
}
