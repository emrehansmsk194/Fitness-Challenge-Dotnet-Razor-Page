using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class Participation
{
    public int Id { get; set; }

    public string UserId { get; set; }
    public int ChallengeNumber { get; set; }
    public DateTime? JoinDate { get; set; }
    public bool IsFavourite { get; set; }

    [ForeignKey(nameof(UserId))]
    public IdentityUser Userid { get; set; }

    [ForeignKey(nameof(ChallengeNumber))]
    public Challenge? Challenge { get; set; }
 
    }