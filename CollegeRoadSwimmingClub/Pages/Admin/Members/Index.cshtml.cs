using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Admin.Members
{   
    [Authorize(Roles="Administrator")]
    public class IndexModel : PageModel
    {
        private readonly CRSCContext _context;

        public IndexModel(CRSCContext context)
        {
            _context = context;
        }


        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }


        public string Search { get; set; }



        public IList<Member> Members { get; set; }

        public async Task OnGetAsync(string sortOrder, string search)
        {
            Search = search;

            FirstNameSort = (sortOrder == "firstname_desc" || string.IsNullOrEmpty(sortOrder)) ? "firstname_asc" : "firstname_desc";
            LastNameSort = (sortOrder == "lastname_desc" || string.IsNullOrEmpty(sortOrder)) ? "lastname_asc" : "lastname_desc";



            if (string.IsNullOrEmpty(search))
            {
                Members = await _context.Members.ToListAsync();
            }
            else
            {
                Members = await _context.Members.Where(m => (m.FirstName.Contains(search) || m.LastName.Contains(search))).ToListAsync();
            }

            switch (sortOrder)
            {
                case "firstname_asc":
                    Members = Members.OrderBy(s => s.FirstName).ToList();
                    break;
                case "firstname_desc":
                    Members = Members.OrderByDescending(s => s.FirstName).ToList();
                    break;
                case "lastname_asc":
                    Members = Members.OrderBy(s => s.LastName).ToList();
                    break;
                case "lastname_desc":
                    Members = Members.OrderByDescending(s => s.LastName).ToList();
                    break;
                
                default:
                    Members = Members.OrderBy(s => s.LastName).ToList();
                    break;
            }

        }

    }
}
