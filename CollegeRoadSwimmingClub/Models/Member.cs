using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CollegeRoadSwimmingClub.Models
{
    public class Member
    {
        //TODO: add full validation
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Address must have a first line")]
        [Display(Name = "Address Line 1")]
        public string Address1 { get; set; }
        [Display(Name = "Address Line 2")]
        public string? Address2 { get; set; }
        public string? Town { get; set; }

        public string? County { get; set; }
        [Required(ErrorMessage = "Address must have a postcode")]
        public string Postcode { get; set; }

        [Required(ErrorMessage = "Must have an email address")]
        [EmailAddress(ErrorMessage = "Must be a valid email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Must have a valid phone number")]
        [Phone]
        public string Telephone { get; set; }

        public int? UserId { get; set; }
        public User? User { get; set; }
        public UserMemberLink? UserMemberLink { get; set; }
        [Display(Name = "Swimmer")]
        public bool IsSwimmer { get; set; }

        
        public ICollection<Squad>? Squads { get; set; }
        public List<MemberSquad>? MemberSquad { get; set; }

        public ICollection<Race>? RacesEntered { get; set; }

        public ICollection<RaceResult>? RaceResults { get; set; }

        public ICollection<TrainingResult>? TrainingResults { get; set; }

        public string FullName
        {
            get { return $"{FirstName} {LastName}"; }
        }

        public int Age
        {
            get
            {
                // calculate swimmer age
                int age = DateTime.Today.Year - DateOfBirth.Year;
                if (DateOfBirth > DateTime.Today.AddYears(-age))
                {
                    age--;
                }
                return age;
            }
        }

        
    }
}
