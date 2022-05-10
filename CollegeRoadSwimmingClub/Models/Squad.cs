using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeRoadSwimmingClub.Models
{
    public class Squad
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        
        
        public ICollection<Member>? Members { get; set; }
        public List<MemberSquad>? MemberSquad { get; set; }

        [NotMapped]
        public List<Member>? Coaches
        {
            get
            {
                if (Members != null && Members.Count > 0)
                {
                    return Members.Where(m => MemberSquad.Any(ms => ms.MemberId == m.Id && ms.MemberRole == MemberSquadRole.Coach)).ToList();
                }
                return new List<Member>();
            }
        }
        [NotMapped]
        public List<Member>? Swimmers
        {
            get
            {
                if (Members != null && Members.Count > 0)
                {
                    return Members.Where(m => MemberSquad.Any(ms => ms.MemberId == m.Id && ms.MemberRole == MemberSquadRole.Swimmer)).ToList();
                }
                return new List<Member>();
                    
            }
        }

        [NotMapped]
        public int NumberOfSwimmers
        {
            get
            {
                if (Members != null && Members.Count > 0)
                {
                    return Swimmers.Count;
                }
                return 0;
                
            }
        }

        [NotMapped]
        public string? CoachList
        {
            get
            {
                if (Coaches != null && Coaches.Count > 0)
                {
                    var coachList = new List<string>();
                    foreach (Member coach in Coaches)
                    {
                        coachList.Add(coach.FullName);
                    }

                    return string.Join(", ", coachList);
                }
                return null; 
            }
            
        }
    }
}
