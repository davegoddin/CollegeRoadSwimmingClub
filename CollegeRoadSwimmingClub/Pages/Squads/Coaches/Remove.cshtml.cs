using CollegeRoadSwimmingClub.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Squads.Coaches
{
    [Authorize(Roles = "Coach")]
    public class RemoveModel : PageModel
    {
        private readonly CRSCContext _context;
        public RemoveModel(CRSCContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnPostAsync(int? squadId, int? memberId)
        {
            if (squadId == null || memberId == null)
            {
               return NotFound();
            }

            var squad = _context.Squads.Include(s => s.MemberSquad).FirstOrDefault(squad => squad.Id == squadId);
            var memberSquad = squad.MemberSquad.FirstOrDefault(ms => ms.MemberId == memberId && ms.MemberRole == Models.MemberSquadRole.Coach );

            if (memberSquad == null || squad == null)
            {
                return NotFound();
            }
            
            squad.MemberSquad.Remove(memberSquad);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = squadId });


        }
    }
}
