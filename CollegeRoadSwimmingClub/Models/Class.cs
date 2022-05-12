using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CollegeRoadSwimmingClub.Models
{
    public class Class
    {

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public Gender Gender { get; set; }
        [DisplayName("Min. Age")]
        [Range(1, int.MaxValue)]
        public int? MinAge { get; set; }
        [DisplayName("Max. Age")]
        [Range(1, int.MaxValue)]
        public int? MaxAge { get; set; }

        public bool IsEligible(Member swimmer)
        {
            bool eligible = true;

            if (!swimmer.IsSwimmer)
            {
                eligible = false;
            }

            //check for age and gender restrictions
            if (MinAge != null && swimmer.Age < MinAge)
            {
                eligible = false;
            }
            if (MaxAge != null && swimmer.Age > MaxAge)
            {
                eligible = false;
            }

            if (swimmer.Gender != Gender && Gender != Gender.Mixed)
            {
                eligible = false;
            }

            return eligible;
            
        }

    }     
}
