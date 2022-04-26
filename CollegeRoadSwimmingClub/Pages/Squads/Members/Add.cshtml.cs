using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Squads.Members
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
        [BindProperty]
        public MemberSquadRole Role { get; set; }

        public async Task<IActionResult> OnGetAsync(int squadId)
        {
            Squad = await _context.Squads.FindAsync(squadId);

            ViewData["MemberId"] = new SelectList(_context.Members.Where(m => m.IsSwimmer), "Id", "FullName");


            return Page();
        }

        public async Task<IActionResult> OnPostAsync(MemberSquadRole role)
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

            Squad.MemberSquad.Add(new MemberSquad() { Member = member, MemberId = member.Id, Squad = Squad, SquadId = Squad.Id, MemberRole = role });
            
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = Squad.Id });



        }
    }
}
