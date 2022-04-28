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
using Microsoft.AspNetCore.Authorization;

namespace CollegeRoadSwimmingClub.Pages.Galas
{
    [Authorize(Roles = "Administrator")]
    public class DeleteModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public DeleteModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
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

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Gala = await _context.Galas.FindAsync(id);

            if (Gala != null)
            {
                _context.Galas.Remove(Gala);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
