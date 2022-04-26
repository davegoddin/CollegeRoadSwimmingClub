using System.ComponentModel.DataAnnotations;

namespace CollegeRoadSwimmingClub.Models
{
    public class TrainingResult
    {
        public int Id { get; set; }
        public int SwimmerId { get; set; }
        public Member? Swimmer { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }
        [DisplayFormat(DataFormatString = "{0:g}")]
        public TimeSpan Time { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

    }
}
