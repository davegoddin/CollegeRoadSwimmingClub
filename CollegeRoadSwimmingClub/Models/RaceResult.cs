using System.ComponentModel.DataAnnotations;

namespace CollegeRoadSwimmingClub.Models
{
    public class RaceResult
    {
        public int Id { get; set; }
        public int RaceId { get; set; }
        public Race? Race { get; set; }
        public int SwimmerId { get; set; }
        public Member? Swimmer { get; set; }
        [DisplayFormat(DataFormatString = "{0:h\\:mm\\:ss\\.ff}")]
        [Required]
        [Range(typeof(TimeSpan), "0:00:00.01", "9:59:59.99")]
        public TimeSpan Time { get; set; }
        [Required]
        [Range(minimum: 1, Int32.MaxValue, ErrorMessage = "Must be a positive integer")]
        public int Position { get; set; }
    }
}
