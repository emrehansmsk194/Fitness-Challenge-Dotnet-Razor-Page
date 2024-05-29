using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
public class UserDetail {
[Key]
public int Id { get; set; }
public byte[] Photo {get; set;}
public string? Bio {get; set;}
public string? UserId { get; set; }

[ForeignKey(nameof(UserId))]
 public IdentityUser Userid { get; set; }



}