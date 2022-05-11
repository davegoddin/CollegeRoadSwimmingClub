#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CollegeRoadSwimmingClub.Pages.Squads
{
    [Authorize(Roles = "Coach")]
    public class EditModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public EditModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
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

            Squad = await _context.Squads.Include(s => s.Members).Include(s=> s.MemberSquad).FirstOrDefaultAsync(m => m.Id == id);

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

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Squad).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SquadExists(Squad.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool SquadExists(int id)
        {
            return _context.Squads.Any(e => e.Id == id);
        }
    }
}
