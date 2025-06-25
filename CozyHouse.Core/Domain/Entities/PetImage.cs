using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using CozyHouse.Core.Domain.Entities.BaseEntities;

namespace CozyHouse.Core.Domain.Entities
{
    public class PetImage : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public required string ImageUrl { get; set; }

        [Required]
        [ForeignKey(nameof(PetPublication))]
        public required Guid PetPublicationId { get; set; }
        public virtual PetPublicationBase PetPublication { get; set; } = null!;
    }
}
