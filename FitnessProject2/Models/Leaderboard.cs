using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Leaderboard
{
    public int Id { get; set; }
    public int? ChallengeId { get; set; }
    public string? UserId { get; set; }
    public int? Rank { get; set; }
    public int? Score { get; set; } 
    public DateTime UpdatedAt { get; set; }
    [ForeignKey(nameof(UserId))]   
    public IdentityUser? User { get; set; }
    [ForeignKey(nameof(ChallengeId))]
    public Challenge? Challenge { get; set; }
}