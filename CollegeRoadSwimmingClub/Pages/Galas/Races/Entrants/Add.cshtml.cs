using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Galas.Races.Entrants
{
    public class AddModel : PageModel
    {
        private readonly CRSCContext _context;
        public AddModel(CRSCContext context)
        {
            _context = context;
        }
        
        [BindProperty]
        public Race Race { get; set; }
        [BindProperty]
        public Member Entrant { get; set; }

        public async Task<IActionResult> OnGetAsync(int raceId)
        {
            Race = await _context.Races.FindAsync(raceId);

            ViewData["EntrantId"] = new SelectList(_context.Members.Where(m => m.IsSwimmer), "Id", "FullName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var race = await _context.Races.Include(r => r.Entrants).Include(r=> r.Class).FirstOrDefaultAsync(r => r.Id == Race.Id);
            if (race == null)
            {
                return Page();
            }

            var entrant = _context.Members.Find(Entrant.Id);

            if (!race.Class.IsEligible(entrant))
            {
                ModelState.AddModelError("Entrant.Id", "Swimmer is not eligible for this class");
                return await OnGetAsync(race.Id);
            }
            
            if (race.Entrants == null)
            {
                race.Entrants = new List<Member>();
            }
            

            race.Entrants.Add(entrant);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = Race.Id });



        }
    }
}
