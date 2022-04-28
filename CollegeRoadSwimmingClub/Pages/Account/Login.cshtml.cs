using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CollegeRoadSwimmingClub.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly CRSCContext _context;

        public LoginModel(CRSCContext context)
        {
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync(string? returnUrl)
        {
            if (!ModelState.IsValid) return Page();

            var user = Authenticate(Credentials);
            if (user == null) return Page();

            ClaimsPrincipal claimsPrincipal = GenerateClaimsPrincipal(user);

            await HttpContext.SignInAsync("CRSCCookieAuth", claimsPrincipal);

            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToPage("/Index");

        }

        [BindProperty]
        public Credentials Credentials { get; set; }

        private ClaimsPrincipal GenerateClaimsPrincipal(User user)
        {

            // create initial list of claims from user details (userID, username)
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            //add each user role to list of claims
            foreach (Role role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role.Name.ToString()));
            }

            var identity = new ClaimsIdentity(claims, "CRSCCookieAuth");
            return new ClaimsPrincipal(identity);
        }

        private User? Authenticate(Credentials credentials)
        {
            var passwordHasher = new PasswordHasher<User>();
            // search for user by username in database, return user object if found, null otherwise

            var currentUser = _context.Users.Include(u => u.Roles).FirstOrDefault(u => u.Username.ToLower() == credentials.Username.ToLower());
            if (currentUser == null) return null;

            if (passwordHasher.VerifyHashedPassword(currentUser, currentUser.Password, credentials.Password) == PasswordVerificationResult.Success)
            {
                return currentUser;
            }
            return null;

        }

    }
}
