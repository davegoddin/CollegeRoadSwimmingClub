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
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace CollegeRoadSwimmingClub.Pages.Squads
{
    
    public class IndexModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public IndexModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public List<Squad> AllSquads { get;set; }
        public List<Squad> MySquads { get;set; }
        public List<Squad> CoachedSquads { get;set; }

        public async Task OnGetAsync()
        {
            
            AllSquads = await _context.Squads.Include(s => s.Members).Include(s => s.MemberSquad).ToListAsync();
                        

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                User user;
                var userId = Int32.Parse(HttpContext.User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
                user = _context.Users.Include(u => u.LinkedMembers).First(u => u.Id == userId);
                MySquads = AllSquads.Where(s => s.Coaches.Contains(user.Self) || s.Swimmers.Contains(user.Self)).ToList();
                CoachedSquads = AllSquads.Where(s => s.Coaches.Contains(user.Self)).ToList();
            }
            else
            {
                MySquads = new List<Squad>();
            }

            
            
        }
    }
}
