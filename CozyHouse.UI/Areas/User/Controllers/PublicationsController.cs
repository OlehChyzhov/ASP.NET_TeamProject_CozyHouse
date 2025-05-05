using CozyHouse.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CozyHouse.UI.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class PublicationsController : Controller
    {
        IShelterPetPublicationService _shelterPetPublicationService;
        IUserPetPublicationService _userPetPublicationService;
        public PublicationsController(IShelterPetPublicationService shelterPublications, IUserPetPublicationService userPublications)
        {
            _shelterPetPublicationService = shelterPublications;
            _userPetPublicationService = userPublications;
        }

        public IActionResult ShelterPublications()
        {
            return View(_shelterPetPublicationService.GetAll());
        }
        public IActionResult UserPublications()
        {
            Guid userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            return View(_userPetPublicationService.GetAll().Where(publication => publication.OwnerId != userId));
        }
        public IActionResult ShelterPublicationDetails(Guid publicationId)
        {
            return View("PetPublicationDetails", _shelterPetPublicationService.Get(publicationId));
        }
        public IActionResult UserPublicationDetails(Guid publicationId)
        {
            return View("PetPublicationDetails", _userPetPublicationService.Get(publicationId));
        }
    }
}
