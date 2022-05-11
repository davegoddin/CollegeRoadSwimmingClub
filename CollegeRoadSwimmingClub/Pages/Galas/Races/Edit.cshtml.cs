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

namespace CollegeRoadSwimmingClub.Pages.Galas.Races
{
    public class EditModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public EditModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Race Race { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Race = await _context.Races
                .Include(r => r.Class)
                .Include(r => r.Event)
                .Include(r => r.Gala).FirstOrDefaultAsync(m => m.Id == id);

            if (Race == null)
            {
                return NotFound();
            }
           ViewData["ClassId"] = new SelectList(_context.Classes, "Id", "Name");
           ViewData["EventId"] = new SelectList(_context.Events, "Id", "Name");
           ViewData["GalaId"] = new SelectList(_context.Galas, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {

            var raceToUpdate = await _context.Races.FirstOrDefaultAsync(r => r.Id == Race.Id);


            if (raceToUpdate == null) { return Page(); }

            if (await TryUpdateModelAsync<Race>(raceToUpdate, "Race", r => r.EventId, r => r.ClassId, r => r.GalaId, r => r.DateTime))
            {
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RaceExists(Race.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToPage("../Details", new { id = raceToUpdate.GalaId });
            }

       
            

            return Page();


            
        }

        private bool RaceExists(int id)
        {
            return _context.Races.Any(e => e.Id == id);
        }
    }
}
