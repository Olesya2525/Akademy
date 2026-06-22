using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainingTrackerAPI.Models
{
    /// <summary>
    /// Программа тренировок.
    /// </summary>
    public class TrainingProgram
    {
        public int ProgramId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        public Guid UserId { get; set; }

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public List<Exercise> Exercises { get; set; } = new();

        [JsonIgnore]
        public List<UserProgram> UserPrograms { get; set; } = new();
    }
}
