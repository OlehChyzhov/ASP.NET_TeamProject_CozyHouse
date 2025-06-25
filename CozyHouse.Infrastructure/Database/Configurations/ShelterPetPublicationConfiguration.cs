using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CozyHouse.Infrastructure.Database.Configurations
{
    public class ShelterPetPublicationConfiguration : IEntityTypeConfiguration<ShelterPetPublication>
    {
        public void Configure(EntityTypeBuilder<ShelterPetPublication> builder)
        {
            builder.HasData(
                new ShelterPetPublication()
                {
                    Id = Guid.Parse("2F9A3B8F-AD8A-418D-95B2-9073BAA4AE5C"),
                    PublicationTitle = "Publication Title",
                    Summary = "Summary",
                    Description = "Description",
                    Location = "Location",
                    PetName = "Pet Name",
                    PetAge = 1,
                    PetType = Species.Dog,
                    Breed = "Angry One",
                    IsVaccinated = false,
                    IsSterilized = false
                }
            );
        }
    }
}
