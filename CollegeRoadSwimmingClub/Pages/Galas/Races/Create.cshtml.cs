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

namespace CollegeRoadSwimmingClub.Pages.Galas.Races
{
    public class CreateModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public CreateModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int? galaId)
        {
            Race = new Race();
            if (galaId != null)
            {
                var galaReferrer = _context.Galas.Find(galaId);
                if (galaReferrer != null)
                {        
                    Race.Gala = galaReferrer;
                    Race.GalaId = galaReferrer.Id;
                    Race.DateTime = galaReferrer.StartDate;
                }
            }
            else
            {
                Race.DateTime = DateTime.Today;
            }

            ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name");
            ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
            ViewData["GalaId"] = new SelectList(_context.Galas, "Id", "Name");

            
            
            return Page();
        }

        [BindProperty]
        public Race Race { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Races.Add(Race);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new {id = Race.GalaId });
        }
    }
}
