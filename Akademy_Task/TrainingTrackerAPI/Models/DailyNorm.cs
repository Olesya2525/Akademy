namespace TrainingTrackerAPI.Models
{
    /// <summary>
    /// Дневная норма активности пользователя
    /// </summary>
    public class DailyNorm
    {
        public Guid NormId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public DateTime NormDate { get; set; }
        public int PlannedMinutes { get; set; } = 30;
        public int ActualMinutes { get; set; } = 0;

        public User? User { get; set; }
    }
}
