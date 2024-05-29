using FitnessProject2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace MyApp.Namespace
{
    public class LeaderboardsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public LeaderboardsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ChallengeLeaderboard> ChallengesWithLeaderboards { get; set; }

        public class ChallengeLeaderboard
        {
            public Challenge Challenge { get; set; }
            public List<Leaderboard> LeaderboardEntries { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var challenges = await _context.Challenges
                .Include(c => c.Leaderboards)
                .ThenInclude(l => l.User)
                .ToListAsync();

            ChallengesWithLeaderboards = challenges.Select(c => new ChallengeLeaderboard
            {
                Challenge = c,
                LeaderboardEntries = c.Leaderboards.OrderByDescending(l => l.Score).ToList()
            }).ToList();

            return Page();
        }
    }
}

