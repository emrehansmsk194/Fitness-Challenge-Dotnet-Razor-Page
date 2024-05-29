using FitnessProject2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    public class ChallengesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ChallengesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public List<ChallengeWithScore>ChallengesList {get; set;}
        public class ChallengeWithScore : Challenge
        {
            public int? Score { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }

            ChallengesList = await _context.Participations
                .Where(p => p.UserId == user.Id)
                .Select(p => new ChallengeWithScore
                {
                    Id = p.Challenge.Id,
                    Title = p.Challenge.Title,
                    Category = p.Challenge.Category,
                    Difficulty = p.Challenge.Difficulty,
                    StartDate = p.Challenge.StartDate,
                    EndDate = p.Challenge.EndDate,
                    Score = _context.Leaderboards
                        .Where(l => l.UserId == p.UserId && l.ChallengeId == p.ChallengeNumber)
                        .Select(l => l.Score)
                        .FirstOrDefault()
                })
                .ToListAsync();

            return Page();
        }
    }
}