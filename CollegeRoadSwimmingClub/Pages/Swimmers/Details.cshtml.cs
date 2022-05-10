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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CollegeRoadSwimmingClub.Pages.Swimmers
{
    public class DetailsModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public DetailsModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public Member Member { get; set; }
        public List<Result> Results { get; set; }

        public int? EventId {  get; set; }
        public async Task<IActionResult> OnGetAsync(int? id, int? eventId)
        {
            if (id == null)
            {
                return NotFound();
            }

            Member = await _context.Members.FirstOrDefaultAsync(m => m.Id == id);

            if (Member == null)
            {
                return NotFound();
            }

            List<RaceResult> raceResults = await _context.RaceResults.Include(rr => rr.Race).ThenInclude(r => r.Event).Where(rr => rr.SwimmerId == Member.Id).ToListAsync();
            List<TrainingResult> trainingResults = await _context.TrainingResults.Include(tr => tr.Event).Where(tr => tr.SwimmerId == Member.Id).ToListAsync();

            if (eventId != null)
            {
                EventId = eventId;
                if (eventId > 0)
                {
                    raceResults = raceResults.Where(rr => rr.Race.EventId == eventId).ToList();
                    trainingResults = trainingResults.Where(tr => tr.EventId == eventId).ToList();
                }
            }

            Results = new List<Result>();

            foreach (RaceResult rr in raceResults)
            {
                Results.Add(new Result() { Swimmer = Member, Event = rr.Race.Event, Position = rr.Position, Time = rr.Time, Competition = true, Date = rr.Race.DateTime });
            }

            foreach (TrainingResult tr in trainingResults)
            {
                Results.Add(new Result() { Swimmer = Member, Event = tr.Event, Position = null, Time = tr.Time, Competition = false, Date = tr.Date });
            }

            List<Event> swimmerEvents = new List<Event>();

            foreach (Result result in Results)
            {
                if (!swimmerEvents.Contains(result.Event))
                {
                    swimmerEvents.Add(result.Event);
                }
            }

            List<SelectListItem> eventList = new SelectList(swimmerEvents, "Id", "Name").ToList();
            eventList.Insert(0, new SelectListItem("All events", "0"));
            ViewData["Events"] = eventList;


            return Page();
        }
    }
}
