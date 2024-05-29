using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FitnessProject2.Models;
using Microsoft.AspNetCore.Authorization;
using FitnessProject2.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    [Authorize]
    public class RateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RateModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            UserFeedbacks = new List<UserRate>();
        }

        public float AverageRating { get; set; }
        public int TotalRatings { get; set; }
        public List<UserRate> UserFeedbacks { get; set; }

        public async Task OnGetAsync(int id)
        {
            AverageRating = await _context.UserRates
                .Where(r => r.ChallengeNumber == id)
                .AverageAsync(r => (float?)r.Rate) ?? 0;

            TotalRatings = await _context.UserRates
                .CountAsync(r => r.ChallengeNumber == id);

            UserFeedbacks = await _context.UserRates
                .Where(r => r.ChallengeNumber == id)
                .Include(r => r.Userid) 
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync(int id, int _rate, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId != null && id != 0 && _rate != 0)
            {
                var existingRating = await _context.UserRates
                    .FirstOrDefaultAsync(r => r.UserId == userId && r.ChallengeNumber == id);

                if (existingRating != null)
                {
                    existingRating.Rate = (short?)_rate;
                    existingRating.Comment = comment;
                }
                else
                {
                    var rating = new UserRate
                    {
                        UserId = userId,
                        Rate = (short?)_rate,
                        ChallengeNumber = id,
                        Comment = comment
                    };
                    _context.UserRates.Add(rating);
                }

                await _context.SaveChangesAsync();
                return RedirectToPage(new { id = id });
            }

            return Page();
        }
    }
}
