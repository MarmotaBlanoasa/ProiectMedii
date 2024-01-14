using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProiectMedii.Data;
using ProiectMedii.Models;

namespace ProiectMedii.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly ProiectMediiContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DetailsModel(ProiectMediiContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load the event and include related schedules and the related speaker for each schedule.
            // Also include event sponsors through the junction table and include the sponsor details.
            Event = await _context.Events
                .Include(e => e.Schedules)
                    .ThenInclude(s => s.Speaker) // Assuming Schedule has a navigation property to Speaker
                .Include(e => e.EventSponsors)
                    .ThenInclude(es => es.Sponsor) // Assuming EventSponsor has a navigation property to Sponsor
                .AsNoTracking() // Use AsNoTracking if you're only reading data for better performance
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (Event == null)
            {
                return NotFound();
            }

            return Page();
        }
        public async Task<IActionResult> OnPostBuyTicketAsync(int eventId, int price)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToPage("/Account/Login");
            }

            var userId = _userManager.GetUserId(User);

            // Check if the user already has a ticket for this event
            var existingTicket = await _context.Ticket
                .FirstOrDefaultAsync(t => t.EventId == eventId && t.UserId == userId);
            if (existingTicket != null)
            {
                // Handle already having a ticket scenario
                return Page();
            }

            var ticket = new Ticket
            {
                EventId = eventId,
                UserId = userId,
                TicketType = "Regular",
                Price = price

            };
            _context.Ticket.Add(ticket);
            await _context.SaveChangesAsync();

            // Redirect to the user's tickets page or confirmation page
            return RedirectToPage("/Tickets/Index");
        }
    }
}
