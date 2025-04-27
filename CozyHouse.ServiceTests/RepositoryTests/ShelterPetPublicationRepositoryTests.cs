using CozyHouse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using CozyHouse.Infrastructure.Repositories;
using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CozyHouse.CoreTests.RepositoryTests
{
    public class ShelterPetPublicationRepositoryTests
    {
        private readonly ShelterPetPublicationRepository _repository;
        public ShelterPetPublicationRepositoryTests()
        {
            DbContextOptionsBuilder options = new DbContextOptionsBuilder<ApplicationDbContext>(); // Будує параметри для створення контексту бази даних
            DbContextMock<ApplicationDbContext> dbContextMock =
                new DbContextMock<ApplicationDbContext>(options.Options); // Створює мок-об'єкт для ApplicationDbContext (імітація бази даних)

            ApplicationDbContext context = dbContextMock.Object; // Отримуємо об'єкт імітованого контексту бази даних

            List<ShelterPetPublication> publications = new List<ShelterPetPublication>() // Початковий набір даних для імітації таблиці Listings
            {
                new ShelterPetPublication()
                {
                    Id = Guid.Parse("5523F9E5-7C5A-4FA0-B861-3C6B4F8E08CD"),
                    PublicationTitle = "Listing 1",
                    Summary = "Content",
                    PetName = "Test Pet",
                    PetAge = 1,
                    PetType = Species.Cat,
                    Location = "Location",
                    IsSterilized = false,
                    IsVaccinated = false,
                }
            };
            dbContextMock.CreateDbSetMock(db => db.ShelterPetPublications, publications); // Створюємо імітований DbSet<Listing> із початковими даними

            _repository = new ShelterPetPublicationRepository(context); // Ініціалізуємо репозиторій, використовуючи імітований контекст

        }

        #region Add
        [Fact]
        public void Create_RightArugments_ToBeSuccessful()
        {
            ShelterPetPublication publication = new ShelterPetPublication()
            {
                PublicationTitle = "New Publication",
                Summary = "New Content",
                PetName = "New Pet",
                PetAge = 2,
                PetType = Species.Hamster,
                Location = "New Location",
                IsSterilized = false,
                IsVaccinated = false,
            };

            _repository.Create(publication);
            Assert.Equal(2, _repository.GetAll().Count());
        }
        #endregion

        #region Get
        [Fact]
        public void Read_RightId_ToBeNotNull()
        {
            Assert.NotNull(_repository.Read(Guid.Parse("5523F9E5-7C5A-4FA0-B861-3C6B4F8E08CD")));
        }

        [Fact]
        public void GetAll_NoArguments_ToBeEqualOne()
        {
            Assert.Single(_repository.GetAll());
        }
        #endregion

        #region Update
        [Fact]
        public void Update_RightArguments_ToBeSuccessful()
        {
            ShelterPetPublication publicationFromDb = _repository.Read(Guid.Parse("5523F9E5-7C5A-4FA0-B861-3C6B4F8E08CD"))!;
            publicationFromDb.PublicationTitle = "New Title";

            _repository.Update(publicationFromDb);

            ShelterPetPublication updatedPublication = _repository.Read(Guid.Parse("5523F9E5-7C5A-4FA0-B861-3C6B4F8E08CD"))!;
            Assert.Equal("New Title", updatedPublication.PublicationTitle);
        }
        #endregion

        #region Delete

        [Fact]
        public void Delete_WrongId_ToBeInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _repository.Delete(Guid.Parse("5523F9E5-7C5A-4FA0-B861-366B4F8E08CD"));
            });
        }
        #endregion
    }
}
