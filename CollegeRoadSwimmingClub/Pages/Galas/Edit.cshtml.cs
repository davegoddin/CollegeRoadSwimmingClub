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

namespace CollegeRoadSwimmingClub.Pages.Galas
{
    [Authorize(Roles = "Administrator")]
    public class EditModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public EditModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Gala Gala { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Gala = await _context.Galas.FirstOrDefaultAsync(m => m.Id == id);

            if (Gala == null)
            {
                return NotFound();
            }
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

            _context.Attach(Gala).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GalaExists(Gala.Id))
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

        private bool GalaExists(int id)
        {
            return _context.Galas.Any(e => e.Id == id);
        }
    }
}
