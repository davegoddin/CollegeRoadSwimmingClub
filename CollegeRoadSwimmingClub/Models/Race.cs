namespace CollegeRoadSwimmingClub.Models
{
    public class Race
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }

        public int GalaId { get; set; }
        public Gala? Gala { get; set; }
        public int ClassId { get; set; }
        public Class? Class { get; set; }
        public DateTime DateTime { get; set; }

        public ICollection<Member>? Entrants { get; set; }

        public ICollection<RaceResult>? RaceResults { get; set; }

        public string? Name
        {
            get
            {
                if (Class == null || Event == null)
                {
                    return null;
                }
                
                return $"{Event.Name} ({ Class.Name})";
            }
        }

    }
}
