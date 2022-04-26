namespace CollegeRoadSwimmingClub.Models
{
    public class MemberSquad
    {
        public int MemberId { get; set; }
        public Member Member { get; set; }
        public int SquadId { get; set; }
        public Squad Squad { get; set; }

        public MemberSquadRole MemberRole { get; set; }
    }
}
