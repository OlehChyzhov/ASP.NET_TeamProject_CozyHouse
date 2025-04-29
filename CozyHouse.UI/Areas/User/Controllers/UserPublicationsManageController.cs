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
    public class UserPublicationsManageController : Controller
    {
        IUserPetPublicationService _publicationService;
        IUserStatsService _userStatsService;
        public UserPublicationsManageController(IUserPetPublicationService publicationService, IUserStatsService userStatsService)
        {
            _publicationService = publicationService;
            _userStatsService = userStatsService;
        }

        public IActionResult Index()
        {
            return View(_publicationService.GetAll().Where(pub => pub.OwnerId == User.GetUserId()));
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserPetPublication publication, IFormFile[] petImages)
        {
            bool result = _publicationService.Add(publication, petImages);
            if (result == true) _userStatsService.IncreasePublicationsCreatedCounterAsync(User.GetUserId(), 1);

            Notificator.CreateNotification(this, result == true ? "Publication Create Success" : "Publication Create Error", result == true ? "success" : "error");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            return View(_publicationService.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(UserPetPublication publication)
        {
            bool result = _publicationService.Update(publication);

            Notificator.CreateNotification(this, result == true ? "Publication Update Success" : "Publication Update Error", result == true ? "success" : "error");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(Guid id)
        {
            bool result = _publicationService.Delete(id);
            if (result == true) _userStatsService.DecreasePublicationsCreatedCounterAsync(User.GetUserId(), 1);

            Notificator.CreateNotification(this, result == true ? "Publication Delete Success" : "Publication Delete Error", result == true ? "success" : "error");
            return RedirectToAction("Index");
        }
    }
}
