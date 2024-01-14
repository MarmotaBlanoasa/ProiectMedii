using Microsoft.AspNetCore.Identity;

namespace ProiectMedii.Models
{
    public class Ticket
    {
        public long TicketId { get; set; }
        public string TicketType { get; set; }
        public decimal Price { get; set; }
        public long EventId { get; set; }
        public string UserId { get; set; }

        // Navigation properties
        public Event? Event { get; set; }
    }

}
