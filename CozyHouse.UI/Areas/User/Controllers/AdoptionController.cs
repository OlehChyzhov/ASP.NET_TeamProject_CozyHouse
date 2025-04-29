using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Helpers;
using CozyHouse.Core.RepositoryInterfaces;
using CozyHouse.Core.ServiceContracts;
using CozyHouse.UI.Models.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyHouse.UI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class AdoptionController : Controller
    {
        IShelterAdoptionRequestService _shelterRequestService;
        IUserAdoptionRequestService _userRequestService;
        IUserPetPublicationRepository _userPublicationRepository;
        public AdoptionController(IShelterAdoptionRequestService shelterRequests, IUserAdoptionRequestService userRequests, IUserPetPublicationRepository userPets)
        {
            _shelterRequestService = shelterRequests;
            _userRequestService = userRequests;
            _userPublicationRepository = userPets;
        }
        [HttpPost]
        public async Task<IActionResult> AdoptFromShelterAsync(Guid publicationId)
        {
            await _shelterRequestService.CreateAsync(publicationId, User.GetUserId());

            Notificator.CreateNotification(this, "Request Submitted! Wait for manager's call", "success");
            return RedirectToAction("Index", "Home", new { area = "User" });
        }
        [HttpPost]
        public async Task<IActionResult> AdoptFromUserAsync(Guid publicationId)
        {
            UserPetPublication publication = _userPublicationRepository.Read(publicationId)!;
            await _userRequestService.CreateAsync(publication.Id, User.GetUserId(), publication.OwnerId);

            Notificator.CreateNotification(this, $"Request Submitted! {publication.Owner.PersonName} will contact you soon", "success");
            return RedirectToAction("Index", "Home", new { area = "User" });
        }
    }
}
