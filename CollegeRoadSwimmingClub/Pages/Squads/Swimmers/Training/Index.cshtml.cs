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

namespace CollegeRoadSwimmingClub.Pages.Squads.Swimmers.Training
{
    public class IndexModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public IndexModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public IList<TrainingResult> TrainingResult { get;set; }

        public async Task OnGetAsync()
        {
            TrainingResult = await _context.TrainingResults
                .Include(t => t.Event)
                .Include(t => t.Swimmer).ToListAsync();
        }
    }
}
