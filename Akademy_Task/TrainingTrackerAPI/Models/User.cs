using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TrainingTrackerAPI.Models
{
    /// <summary>
    /// Пользователь системы.
    /// </summary>
    public class User
    {
        public Guid UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        [JsonIgnore]
        public List<UserProgram> UserPrograms { get; set; } = new();

        [JsonIgnore]
        public List<Statistics> Statistics { get; set; } = new();

        [JsonIgnore]
        public List<DailyNorm> DailyNorms { get; set; } = new();
    }
}
