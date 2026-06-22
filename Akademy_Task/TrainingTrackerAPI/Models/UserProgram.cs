using System;
using System.Text.Json.Serialization;

namespace TrainingTrackerAPI.Models
{
    /// <summary>
    /// Связь пользователя с программой тренировок
    /// </summary>
    public class UserProgram
    {
        public Guid UserId { get; set; }
        public int ProgramId { get; set; }
        public DateTime AssignedDate { get; set; } = DateTime.Now;

        [JsonIgnore]
        public User? User { get; set; }

        [JsonIgnore]
        public TrainingProgram? Program { get; set; }
    }
}
