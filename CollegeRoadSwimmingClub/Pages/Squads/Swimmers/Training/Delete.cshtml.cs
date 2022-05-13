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
    public class DeleteModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public DeleteModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            TrainingResult = await _context.TrainingResults.FindAsync(id);

            if (TrainingResult != null)
            {
                _context.TrainingResults.Remove(TrainingResult);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("../Details", new { id = TrainingResult.SwimmerId});
        }
    }
}
