using CozyHouse.Core.RepositoryInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CozyHouse.UI.Areas.Guest.Controllers
{
    [AllowAnonymous]
    public class PublicationController : Controller
    {
        IShelterPetPublicationRepository _petPublicationRepository;
        IUserPetPublicationRepository _userPetPublicationRepository;
        public PublicationController(IShelterPetPublicationRepository petPublicationRepository, IUserPetPublicationRepository userPetPublicationRepository)
        {
            _petPublicationRepository = petPublicationRepository;
            _userPetPublicationRepository = userPetPublicationRepository;
        }
        public IActionResult ShelterPublications()
        {
            return View(_petPublicationRepository.GetAll());
        }
        public IActionResult UserPublications()
        {
            return View(_userPetPublicationRepository.GetAll());
        }
        public IActionResult ShelterPublicationDetails(Guid publicationId)
        {
            return View("PetPublicationDetails", _petPublicationRepository.Read(publicationId));
        }
        public IActionResult UserPublicationDetails(Guid publicationId)
        {
            return View("PetPublicationDetails", _userPetPublicationRepository.Read(publicationId));
        }
    }
}
