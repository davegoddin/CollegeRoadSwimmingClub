using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Identity;

namespace CollegeRoadSwimmingClub.Pages.Account
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
        public User User { get; set; }
        [BindProperty]
        public Member Member { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            //check for existing user with same username
            if (_context.Users.FirstOrDefault(u => u.Username == User.Username) != null)
            {
                ModelState.AddModelError("User.Username", "User with this name already exists");
            }

            // check for under 18s
            if (Member.Age < 18)
            {
                ModelState.AddModelError("Member.DateOfBirth", "Membership for under 18s must be managed by a parent or guardian");
            }
            
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // hash password
            var passwordHasher = new PasswordHasher<User>();
            User.Password = passwordHasher.HashPassword(User, User.Password);

            // give user default role
            User.Roles = new List<Role>() { _context.Roles.First(r => r.Name == "Member") };

            //link user and member
            User.LinkedMembers = new List<Member>();
            _context.Users.Add(User);
            Member.UserMemberLink = UserMemberLink.Self;
            Member.User = User;
            _context.Members.Add(Member);
            User.LinkedMembers.Add(Member);

            await _context.SaveChangesAsync();

            return Redirect("./Login?returnUrl=%2FAccount%2FDetails");
        }
    }
}
