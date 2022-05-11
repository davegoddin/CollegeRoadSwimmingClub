using CollegeRoadSwimmingClub.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Galas.Races.Entrants
{
    public class RemoveModel : PageModel
    {
        private readonly CRSCContext _context;
        public RemoveModel(CRSCContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(int? raceId, int? memberId)
        {
            if (raceId == null || memberId == null)
            {
               return NotFound();
            }


            var race = _context.Races.Include(r => r.Entrants).FirstOrDefault(r => r.Id == raceId);
            var entrant = race.Entrants.FirstOrDefault(e => e.Id == memberId);

            if (race == null || entrant == null)
            {
                return NotFound();
            }

            race.Entrants.Remove(entrant);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = raceId });


        }
    }
}
