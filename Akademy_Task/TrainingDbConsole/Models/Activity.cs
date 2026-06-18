using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingDbConsoleApp.Models
{
    public class Activity
    {
        public Guid ActivityId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime ActivityDate { get; set; }
        public int DurationMinutes { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
