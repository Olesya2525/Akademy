using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainingTrackerAPI.Models
{
    /// <summary>
    /// Выполненная активность.
    /// </summary>
    public class Activity
    {
        public Guid ActivityId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime ActivityDate { get; set; }
        public int DurationMinutes { get; set; }
        public string Note { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid UserId { get; set; }


        [JsonIgnore]
        public Exercise? Exercise { get; set; }
        [JsonIgnore]
        public User? User { get; set; }
    }
}
