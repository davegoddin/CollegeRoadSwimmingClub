using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Squads.Coaches
{
    public class AddModel : PageModel
    {
        private readonly CRSCContext _context;
        public AddModel(CRSCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Squad Squad { get; set; }
        [BindProperty]
        public Member Member { get; set; }
        

        public async Task<IActionResult> OnGetAsync(int squadId)
        {
            Squad = await _context.Squads.FindAsync(squadId);

            Role coachRole = _context.Roles.Find(2);

            var coaches = _context.Members
                .Include(m => m.User)
                .ThenInclude(u => u.Roles)
                .Where(m => m.UserMemberLink == UserMemberLink.Self)
                .Where(m => m.User.Roles.Contains(coachRole));

            ViewData["MemberId"] = new SelectList(coaches, "Id", "FullName");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Squad = _context.Squads.Include(s => s.MemberSquad).FirstOrDefault(s => s.Id == Squad.Id);
            if (Squad == null)
            {
                return Page();
            }

            var member = _context.Members.Find(Member.Id);

            if (Squad.MemberSquad == null)
            {
                Squad.MemberSquad = new List<MemberSquad>();
            }

            Squad.MemberSquad.Add(new MemberSquad() { Member = member, MemberId = member.Id, Squad = Squad, SquadId = Squad.Id, MemberRole = MemberSquadRole.Coach });
            
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = Squad.Id });



        }
    }
}
