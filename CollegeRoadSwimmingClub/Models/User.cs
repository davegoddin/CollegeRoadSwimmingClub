using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeRoadSwimmingClub.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "New user must have a username")]
        [MaxLength(50)]
        public string Username { get; set; }
        [Required(ErrorMessage = "New user must have a password")]
        [MinLength(6,ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        public ICollection<Member>? LinkedMembers { get; set; }

        public ICollection<Role>? Roles { get; set; }


        [NotMapped]
        public Member? Self
        {
            get
            {
                if (LinkedMembers == null) return null;

                return LinkedMembers.FirstOrDefault(m => m.UserMemberLink == UserMemberLink.Self);
            }
        }

    }
}
