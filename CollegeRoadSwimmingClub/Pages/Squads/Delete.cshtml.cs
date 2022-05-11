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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CollegeRoadSwimmingClub.Pages.Squads
{
    [Authorize(Roles = "Coach")]
    public class DeleteModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public DeleteModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Squad Squad { get; set; }

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
                User user;
                var userId = Int32.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                user = _context.Users.Include(u => u.LinkedMembers).First(u => u.Id == userId);
                if (!Squad.Coaches.Contains(user.Self))
                {
                    return Forbid();
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Squad = await _context.Squads.FindAsync(id);

            if (Squad != null)
            {
                _context.Squads.Remove(Squad);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
