using FitnessProject2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    public class ViewChallengesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ViewChallengesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context; 
            _userManager = userManager;
        }

        [BindProperty(SupportsGet = true)]
        public string? Keyword { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Category { get; set; }
        [BindProperty(SupportsGet = true)]
        public string? Difficulty { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? StartDate { get; set; }
        [BindProperty(SupportsGet = true)]
        public DateTime? EndDate { get; set; }

        public List<Challenge>? ChallengesList { get; set; }
        public List<int>? JoinedChallengeIds { get; set; } 

        public async Task OnGetAsync()
        {
            var query = _context.Challenges.AsQueryable();

            if (!string.IsNullOrEmpty(Keyword))
            {
                query = query.Where(c => c.Title.Contains(Keyword));
            }

            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(c => c.Category.Contains(Category));
            }

            if (!string.IsNullOrEmpty(Difficulty))
            {
                query = query.Where(c => c.Difficulty == Difficulty);
            }

            if (StartDate.HasValue)
            {
                query = query.Where(c => c.StartDate >= StartDate.Value);
            }

            if (EndDate.HasValue)
            {
                query = query.Where(c => c.EndDate <= EndDate.Value);
            }

            ChallengesList = query.Where(c => (bool)!c.IsDeleted).ToList();
             var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                JoinedChallengeIds = await _context.Participations
                    .Where(p => p.UserId == user.Id)
                    .Select(p => p.ChallengeNumber)
                    .ToListAsync();
            }
            else
            {
                JoinedChallengeIds = new List<int>();
            }
        }
    }
}
