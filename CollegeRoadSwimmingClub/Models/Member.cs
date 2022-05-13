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
        [Required]
        [MaxLength(32)]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(32)]
        public string LastName { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        [Required]
        public DateTime DateOfBirth { get; set; }

        public Gender Gender { get; set; }
        [Required(ErrorMessage = "Address must have a first line")]
        [Display(Name = "Address Line 1")]
        [MaxLength(32)]
        public string Address1 { get; set; }
        [Display(Name = "Address Line 2")]
        [MaxLength(32)]
        public string? Address2 { get; set; }
        [MaxLength(32)]
        public string? Town { get; set; }
        [MaxLength(32)]
        public string? County { get; set; }
        [Required(ErrorMessage = "Address must have a postcode")]
        [RegularExpression("^[A-Z]{1,2}[0-9R][0-9A-Z]? [0-9][ABD-HJLNP-UW-Z]{2}$", ErrorMessage="Invalid postcode")]
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
