namespace ProiectMedii.Models
{
    public class Sponsor
    {
        public long SponsorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        // Navigation properties
        public ICollection<EventSponsor>? EventSponsors { get; set; }
    }

}
