namespace CollegeRoadSwimmingClub.Models
{
    public class Class
    {

        public int Id { get; set; }
        public string Name { get; set; }

        public Gender Gender { get; set; }

        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        public bool IsEligible(Member swimmer)
        {
            bool eligible = true;

            

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
