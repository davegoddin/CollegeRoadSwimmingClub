#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using System.Security.Claims;

namespace CollegeRoadSwimmingClub.Pages.Squads
{
    public class DetailsModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public DetailsModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public Squad Squad { get; set; }
        public User User { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Squad = await _context.Squads.Include(s => s.Members).Include(s => s.MemberSquad).FirstOrDefaultAsync(m => m.Id == id);

            if (Squad == null)
            {
                return NotFound();
            }

            if (HttpContext.User.Identity.IsAuthenticated)
            {

                var userId = Int32.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                User = _context.Users.Include(u => u.LinkedMembers).First(u => u.Id == userId);
            }

            return Page();
        }
    }
}
