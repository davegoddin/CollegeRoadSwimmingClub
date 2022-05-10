#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using CollegeRoadSwimmingClub.Data;
using CollegeRoadSwimmingClub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace CollegeRoadSwimmingClub.Pages.Swimmers
{
    public class IndexModel : PageModel
    {
        private readonly CollegeRoadSwimmingClub.Data.CRSCContext _context;

        public IndexModel(CollegeRoadSwimmingClub.Data.CRSCContext context)
        {
            _context = context;
        }

        
        public string FirstNameSort { get; set; }
        public string LastNameSort { get; set; }
        public string AgeSort {  get; set; }

        public string GenderSort { get; set; }

        public string Search { get; set; }
        public string AgeMax { get; set; }
        public string AgeMin { get; set; }
        public string Gender { get; set; }


        public IList<Member> Swimmers { get;set; }

        public async Task OnGetAsync(string sortOrder, string search, int? ageMax, int? ageMin, string gender)
        {
            Search = search;
            AgeMax = AgeMax;
            AgeMin = AgeMin;
            Gender = gender;

            FirstNameSort = (sortOrder == "firstname_desc" || string.IsNullOrEmpty(sortOrder)) ? "firstname_asc" : "firstname_desc";
            LastNameSort = (sortOrder == "lastname_desc" || string.IsNullOrEmpty(sortOrder)) ? "lastname_asc" : "lastname_desc";
            AgeSort = (sortOrder == "age_desc" || string.IsNullOrEmpty(sortOrder)) ? "age_asc" : "age_desc";
            GenderSort = (sortOrder == "gender_desc" || string.IsNullOrEmpty(sortOrder)) ? "gender_asc" : "gender_desc";

            
            ViewData["GenderList"] = new List<SelectListItem>()
            {
                new SelectListItem() { Text = "All", Value="All"},
                new SelectListItem() { Text = "Male", Value = "Male"},
                new SelectListItem() { Text = "Female", Value="Female"}
            };

            if (string.IsNullOrEmpty(search))
            {
                Swimmers = await _context.Members.Where(m => m.IsSwimmer).ToListAsync();
            }
            else
            {
                Swimmers = await _context.Members.Where(m => m.IsSwimmer && (m.FirstName.Contains(search) || m.LastName.Contains(search))).ToListAsync();
            }

            switch (sortOrder)
            {
                case "firstname_asc":
                    Swimmers = Swimmers.OrderBy(s => s.FirstName).ToList();
                    break;
                case "firstname_desc":
                    Swimmers = Swimmers.OrderByDescending(s => s.FirstName).ToList();
                    break;
                case "lastname_asc":
                    Swimmers = Swimmers.OrderBy(s => s.LastName).ToList();
                    break;
                case "lastname_desc":
                    Swimmers = Swimmers.OrderByDescending(s => s.LastName).ToList();
                    break;
                case "age_asc":
                    Swimmers = Swimmers.OrderBy(s => s.Age).ToList();
                    break;
                case "age_desc":
                    Swimmers = Swimmers.OrderByDescending(s => s.Age).ToList();
                    break;
                case "gender_asc":
                    Swimmers = Swimmers.OrderBy(s=> s.Gender).ToList();
                    break;
                case "gender_desc":
                    Swimmers = Swimmers.OrderByDescending(s => s.Gender).ToList();
                    break;
                default:
                    Swimmers = Swimmers.OrderBy(s => s.LastName).ToList();
                    break;
            }

            if (gender != "All" && !string.IsNullOrEmpty(gender))
            {
                Swimmers = Swimmers.Where(s => s.Gender == (Gender)Enum.Parse(typeof(Gender), gender)).ToList();
            }

            if (ageMax != null)
            {
                Swimmers = Swimmers.Where(s => s.Age <= ageMax).ToList();
            }

            if (ageMin != null)
            {
                Swimmers = Swimmers.Where(s => s.Age >= ageMin).ToList();
            }



            
        }

    }
}
