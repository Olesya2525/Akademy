using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainingTrackerAPI.Models
{
    /// <summary>
    /// Упражнение.
    /// </summary>
    public class Exercise
    {
        public int ExerciseId { get; set; }
        public string Name { get; set; }
        public int ProgramId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public TrainingProgram? Program { get; set; }

        [JsonIgnore]
        public List<Activity> Activities { get; set; } = new();

        [JsonIgnore]
        public List<Statistics> Statistics { get; set; } = new();
    }
}
