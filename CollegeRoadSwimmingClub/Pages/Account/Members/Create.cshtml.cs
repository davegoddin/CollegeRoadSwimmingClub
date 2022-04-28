#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Account.Members
{
    [Authorize(Roles = "Member")]
    public class CreateModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public CreateModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            int userId = Int32.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            User user = _context.Users.Include(u => u.LinkedMembers).First(u => u.Id == userId);

            Member = new Member()
            {
                Address1 = user.Self.Address1,
                Address2 = user.Self.Address2,
                Town = user.Self.Town,
                County = user.Self.County,
                Postcode = user.Self.Postcode,
                Email = user.Self.Email,
                Telephone = user.Self.Telephone,
                UserId = userId,
                UserMemberLink = UserMemberLink.Parent,
                IsSwimmer = true
            };

            

            return Page();
        }

        [BindProperty]
        public Member Member { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Members.Add(Member);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details");
        }
    }
}
