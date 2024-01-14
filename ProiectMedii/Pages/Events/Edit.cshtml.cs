using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProiectMedii.Data;
using ProiectMedii.Models;

namespace ProiectMedii.Pages.Events
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly ProiectMedii.Data.ProiectMediiContext _context;

        public EditModel(ProiectMedii.Data.ProiectMediiContext context)
        {
            _context = context;
        }
        public List<Sponsor> AllSponsors { get; set; }
        [BindProperty]
        public List<long> SelectedSponsorIds { get; set; }
        [BindProperty]
        public Event Event { get; set; } = default!;
        [BindProperty]
        public List<Schedule> Schedules { get; set; }
        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Include the related schedules when loading the event
            Event = await _context.Events
                .Include(e => e.Schedules)
                    .ThenInclude(s => s.Speaker)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (Event == null)
            {
                return NotFound();
            }
            SelectedSponsorIds = _context.EventSponsors.Select(es => es.SponsorId).ToList();
            ViewData["SpeakerId"] = new SelectList(_context.Speaker, "SpeakerId", "Name");
            AllSponsors = _context.Sponsors.ToList();
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (!ModelState.IsValid)
            {
                SelectedSponsorIds = await _context.EventSponsors
            .Where(es => es.EventId == id)
            .Select(es => es.SponsorId)
            .ToListAsync();
                // Reload the data needed for the page
                AllSponsors = await _context.Sponsors.ToListAsync();
                ViewData["SpeakerId"] = new SelectList(_context.Speaker, "SpeakerId", "Name");
                return Page();
            }

            var eventToUpdate = await _context.Events
                .Include(e => e.EventSponsors)
                .FirstOrDefaultAsync(m => m.EventId == id);

            if (eventToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Event>(
                eventToUpdate,
                "Event", // Prefix for form value.
                e => e.EventName, e => e.EventDescription, e => e.DateStart, e => e.DateEnd, e => e.Location, e => e.MaxTickets, e => e.Price))
            {
                // Remove all current sponsors
                foreach (var existingSponsor in eventToUpdate.EventSponsors.ToList())
                {
                    _context.EventSponsors.Remove(existingSponsor);
                }
                await _context.SaveChangesAsync();

                // Add back the selected sponsors
                foreach (var sponsorId in SelectedSponsorIds)
                {
                    _context.EventSponsors.Add(new EventSponsor { EventId = eventToUpdate.EventId, SponsorId = sponsorId });
                }
                foreach (var scheduleToUpdate in eventToUpdate.Schedules)
                {
                    var schedule = Schedules.FirstOrDefault(s => s.ScheduleId == scheduleToUpdate.ScheduleId);
                    if (schedule != null)
                    {
                        // Update the existing schedule
                        _context.Entry(scheduleToUpdate).CurrentValues.SetValues(schedule);
                    }
                }
                await _context.SaveChangesAsync();

                return RedirectToPage("./Index");
            }
            else
            {
                // If there's a problem updating the model, reload the necessary data for reshowing the edit form
                SelectedSponsorIds = await _context.EventSponsors
                    .Where(es => es.EventId == id.Value)
                    .Select(es => es.SponsorId)
                    .ToListAsync();
                AllSponsors = await _context.Sponsors.ToListAsync();
                ViewData["SpeakerId"] = new SelectList(_context.Speaker, "SpeakerId", "Name");
                return Page();
            }
        }
    }
}
