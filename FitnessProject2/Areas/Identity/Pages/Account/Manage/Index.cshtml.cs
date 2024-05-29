// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using FitnessProject2.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FitnessProject2.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly ApplicationDbContext _context;

    public IndexModel(
        UserManager<IdentityUser> userManager,
        SignInManager<IdentityUser> signInManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public string Username { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }
    public List<Challenge> FavoriteChallenges { get; set; }
    [BindProperty]
    public BufferedSingleFileUploadDb FileUpload { get; set; } = new BufferedSingleFileUploadDb();
    public byte[]? Picture { get; set; }
    public UserDetail? ProfileDetail { get; set; }

    public class InputModel
    {
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Bio")]
        public string Bio { get; set; }
    }

    public class BufferedSingleFileUploadDb
    {
        [Display(Name = "Profile Picture")]
        public IFormFile? FormFile { get; set; }
    }

    private async Task LoadAsync(IdentityUser user)
    {
        var userName = await _userManager.GetUserNameAsync(user);
        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

        Username = userName;

        Input = new InputModel
        {
            PhoneNumber = phoneNumber
        };
        ProfileDetail = _context.UserDetails.Where(p => p.UserId == user.Id).FirstOrDefault();
        if (ProfileDetail != null && ProfileDetail.Photo != null)
        {
            Picture = ProfileDetail.Photo;
            Input.Bio = ProfileDetail.Bio;
        }
        else
        {
            // Save a default image if no profile photo is available
            string path = "./wwwroot/images/profile.jpg";
            using var stream = System.IO.File.OpenRead(path);
            var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            Picture = memoryStream.ToArray();
            ProfileDetail = new UserDetail
            {
                Photo = Picture,
                UserId = user.Id
            };
            _context.UserDetails.Add(ProfileDetail);
            await _context.SaveChangesAsync();
        }
        FavoriteChallenges = await _context.Participations
            .Include(p => p.Challenge)
            .Where(p => p.UserId == user.Id && p.IsFavourite)
            .Select(p => p.Challenge)
            .ToListAsync();
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        await LoadAsync(user);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
        }

        if (!ModelState.IsValid)
        {
            await LoadAsync(user);
            return Page();
        }

        var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
        if (Input.PhoneNumber != phoneNumber)
        {
            var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
            if (!setPhoneResult.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to set phone number.";
                return RedirectToPage();
            }
        }

        await _signInManager.RefreshSignInAsync(user);
        ProfileDetail = _context.UserDetails.Where(p => p.UserId == user.Id).FirstOrDefault();
        if (ProfileDetail != null)
        {
            ProfileDetail.Bio = Input.Bio;  // Save the biography
            if (FileUpload.FormFile != null)
            {
                var memoryStream = new MemoryStream();
                await FileUpload.FormFile.CopyToAsync(memoryStream);
                ProfileDetail.Photo = memoryStream.ToArray();
            }
            _context.UserDetails.Update(ProfileDetail);
        }
        else
        {
            ProfileDetail = new UserDetail
            {
                UserId = user.Id,
                Bio = Input.Bio,
                Photo = Picture
            };
            if (FileUpload.FormFile != null)
            {
                var memoryStream = new MemoryStream();
                await FileUpload.FormFile.CopyToAsync(memoryStream);
                ProfileDetail.Photo = memoryStream.ToArray();
            }
            _context.UserDetails.Add(ProfileDetail);
        }
        await _context.SaveChangesAsync();
        StatusMessage = "Your profile has been updated";
        return RedirectToPage();
    }
}
}
