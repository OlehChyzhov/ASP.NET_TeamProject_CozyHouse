using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Domain.IdentityEntities;
using CozyHouse.Core.ServiceContracts;
using CozyHouse.UI.Models;
using CozyHouse.UI.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CozyHouse.UI.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Authorize(Roles = "Manager")]
    public class HomeController : Controller
    {
        SignInManager<ApplicationUser> _signInManager;
        IShelterAdoptionRequestService _requestService;
        IUserStatsService _userStatsService;
        public HomeController(SignInManager<ApplicationUser> signInManager, IShelterAdoptionRequestService requestService, IUserStatsService userStatsService)
        {
            _signInManager = signInManager;
            _requestService = requestService;
            _userStatsService = userStatsService;
        }
        public async Task<IActionResult> IndexAsync()
        {
            IEnumerable<ApplicationUser> users = await _signInManager.UserManager.GetUsersInRoleAsync("User");
            IEnumerable<ApplicationUser> managers = await _signInManager.UserManager.GetUsersInRoleAsync("Manager");
            return View(new UserManagerViewModel() { Users = users, Managers = managers});
        }
        public IActionResult SeeRequests()
        {
            return View(_requestService.GetAll());
        }
        [HttpPost]
        public IActionResult Approve(Guid id)
        {
            ShelterAdoptionRequest? request = _requestService.Get(id);
            bool result = _requestService.Approve(id);

            if (result == true && request != null) _userStatsService.IncreasePetsAdoptedCounterAsync(request.AdopterId, 1);

            if (result == true) Notificator.CreateNotification(this, "Request Approved! :D", "success");
            return RedirectToAction("SeeRequests");
        }
        [HttpPost]
        public IActionResult Reject(Guid id)
        {
            bool result = _requestService.Reject(id);

            if (result == true) Notificator.CreateNotification(this, "Request Rejected :(", "error");
            return RedirectToAction("SeeRequests");
        }
        public async Task<IActionResult> LogoutCommand()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
