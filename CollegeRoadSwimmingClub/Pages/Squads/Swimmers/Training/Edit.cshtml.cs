#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;

namespace CollegeRoadSwimmingClub.Pages.Squads.Swimmers.Training
{
    public class EditModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public EditModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
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
           ViewData["EventId"] = new SelectList(_context.Events, "Id", "Id");
           ViewData["SwimmerId"] = new SelectList(_context.Members, "Id", "Address1");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TrainingResult).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainingResultExists(TrainingResult.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TrainingResultExists(int id)
        {
            return _context.TrainingResults.Any(e => e.Id == id);
        }
    }
}
