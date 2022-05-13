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
using Microsoft.AspNetCore.Authorization;

namespace CollegeRoadSwimmingClub.Pages.Galas.Races.Results
{
    [Authorize(Roles="Administrator")]
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
            Race race = _context.Races.Include(r => r.Entrants).Include(r=>r.RaceResults).FirstOrDefault(r => r.Id == raceId);

            var enteredSwimmers = _context.Members
                .Include(m => m.RacesEntered).Include(m => m.RaceResults)
                .Where(m => m.RacesEntered.Contains(race) && !m.RaceResults.Where(rr => rr.RaceId == raceId).Any()).ToList();

            

            ViewData["RaceId"] = new SelectList(_context.Races.Include(r=> r.Class).Include(r=>r.Event), "Id", "Name");
            ViewData["SwimmerId"] = new SelectList(enteredSwimmers, "Id", "FullName");
            return Page();
        }

        [BindProperty]
        public RaceResult RaceResult { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (RaceResult.Time <= TimeSpan.Zero || RaceResult.Time > new TimeSpan(0, 9, 59, 59, 999))
            {
                ModelState.AddModelError("RaceResult.Time", "Time must be more than 0 and less than 9:59:59:99");
            }

            RaceResult.Race = _context.Races.Include(r => r.RaceResults).FirstOrDefault(r => r.Id == RaceResult.RaceId);

            if (RaceResult.Race.RaceResults.Where(rr => rr.Position == RaceResult.Position).Any())
            {
                ModelState.AddModelError("RaceResult.Position", "Cannot have more than one swimmer in the same position");
            }

            if (!ModelState.IsValid)
            {
                return OnGet(RaceResult.RaceId);
            }

            
            RaceResult.Swimmer = _context.Members.Find(RaceResult.SwimmerId);


            _context.RaceResults.Add(RaceResult);
            await _context.SaveChangesAsync();

            return RedirectToPage("../Details", new {id = RaceResult.RaceId});
        }
    }
}
