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

namespace CollegeRoadSwimmingClub.Pages.Galas.Races.Results
{
    public class IndexModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public IndexModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public IList<RaceResult> RaceResult { get;set; }

        public async Task OnGetAsync()
        {
            RaceResult = await _context.RaceResults
                .Include(r => r.Race)
                .ThenInclude(r => r.Class)
                .Include(r => r.Race)
                .ThenInclude(r => r.Event)
                .Include(r => r.Swimmer)
                .ToListAsync();
        }
    }
}
