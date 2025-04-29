using CozyHouse.Core.Domain.IdentityEntities;
using CozyHouse.Core.Helpers;
using CozyHouse.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyHouse.UI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize]
    public class HomeController : Controller
    {
        IAuthorizationManageService _authorizationService;

        public HomeController(IAuthorizationManageService authorizationService, IShelterPetPublicationService shelterPublications, IUserPetPublicationService userPublications)
        {
            _authorizationService = authorizationService;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        public async Task<IActionResult> LogoutCommand()
        {
            await _authorizationService.LogoutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
        public async Task<IActionResult> DeleteAccount()
        {
            ApplicationUser? user = await _authorizationService.GetUserAsync(User.GetUserId());
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(ApplicationUser user)
        {
            await _authorizationService.DeleteAsync(User.GetUserId());
            await _authorizationService.LogoutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
