using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeRoadSwimmingClub.Models
{
    
    public class Event
    {
        public int Id { get; set; }
        public int Distance { get; set; }
        public Stroke Stroke { get; set; }

        [NotMapped]
        public string Name
        {
            get
            {
                return $"{Distance.ToString()}m {Stroke.ToString()}";
            }
        }

    }
}
