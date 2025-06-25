using CozyHouse.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CozyHouse.Infrastructure.Database.Configurations
{
    public class ShelterAdoptionRequestConfigurations : IEntityTypeConfiguration<ShelterAdoptionRequest>
    {
        public void Configure(EntityTypeBuilder<ShelterAdoptionRequest> builder)
        {
            builder.HasData(
                new ShelterAdoptionRequest()
                {
                    Id = Guid.Parse(""),
                    AdopterId = Guid.Parse(""),
                    PetPublicationId = Guid.Parse(""),
                    Message = "Message"
                    
                }
            );
        }
    }
}
