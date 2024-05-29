using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using FitnessProject2.Models;
using FitnessProject2.Data;


namespace MyApp.Namespace
{
    public class UserRatesModel : PageModel
    {
       
        private readonly ApplicationDbContext _context;

        public UserRatesModel(ApplicationDbContext context)
        {
            _context = context;
        }
         [BindProperty]
        public Challenge NewChallenge   { get; set; } = default!;
        public List<Challenge> ChallengeList { get; set;} =  default!;
        public List<AverageRating> AverageRatings { get; set; } = default!;
        public List<float> Ratings { get; set; } = default!;
        public void OnGet()
        {
            ChallengeList = (from item in _context.Challenges
                            where item.IsDeleted == false
                            select item).ToList();
            AverageRatings = (from challenge in _context.Challenges
                  join rate in _context.UserRates on challenge.Id equals rate.ChallengeNumber into gj
                  from subRate in gj.DefaultIfEmpty()
                  group subRate by challenge into g
                  select new AverageRating{
                      ChallengeNumber = g.Key.Id,
                      Average = g.Any() ? (float)g.Average(r => r.Rate ?? 0) : 0
                  }).ToList();
            Ratings = AverageRatings.Select(x => x.Average).ToList();

        }
    }
}
