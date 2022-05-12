using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CollegeRoadSwimmingClub.Pages.Admin.Classes
{
    public class CreateModel : PageModel
    {
        private readonly CRSCContext _context;

        public CreateModel(CRSCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        { 
            return Page();
        }

        [BindProperty]
        public Class Class { get; set; }
        [BindProperty]
        public bool MinAgeNull { get; set; }
        [BindProperty]
        public bool MaxAgeNull { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (MinAgeNull)
            {
                Class.MinAge = null;
            }

            if (MaxAgeNull)
            {
                Class.MaxAge = null;
            }

            if (Class.MinAge != null && Class.MaxAge != null)
            {
                if (Class.MaxAge < Class.MinAge)
                {
                    ModelState.AddModelError("Class.MinAge", "Min age must be less than or equal to max age");
                    ModelState.AddModelError("Class.MaxAge", "Max age must be greater than or equal to min age");
                }
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }


            _context.Classes.Add(Class);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
