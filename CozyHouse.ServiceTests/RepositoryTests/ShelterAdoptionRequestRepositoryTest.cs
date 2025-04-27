using CozyHouse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using EntityFrameworkCoreMock;
using CozyHouse.Infrastructure.Repositories;
using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace CozyHouse.CoreTests.RepositoryTests
{
    public class ShelterAdoptionRequestRepositoryTests
    {
        private readonly ShelterAdoptionRequestRepository _repository;

        public ShelterAdoptionRequestRepositoryTests()
        {
            DbContextOptionsBuilder options = new DbContextOptionsBuilder<ApplicationDbContext>();
            DbContextMock<ApplicationDbContext> dbContextMock =
                new DbContextMock<ApplicationDbContext>(options.Options);

            ApplicationDbContext context = dbContextMock.Object;

            List<ShelterAdoptionRequest> requests = new List<ShelterAdoptionRequest>()
            {
                new ShelterAdoptionRequest()
                {
                    Id = Guid.Parse("12345678-1234-1234-1234-123456789012"),
                    Status = AdoptionStatus.Pending
                }
            };
            dbContextMock.CreateDbSetMock(db => db.ShelterAdoptionRequests, requests);

            _repository = new ShelterAdoptionRequestRepository(context);
        }

        #region Create
        [Fact]
        public void Create_RightArguments_ToBeSuccessful()
        {
            ShelterAdoptionRequest request = new ShelterAdoptionRequest()
            {
                Status = AdoptionStatus.Pending
            };

            _repository.Create(request);
            Assert.Equal(2, _repository.GetAll().Count());
        }

        #endregion

        #region Read
        [Fact]
        public void Read_RightId_ToBeNotNull()
        {
            Assert.NotNull(_repository.Read(Guid.Parse("12345678-1234-1234-1234-123456789012")));
        }

        [Fact]
        public void GetAll_ToBeEqualOne()
        {
            Assert.Single(_repository.GetAll());
        }
        #endregion

        #region Update
        [Fact]
        public void Update_RightArguments_ToBeSuccessful()
        {
            ShelterAdoptionRequest requestFromDb = _repository.Read(Guid.Parse("12345678-1234-1234-1234-123456789012"));
            requestFromDb.Status = AdoptionStatus.Approved;

            _repository.Update(requestFromDb);

            ShelterAdoptionRequest updatedRequest = _repository.Read(Guid.Parse("12345678-1234-1234-1234-123456789012"));
            Assert.Equal(AdoptionStatus.Approved, updatedRequest.Status);
        }
        #endregion

        #region Delete

        #endregion
    }
}
