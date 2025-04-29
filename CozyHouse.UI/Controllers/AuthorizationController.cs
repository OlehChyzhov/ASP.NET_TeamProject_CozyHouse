using CozyHouse.Core.Domain.IdentityEntities;
using CozyHouse.Core.Helpers;
using CozyHouse.Core.ServiceContracts;
using CozyHouse.UI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CozyHouse.UI.Areas.Guest.Controllers
{
    [AllowAnonymous]
    public class AuthorizationController : Controller
    {
        IAuthorizationManageService _authorizationService;
        public AuthorizationController(IAuthorizationManageService authorizationService)
        {
            _authorizationService = authorizationService;
        }
        public IActionResult Register(RegisterDTO DTO)
        {
            return View(DTO);
        }
        public IActionResult Login(LoginDTO DTO)
        {
            return View(DTO);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterCommand(RegisterDTO data)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = data.Email,
                PersonName = data.Login,
                PhoneNumber = data.PhoneNumber,
                Age = data.Age,
                Location = data.Location,
            };

            IdentityResult result = await _authorizationService.RegisterWithRoleAsync(user, data.Password, "User");
            if (result.Succeeded == true)
            {
                await _authorizationService.LoginAsync(user.UserName, data.Password);
                return RedirectToAction("Index", "Home", new { area = "User" });
            }
            return RedirectToAction("Index", data);
        }

        [HttpPost]
        public async Task<IActionResult> LoginCommand(LoginDTO data)
        {
            ExtendedSignInResult result = await _authorizationService.LoginAsync(data.UserEmail, data.UserPassword);
            if (result.Result.Succeeded == false) return RedirectToAction("Index", data);

            if (User.IsInRole("User")) return RedirectToAction("Index", "Home", new { area = "User" });
            else return RedirectToAction("Index", "Home", new { area = "Manager" });
        }
    }
}
