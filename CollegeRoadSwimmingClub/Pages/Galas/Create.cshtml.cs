﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;

namespace CollegeRoadSwimmingClub.Pages.Galas
{
    public class CreateModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public CreateModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Gala Gala { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Galas.Add(Gala);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
