using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CollegeRoadSwimmingClub.Pages.Admin.Classes
{
    public class IndexModel : PageModel
    {

        private CRSCContext _context;
        public IndexModel(CRSCContext context)
        {
            _context = context;
        }

        public List<Class> Classes { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            Classes = _context.Classes.ToList();

            return Page();
        }
    }
}
