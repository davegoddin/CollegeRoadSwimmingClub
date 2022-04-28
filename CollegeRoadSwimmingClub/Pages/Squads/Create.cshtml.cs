#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CollegeRoadSwimmingClub.Pages.Squads
{
    [Authorize(Roles ="Coach")]
    public class CreateModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public CreateModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {

            return Page();
        }

        [BindProperty]
        public Squad Squad { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Squad.Members == null)
            {
                Squad.Members = new List<Member>();
            }

            int currentUserId = Int32.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            Member coach = _context.Users.Include(u => u.LinkedMembers).First(u => u.Id == currentUserId).Self;

            


            _context.Squads.Add(Squad);
            _context.MembersSquads.Add(new MemberSquad() { Member = coach, MemberId = coach.Id, Squad = Squad, SquadId = Squad.Id, MemberRole = MemberSquadRole.Coach });
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
