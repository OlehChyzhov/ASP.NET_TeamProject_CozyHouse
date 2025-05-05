using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Helpers;
using CozyHouse.Core.ServiceContracts;
using CozyHouse.UI.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyHouse.UI.Areas.User.Controllers
{
    [Authorize]
    [Area("User")]
    public class UserRequestController : Controller
    {
        IUserAdoptionRequestService _userAdoptionRequestService;
        IUserStatsService _userStatsService;
        public UserRequestController(IUserAdoptionRequestService requests, IUserStatsService userStatsService)
        {
            _userAdoptionRequestService = requests;
            _userStatsService = userStatsService;
        }
        public IActionResult Index()
        {
            var requests = _userAdoptionRequestService.GetAll().Where(request => request.OwnerId == User.GetUserId());
            return View(requests);
        }
        [HttpPost]
        public async Task<IActionResult> Approve(Guid id)
        {
            UserAdoptionRequest? request = _userAdoptionRequestService.Get(id);
            bool result = _userAdoptionRequestService.Approve(id);

            if (result == true && request != null) await _userStatsService.IncreasePetsAdoptedCounterAsync(request.AdopterId, 1);

            if (result == true) Notificator.CreateNotification(this, "Request Approved! :D", "success");
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Reject(Guid id)
        {
            bool result = _userAdoptionRequestService.Reject(id);

            if (result == true) Notificator.CreateNotification(this, "Request Rejected :(", "error");
            return RedirectToAction("Index");
        }
    }
}
