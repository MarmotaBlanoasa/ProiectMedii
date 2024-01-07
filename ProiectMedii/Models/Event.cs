using System.Net.Sockets;

namespace ProiectMedii.Models
{
    public class Event
    {
        public long EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }
        public string Location { get; set; }
        public int MaxTickets { get; set; }
        public decimal Price { get; set; }

        // Navigation properties
        public ICollection<Ticket>? Tickets { get; set; }
        public ICollection<EventSponsor>? EventSponsors { get; set; }
        public virtual List<Schedule> Schedules { get; set; } = new List<Schedule>();
    }
}
