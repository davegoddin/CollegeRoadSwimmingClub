using System.ComponentModel.DataAnnotations;

namespace CollegeRoadSwimmingClub.Models
{
    public class Result
    {
        public Event Event { get; set; }
        [DisplayFormat(DataFormatString = "{0:h\\:mm\\:ss\\.ff}")]
        public TimeSpan Time { get; set; }
        public int? Position { get; set; }
        public Member Swimmer { get; set; }
        public bool Competition { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
    }
}
