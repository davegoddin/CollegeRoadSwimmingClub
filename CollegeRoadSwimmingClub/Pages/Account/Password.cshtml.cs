using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CollegeRoadSwimmingClub.Pages.Account
{
    public class PasswordModel : PageModel
    {
        private readonly CRSCContext _context;
        public PasswordModel(CRSCContext context)
        {
            _context = context;
        }
        [BindProperty]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [BindProperty]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        public async Task OnGetAsync()
        {

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (HttpContext.User == null || !HttpContext.User.Identity.IsAuthenticated || !HttpContext.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                return RedirectToPage("/Index");
            }

            int userId = Int32.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var user = _context.Users.Find(userId);

            var passwordHasher = new PasswordHasher<User>();

            if (passwordHasher.VerifyHashedPassword(user, user.Password, OldPassword) == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError("OldPassword", "Password is not correct");
                return Page();
            }

            user.Password = passwordHasher.HashPassword(user, NewPassword);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { message = "Update successful" });
        }


    }
}
