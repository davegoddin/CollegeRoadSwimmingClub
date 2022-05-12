using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Admin.Members
{
    public class DetailsModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public DetailsModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public Member Member { get; set; }

        [BindProperty]
        public Role Role { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null) return BadRequest();

            Member = await _context.Members
                .Include(m => m.User)
                .ThenInclude(u => u.LinkedMembers)
                .Include(m => m.User)
                .ThenInclude(u => u.Roles)
                .FirstOrDefaultAsync(m => m.Id == id);

            List<string> currentRoleList = new List<string>();
            if (Member.User.Roles != null)
            {
                foreach (Role role in Member.User.Roles)
                {
                    currentRoleList.Add(role.Name);
                }
            }

            List<string> linkedMembers = new List<string>();
            if (Member.User.LinkedMembers.Count > 1)
            {
                foreach (Member member in Member.User.LinkedMembers.Where(m => m != Member.User.Self))
                {
                    linkedMembers.Add(member.FullName);
                }
            }

            ViewData["LinkedMembers"] = linkedMembers.Count > 0 ? string.Join(", ", linkedMembers) : "None";
            ViewData["CurrentRoles"] = string.Join(", ", currentRoleList);

            List<Role> availableRoles = _context.Roles.Where(r => !currentRoleList.Contains(r.Name)).ToList();
            ViewData["RoleList"] = new SelectList(availableRoles, "Id", "Name");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? userId, int? memberId)
        {

            if (userId == null || memberId == null) return BadRequest();

            var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(u => u.Id == userId);

            Role = _context.Roles.Find(Role.Id);

            if (user == null) return NotFound();

            user.Roles.Add(Role);
            _context.SaveChanges();

            return RedirectToPage("./Details", new { id = memberId });
                
            
        }
    }
}
