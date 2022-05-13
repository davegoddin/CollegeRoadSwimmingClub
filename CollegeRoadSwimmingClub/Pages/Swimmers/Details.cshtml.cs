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

        public string DateSort { get; set; }
        public string TimeSort { get; set; }
        public string PositionSort { get; set; }

        public Member Member { get; set; }
        public List<Result> Results { get; set; }

        public int? EventId {  get; set; }
        public bool CompOnly { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id, int? eventId, string sortOrder, bool compOnly)
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

            DateSort = (sortOrder == "date_desc" || string.IsNullOrEmpty(sortOrder)) ? "date_asc" : "date_desc";
            TimeSort = (sortOrder == "time_desc" || string.IsNullOrEmpty(sortOrder)) ? "time_asc" : "time_desc";
            PositionSort = (sortOrder == "position_desc" || string.IsNullOrEmpty(sortOrder)) ? "position_asc" : "position_desc";


            switch (sortOrder)
            {
                case "date_asc":
                    Results = Results.OrderBy(r => r.Date).ToList();
                    break;
                case "date_desc":
                    Results = Results.OrderByDescending(r => r.Date).ToList();
                    break;
                case "time_asc":
                    Results = Results.OrderBy(r => r.Time).ToList();
                    break;
                case "time_desc":
                    Results = Results.OrderByDescending(r => r.Time).ToList();
                    break;
                case "position_asc":
                    Results = Results.OrderBy(r => r.Position).ToList();
                    break;
                case "position_desc":
                    Results = Results.OrderByDescending(r => r.Position).ToList();
                    break;
                default:
                    Results = Results.OrderByDescending(r => r.Date).ToList();
                    break;
            }


            List<Event> swimmerEvents = new List<Event>();

            foreach (Result result in Results)
            {
                if (!swimmerEvents.Contains(result.Event))
                {
                    swimmerEvents.Add(result.Event);
                }
            }

            CompOnly = compOnly;
            if (compOnly)
            {
                Results = Results.Where(r => r.Competition).ToList();
            }

            List<SelectListItem> eventList = new SelectList(swimmerEvents, "Id", "Name").ToList();
            eventList.Insert(0, new SelectListItem("All events", "0"));
            ViewData["Events"] = eventList;


            return Page();
        }
    }
}
