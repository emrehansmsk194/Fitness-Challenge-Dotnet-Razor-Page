using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

public class UserRate{
    [Key]
    public int Id { get; set; }
    public string UserId { get; set; }
    public int? ChallengeNumber { get; set; }
    public short? Rate { get; set; }
    public string? Comment { get; set; }

    [ForeignKey(nameof(ChallengeNumber))]
    public Challenge? Challenge { get; set; }

    [ForeignKey(nameof(UserId))]
    public IdentityUser Userid { get; set; }

}