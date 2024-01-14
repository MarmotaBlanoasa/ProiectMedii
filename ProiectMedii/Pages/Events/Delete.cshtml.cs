using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ProiectMedii.Data;
using ProiectMedii.Models;

namespace ProiectMedii.Pages.Events
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly ProiectMediiContext _context;

        public DeleteModel(ProiectMediiContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Event Event { get; set; }

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include related data if necessary, e.g. Schedules or EventSponsors
            Event = await _context.Events
                .Include(e => e.Schedules)
                .Include(e => e.EventSponsors)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (Event == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load the event to include related data to avoid foreign key constraint issues
            Event = await _context.Events
                .Include(e => e.Schedules)
                .Include(e => e.EventSponsors)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (Event != null)
            {
                // If there are related entities, you may want to remove them as well
                _context.Schedule.RemoveRange(Event.Schedules);
                _context.EventSponsors.RemoveRange(Event.EventSponsors);

                _context.Events.Remove(Event);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
