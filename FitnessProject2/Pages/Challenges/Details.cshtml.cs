using FitnessProject2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace MyApp.Namespace
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public Challenge? Challenge { get; set; }
        public Participation Participant {get; set;}

        public DetailsModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Challenge = await _context.Challenges.FindAsync(id);
            if (Challenge == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostSubmitScoreAsync(int id, int score, bool isFavorite)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToPage("/Account/Login");
            }
             bool isFavoriteBool = isFavorite == true;
             var participation = _context.Participations.FirstOrDefault(p => p.UserId == user.Id && p.ChallengeNumber == id);
            if (participation == null)
            {
                participation = new Participation //buraya bak favorite kısmında sorun var.
                {
                    UserId = user.Id,
                    ChallengeNumber = id,
                    JoinDate = DateTime.UtcNow,
                    IsFavourite = isFavoriteBool
                };
                _context.Participations.Add(participation);
            }
            else
            {
                participation.IsFavourite = isFavorite;
                _context.Participations.Update(participation);
            }


            var leaderboardEntry = _context.Leaderboards.FirstOrDefault(l => l.UserId == user.Id && l.ChallengeId == id);
            if (leaderboardEntry == null)
            {
                leaderboardEntry = new Leaderboard
                {
                    UserId = user.Id,
                    ChallengeId = id,
                    Score = score,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Leaderboards.Add(leaderboardEntry);
            }
            else
            {
                leaderboardEntry.Score = score;
                leaderboardEntry.UpdatedAt = DateTime.UtcNow;
                _context.Leaderboards.Update(leaderboardEntry);
            }
            
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Your score has been successfully submitted!";
            return RedirectToPage("/Challenges/Details", new { id = id });
        }
    }
}