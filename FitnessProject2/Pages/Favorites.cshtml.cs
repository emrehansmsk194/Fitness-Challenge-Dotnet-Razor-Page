using FitnessProject2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Namespace
{
    public class FavoritesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public List<Challenge>? FavoriteChallenges { get; set; }
         public FavoritesModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> OnGetAsync()
        {
              var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound("User not found");
            }
            FavoriteChallenges = await _context.Participations
                .Include(p => p.Challenge)
                .Where(p => p.UserId == user.Id && p.IsFavourite)
                .Select(p => p.Challenge)
                .ToListAsync();

                return Page();
        }
    }
}
