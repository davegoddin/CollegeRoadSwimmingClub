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

            List<string> coachList = new List<string>();
            foreach (Member coach in Squad.Coaches)
            {
                coachList.Add(coach.FullName);
            }
            
            var coaches = string.Join(",", coachList);

            ViewData["CoachList"] = coaches;
            return Page();
        }
    }
}
