using FitnessProject2.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitnessProject2.Pages;
[Authorize]
public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;

    public IndexModel(ApplicationDbContext context){
        _context = context;
    }
    public List<ChallengeViewModel> PopularChallenges { get; set; }
    public List<Challenge> ChallengeLists { get; set; }
  

    [BindProperty]
    public Challenge NewChallenge {get; set;}


         public async Task OnGetAsync()
        {
               var query = _context.Challenges.AsQueryable();
            
            PopularChallenges = await _context.Challenges
                .Where(c => (bool)!c.IsDeleted)
                .Select(c => new ChallengeViewModel
                {
                    Challenge = c,
                    AverageRating = c.UserRates.Average(ur => (float?)ur.Rate) ?? 0
                })
                .OrderByDescending(c => c.AverageRating)
                .Take(5) 
                .ToListAsync();
            ChallengeLists = query.Where(c => (bool)!c.IsDeleted).ToList();
            
        }
    public async Task<IActionResult> OnPostAsync(){
        if (!ModelState.IsValid){
            return Page();
        }
        NewChallenge.IsDeleted = false;
        _context.Challenges.Add(NewChallenge);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
      public class ChallengeViewModel
    {
        public Challenge Challenge { get; set; }
        public float AverageRating { get; set; }
    }
    
}