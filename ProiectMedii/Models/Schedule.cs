namespace ProiectMedii.Models
{
    public class Schedule
    {
        public long ScheduleId { get; set; }
        public long EventId { get; set; }
        public string SessionName { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Description { get; set; }
        public long SpeakerId { get; set; }

        // Navigation properties
        public Event? Event { get; set; }
        public Speaker? Speaker { get; set; }
    }

}
