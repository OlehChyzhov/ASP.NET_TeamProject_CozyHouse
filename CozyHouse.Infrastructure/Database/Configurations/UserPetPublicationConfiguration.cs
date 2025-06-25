using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CozyHouse.Infrastructure.Database.Configurations
{
    public class UserPetPublicationConfiguration : IEntityTypeConfiguration<UserPetPublication>
    {
        public void Configure(EntityTypeBuilder<UserPetPublication> builder)
        {
            builder.HasData(
                new UserPetPublication()
                {
                    Id = Guid.Parse("16B0C5F5-784E-46E9-A788-171D29622192"),
                    PublicationTitle = "User Publication Title",
                    Summary = "Summary",
                    Description = "Description",
                    Location = "Location",
                    PetName = "User Pet Name",
                    PetAge = 1,
                    PetType = Species.Dog,
                    Breed = "Calm One",
                    IsVaccinated = false,
                    IsSterilized = false
                }
            );
        }
    }
}
