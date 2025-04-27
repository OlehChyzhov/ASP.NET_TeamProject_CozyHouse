using CozyHouse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using CozyHouse.Infrastructure.Repositories;
using CozyHouse.Core.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CozyHouse.CoreTests.RepositoryTests
{
    public class PetImageRepositoryTests
    {
        private readonly PetImageRepository _repository;

        public PetImageRepositoryTests()
        {
            DbContextOptionsBuilder options = new DbContextOptionsBuilder<ApplicationDbContext>();
            DbContextMock<ApplicationDbContext> dbContextMock =
                new DbContextMock<ApplicationDbContext>(options.Options);

            ApplicationDbContext context = dbContextMock.Object;

            List<PetImage> images = new List<PetImage>()
            {
                new PetImage()
                {
                    Id = Guid.Parse("6623F9E5-7C5A-4FA0-B861-3C6B4F8E08CD"),
                    ImageUrl = "https://example.com/image1.jpg",
                    PetPublicationId = Guid.Parse("5523F9E5-7C5A-4FA0-B861-3C6B4F8E08CD"),
                }
            };

            dbContextMock.CreateDbSetMock(db => db.PetImages, images);

            _repository = new PetImageRepository(context);
        }

        #region Create
        [Fact]
        public void Create_RightArguments_ToBeSuccessful()
        {
            Guid publicationId = Guid.Parse("5523F9E5-7C5A-4FA0-B861-3C6B4F8E08CD");
            PetImage image = new PetImage()
            {
                ImageUrl = "https://example.com/image2.jpg",
                PetPublicationId = publicationId
            };

            _repository.Create(image);

            Assert.Equal(2, _repository.GetAll(publicationId).Count());
        }
        #endregion

        #region Read
        [Fact]
        public void Read_RightId_ToBeNotNull()
        {
            Assert.NotNull(_repository.Read(Guid.Parse("6623F9E5-7C5A-4FA0-B861-3C6B4F8E08CD")));
        }

        [Fact]
        public void Read_WrongId_ToBeInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _repository.Read(Guid.Parse("8A3DCAFC-7421-4B6E-B593-A9E1833BE9B3"));
            });
        }


        [Fact]
        public void GetAll_ExistingPublicationId_ToBeEqualOne()
        {
            Guid publicationId = Guid.Parse("5523F9E5-7C5A-4FA0-B861-3C6B4F8E08CD");

            Assert.Single(_repository.GetAll(publicationId));
        }

        [Fact]
        public void GetAll_NonExistingPublicationId_ToBeEmpty()
        {
            Guid nonExistingPublicationId = Guid.Parse("9A3DCAFC-7421-4B6E-B593-A9E1833BE9B3");

            Assert.Empty(_repository.GetAll(nonExistingPublicationId));
        }
        #endregion

        #region Update
        [Fact]
        public void Update_RightArguments_ToBeSuccessful()
        {
            PetImage imageFromDb = _repository.Read(Guid.Parse("6623F9E5-7C5A-4FA0-B861-3C6B4F8E08CD"));
            string newUrl = "https://example.com/updated-image.jpg";
            imageFromDb.ImageUrl = newUrl;

            _repository.Update(imageFromDb);

            PetImage updatedImage = _repository.Read(Guid.Parse("6623F9E5-7C5A-4FA0-B861-3C6B4F8E08CD"));
            Assert.Equal(newUrl, updatedImage.ImageUrl);
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_RightId_ToBeInvalidOperationException()
        {
            Guid imageId = Guid.Parse("6623F9E5-7C5A-4FA0-B861-3C6B4F8E08CD");

            _repository.Delete(imageId);

            Assert.Throws<InvalidOperationException>(() =>
            {
                _repository.Read(imageId);
            });
        }

        [Fact]
        public void Delete_WrongId_ToBeInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                _repository.Delete(Guid.Parse("9A3DCAFC-7421-4B6E-B593-A9E1833BE9B3"));
            });
        }
        #endregion

    }
}
