using Microsoft.EntityFrameworkCore;

namespace ProiectMedii.Models
{
    public class Utilizator
    {
        public long UtilizatorId { get; set; }
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        // Navigation properties
        //public ICollection<Ticket>? Tickets { get; set; }
    }
}
