namespace TrainingTrackerAPI.Models
{
    /// <summary>
    /// Агрегированная статистика по упражнению для пользователя.
    /// </summary>
    public class Statistics
    {
        public Guid StatisticId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public int ExerciseId { get; set; }
        public int TotalMinutes { get; set; }
        public DateTime? LastActivityDate { get; set; }

        public User? User { get; set; }
        public Exercise? Exercise { get; set; }
    }
}
