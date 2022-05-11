using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Squads.Swimmers
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
            Squad = _context.Squads.Include(s => s.Members).Include(s => s.MemberSquad).FirstOrDefault(s => s.Id == squadId);

            var eligibleMembers = _context.Members.Where(m => m.IsSwimmer && !Squad.Swimmers.Contains(m)).ToList();

            ViewData["MemberId"] = new SelectList(eligibleMembers, "Id", "FullName");


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

            Squad.MemberSquad.Add(new MemberSquad() { Member = member, MemberId = member.Id, Squad = Squad, SquadId = Squad.Id, MemberRole = MemberSquadRole.Swimmer });
            
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = Squad.Id });



        }
    }
}
