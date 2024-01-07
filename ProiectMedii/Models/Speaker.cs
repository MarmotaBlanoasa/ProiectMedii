namespace ProiectMedii.Models
{
    public class Speaker
    {
        public long SpeakerId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; } // Assuming this is a first name or similar
        public string Email { get; set; }
        public string Phone { get; set; } // Assuming this is a phone number
        public string Bio { get; set; }

        // Navigation properties
        public ICollection<Schedule>? Schedules { get; set; }
    }

}
