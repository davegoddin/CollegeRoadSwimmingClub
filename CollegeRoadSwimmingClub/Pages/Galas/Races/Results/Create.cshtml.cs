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

namespace CollegeRoadSwimmingClub.Pages.Galas.Races.Results
{
    public class CreateModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public CreateModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet(int raceId)
        {

            RaceResult = new RaceResult() { RaceId = raceId};
            Race race = _context.Races.Find(raceId);

            ViewData["RaceId"] = new SelectList(_context.Races.Include(r=> r.Class).Include(r=>r.Event), "Id", "Name");
            ViewData["SwimmerId"] = new SelectList(_context.Members.Include(m => m.RacesEntered).Where(m => m.IsSwimmer && m.RacesEntered.Contains(race)), "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public RaceResult RaceResult { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return RedirectToPage();
            }

            RaceResult.Race = _context.Races.Find(RaceResult.RaceId);
            RaceResult.Swimmer = _context.Members.Find(RaceResult.SwimmerId);


            _context.RaceResults.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new {id = RaceResult.RaceId});
        }
    }
}
