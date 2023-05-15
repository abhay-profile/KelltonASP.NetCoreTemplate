using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TwoFactorAuthentication.Areas.Identity.Data;

namespace TwoFactorAuthentication.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize]
public class UserController : Controller
{
    public UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        return Json(new { data = users });
    }

    [HttpPatch]
    public async Task<IActionResult> LockUnlock(string userId)
    {
        var userIdClaim = new Claim(ClaimTypes.NameIdentifier, userId);
        var principal = new ClaimsPrincipal(new ClaimsIdentity(new[] { userIdClaim }));
        var userFromDb = await _userManager.GetUserAsync(principal);

        if (userFromDb is null)
        {
            return Json(new { success = false, message = "Error while Locking/Unlocking" });
        }

        if (userFromDb.LockoutEnd != null && userFromDb.LockoutEnd > DateTime.Now)
        {
            await _userManager.SetLockoutEndDateAsync(userFromDb, DateTime.Now);
        }
        else
        {
            await _userManager.SetLockoutEndDateAsync(userFromDb, DateTime.Now.AddYears(1000));
        }

        return Json(new { success = true, message = "Operation Successfull" });
    }
}
