using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Helpers;
using CozyHouse.Core.ServiceContracts;
using CozyHouse.UI.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyHouse.UI.Areas.Manager.Controllers
{
    [Authorize]
    [Area("Manager")]
    public class PetPublicationController : Controller
    {
        IShelterPetPublicationService _publicationService;
        IUserStatsService _userStatsService;
        public PetPublicationController(IShelterPetPublicationService publicationService, IUserStatsService userStatsService)
        {
            _publicationService = publicationService;
            _userStatsService = userStatsService;
        }
        public IActionResult Index()
        {
            return View(_publicationService.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(ShelterPetPublication publication, IFormFile[] petImages)
        {
            bool result = _publicationService.Add(publication, petImages);
            if (result == true) await _userStatsService.IncreasePublicationsCreatedCounterAsync(User.GetUserId(), 1);

            Notificator.CreateNotification(this, result == true ? "Publication Create Success" : "Publication Create Failure", result == true ? "success" : "error");
            return RedirectToAction("Index");
        }

        public IActionResult Edit(Guid id)
        {
            return View(_publicationService.Get(id));
        }

        [HttpPost]
        public IActionResult Edit(ShelterPetPublication publication)
        {
            bool result = _publicationService.Update(publication);

            Notificator.CreateNotification(this, result == true ? "Publication Update Success" : "Publication Update Failure", result == true ? "success" : "error");
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            bool result = _publicationService.Delete(id);
            if (result == true) await _userStatsService.DecreasePublicationsCreatedCounterAsync(User.GetUserId(), 1);

            Notificator.CreateNotification(this, result == true ? "Publication Delete Success" : "Publication Delete Failure", result == true ? "success" : "error");
            return RedirectToAction("Index");
        }
    }
}
