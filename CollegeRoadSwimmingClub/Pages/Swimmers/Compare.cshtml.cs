using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CollegeRoadSwimmingClub.Pages.Swimmers
{
    public class CompareModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public CompareModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        public string DateSort { get; set; }
        public string TimeSort { get; set; }
        public string PositionSort { get; set; }
        [BindProperty]
        public List<int> SwimmerIds { get; set; }
        public List<Member> Swimmers { get; set; }
        public List<Result> Results { get; set; }

        public Dictionary<string, string> queryParams { get; set; }

        public int? EventId { get; set; }

        public bool CompOnly { get; set; }
        
        public async Task<IActionResult> OnGetAsync(List<int> selectedSwimmer, int? eventId, string sortOrder, bool compOnly)
        {

            queryParams = new Dictionary<string, string>();
            for (var i = 0; i < selectedSwimmer.Count; i++)
            {
                var paramValue = selectedSwimmer[i];
                queryParams.Add($"{nameof(selectedSwimmer)}[{i}]", paramValue.ToString());
            }
            queryParams.Add("eventId", eventId.ToString());
            queryParams.Add("compOnly", compOnly.ToString());

            if (selectedSwimmer.Count() < 2)
            {
                return RedirectToPage("./Index");
            }

            Swimmers = await _context.Members.Where(m => selectedSwimmer.Contains(m.Id)).ToListAsync();

            if (!Swimmers.Any())
            {
                return NotFound();
            }

            SwimmerIds = selectedSwimmer;

            List<RaceResult> raceResults = await _context.RaceResults.Include(rr => rr.Race).ThenInclude(r => r.Event).Where(rr => selectedSwimmer.Contains(rr.SwimmerId)).ToListAsync();
            List<TrainingResult> trainingResults = await _context.TrainingResults.Include(tr => tr.Event).Where(tr => selectedSwimmer.Contains(tr.SwimmerId)).ToListAsync();

            Results = new List<Result>();
            foreach (RaceResult rr in raceResults)
            {
                Results.Add(new Result() { Swimmer = Swimmers.First(s => s.Id == rr.SwimmerId), Event = rr.Race.Event, Position = rr.Position, Time = rr.Time, Competition = true, Date = rr.Race.DateTime });
            }

            foreach (TrainingResult tr in trainingResults)
            {
                Results.Add(new Result() { Swimmer = Swimmers.First(s => s.Id == tr.SwimmerId), Event = tr.Event, Position = null, Time = tr.Time, Competition = false, Date = tr.Date });
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

            List<SelectListItem> eventList = new SelectList(swimmerEvents, "Id", "Name").ToList();
            eventList.Insert(0, new SelectListItem("All events", "0"));
            ViewData["Events"] = eventList;

            if (eventId != null)
            {
                EventId = eventId;
                if (eventId > 0)
                {
                    Results = Results.Where(r => r.Event.Id == eventId).ToList();
                }
            }

            CompOnly = compOnly;
            if (compOnly)
            {
                Results = Results.Where(r => r.Competition).ToList();
            }
            

            


            return Page();
        }
    }
}
