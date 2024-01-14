namespace ProiectMedii.Models
{
    public class EventSponsor
    {
        public long SponsorId { get; set; }
        public long EventId { get; set; }

        // Navigation properties
        public Sponsor? Sponsor { get; set; }
        public Event? Event { get; set; }
    }

}
