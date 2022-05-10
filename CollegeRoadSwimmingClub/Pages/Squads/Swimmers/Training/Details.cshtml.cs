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

namespace CollegeRoadSwimmingClub.Pages.Squads.Swimmers.Training
{
    public class DetailsModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public DetailsModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public TrainingResult TrainingResult { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TrainingResult = await _context.TrainingResults
                .Include(t => t.Event)
                .Include(t => t.Swimmer).FirstOrDefaultAsync(m => m.Id == id);

            if (TrainingResult == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
