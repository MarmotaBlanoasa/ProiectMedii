using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ProiectMedii.Data;
using ProiectMedii.Models;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace ProiectMedii.Pages.Events
{
    public class CreateModel : PageModel
    {
        private readonly ProiectMediiContext _context;
        private readonly ILogger<CreateModel> _logger;

        public CreateModel(ProiectMediiContext context, ILogger<CreateModel> logger)
        {
            _context = context;
            _logger = logger;
        }
        public List<Sponsor> AllSponsors { get; set; }
        [BindProperty]
        public List<long> SelectedSponsorIds { get; set; }
        public int SpeakerId { get; set; } // Assuming the speaker ID is an integer
        public IActionResult OnGet()
        {
            // Populate the list of speakers for the dropdown
            ViewData["SpeakerId"] = new SelectList(_context.Speaker, "SpeakerId", "Name");
            AllSponsors = _context.Sponsors.ToList();
            SelectedSponsorIds = new List<long>();
            return Page();
        }

        [BindProperty]
        public Event Event { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                _logger.LogWarning("Model state is invalid. Errors: {Errors}", string.Join(", ", errors));
                AllSponsors = _context.Sponsors.ToList();
                ViewData["SpeakerId"] = new SelectList(_context.Speaker, "SpeakerId", "Name", Event.Schedules.Select(s => s.SpeakerId));
                return Page();
            }

            _context.Events.Add(Event);
            await _context.SaveChangesAsync();
            foreach (var sponsorId in SelectedSponsorIds)
            {
                _context.EventSponsors.Add(new EventSponsor { EventId = Event.EventId, SponsorId = sponsorId });
            }
            try { 
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error saving event sponsors");
            }
            return RedirectToPage("./Index");
        }
    }
}
