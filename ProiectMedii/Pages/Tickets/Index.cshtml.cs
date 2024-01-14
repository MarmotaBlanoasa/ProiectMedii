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

namespace ProiectMedii.Pages.Tickets
{
    public class IndexModel : PageModel
    {
        private readonly ProiectMedii.Data.ProiectMediiContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ProiectMedii.Data.ProiectMediiContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        public IList<Ticket> Ticket { get;set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = _userManager.GetUserId(User);
            Ticket = await _context.Ticket
                        .Where(t => t.UserId == userId) // Filter tickets by UserId
                        .Include(t => t.Event)          // Include related Event data
                        .ToListAsync();
        }
    }
}
