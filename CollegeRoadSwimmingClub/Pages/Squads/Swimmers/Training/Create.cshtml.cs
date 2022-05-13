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

namespace CollegeRoadSwimmingClub.Pages.Squads.Swimmers.Training
{
    public class CreateModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public CreateModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? swimmerId)
        {
            if (swimmerId == null)
            {
                RedirectToPage("../../Index", new { id = TempData.First(t => t.Key == "SquadId")});
            }

            TrainingResult = new TrainingResult() { SwimmerId = (int)swimmerId, Date = DateTime.Today };

            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            ViewData["SwimmerId"] = new SelectList(_context.Members.Where(m => m.IsSwimmer), "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public TrainingResult TrainingResult { get; set; }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (TrainingResult.Time <= TimeSpan.Zero || TrainingResult.Time > new TimeSpan(0, 9, 59, 59, 999))
            {
                ModelState.AddModelError("TrainingResult.Time", "Time must be more than 0 and less than 9:59:59:99");
            }

            if (TrainingResult.Date > DateTime.Today)
            {
                ModelState.AddModelError("TrainingResult.Date", "Training result cannot be in the future");
            }

            if (!ModelState.IsValid)
            {
                return OnGet(TrainingResult.SwimmerId);
            }

            _context.TrainingResults.Add(TrainingResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new { id = TrainingResult.SwimmerId });
        }
    }
}
