using CozyHouse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using CozyHouse.Infrastructure.Repositories;
using CozyHouse.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CozyHouse.CoreTests.RepositoryTests
{
    public class UserPetPublicationRepositoryTests
    {
        private readonly UserPetPublicationRepository _repository;

        public UserPetPublicationRepositoryTests()
        {
            DbContextOptionsBuilder options = new DbContextOptionsBuilder<ApplicationDbContext>();
            DbContextMock<ApplicationDbContext> dbContextMock = new DbContextMock<ApplicationDbContext>(options.Options);

            ApplicationDbContext context = dbContextMock.Object;

            List<UserPetPublication> publications = new List<UserPetPublication>
            {
                new UserPetPublication
                {
                    Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                    OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                    PublicationTitle = "Test Publication",
                    Summary = "Test Summary",
                    Location = "Test Location",
                    IsVaccinated = false,
                    IsSterilized = false,
                    PetName = "Test Pet"
                }
            };

            dbContextMock.CreateDbSetMock(db => db.UserPetPublications, publications);

            _repository = new UserPetPublicationRepository(context);
        }

        #region Create
        [Fact]
        public void Create_ValidPublication_ShouldBeSuccessful()
        {
            UserPetPublication publication = new UserPetPublication
            {
                Id = Guid.NewGuid(),
                OwnerId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                PublicationTitle = "New Publication",
                Summary = "New Summary",
                Location = "New Location",
                IsVaccinated = true,
                IsSterilized = false,
                PetName = "New Pet"
            };

            _repository.Create(publication);

            Assert.Equal(2, _repository.GetAll().Count());
        }
        #endregion

        #region Read
        [Fact]
        public void Read_ValidId_ShouldReturnPublication()
        {
            var result = _repository.Read(Guid.Parse("12345678-1234-1234-1234-123456789012"));
            Assert.NotNull(result);
        }

        [Fact]
        public void Read_InvalidId_ShouldThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _repository.Read(Guid.NewGuid()));
        }
        #endregion

        #region Update
        [Fact]
        public void Update_ValidPublication_ShouldBeSuccessful()
        {
            UserPetPublication publication = _repository.Read(Guid.Parse("12345678-1234-1234-1234-123456789012"));
            publication.PublicationTitle = "Updated Title";

            _repository.Update(publication);

            UserPetPublication updatedPublication = _repository.Read(Guid.Parse("12345678-1234-1234-1234-123456789012"));
            Assert.Equal("Updated Title", updatedPublication.PublicationTitle);
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_ValidId_ShouldRemovePublication()
        {
            _repository.Delete(Guid.Parse("12345678-1234-1234-1234-123456789012"));
            Assert.Empty(_repository.GetAll());
        }

        [Fact]
        public void Delete_InvalidId_ShouldThrowInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => _repository.Delete(Guid.NewGuid()));
        }
        #endregion

        #region IsPublicationExists
        [Fact]
        public void IsPublicationExists_ExistingId_ShouldReturnTrue()
        {
            bool exists = _repository.IsPublicationExists(Guid.Parse("12345678-1234-1234-1234-123456789012"));
            Assert.True(exists);
        }

        [Fact]
        public void IsPublicationExists_NonExistingId_ShouldReturnFalse()
        {
            bool exists = _repository.IsPublicationExists(Guid.NewGuid());
            Assert.False(exists);
        }
        #endregion

        #region GetAll
        [Fact]
        public void GetAll_ShouldReturnSinglePublication()
        {
            var publications = _repository.GetAll();
            Assert.Single(publications);
        }
        #endregion
    }
}

