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
using System.Security.Claims;

namespace CollegeRoadSwimmingClub.Pages.Account
{
    public class DetailsModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public DetailsModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public User User { get;set; }

        public async Task<IActionResult> OnGetAsync(string? pwdOutcome)
        {
            if (pwdOutcome != null)
            {
                ViewData["pwdOutcome"] = pwdOutcome;
            }

            if (!HttpContext.User.Identity.IsAuthenticated || !HttpContext.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                return RedirectToPage("/Index");
            }
            
            int userId = Int32.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            User = _context.Users.Include(u => u.LinkedMembers).Include(u => u.Roles).First(u => u.Id == userId);

            List<string> roleList = new List<string>();
            if (User.Roles != null)
            {
                foreach (Role role in User.Roles)
                {
                    roleList.Add(role.Name);
                }
            }

            ViewData["Roles"] = string.Join(",", roleList);

            return Page();
        }
    }
}
