using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using CozyHouse.Core.Domain.Enums;

namespace CozyHouse.Core.Domain.Entities.BaseEntities
{
    public abstract class PetPublicationBase : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [DisplayName("Publication Title")]
        public required string PublicationTitle { get; set; }
        [Required]
        public required string Summary { get; set; }
        public string? Description { get; set; }
        [Required]
        [DisplayName("Published Date")]
        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
        [Required]
        public required string Location { get; set; }


        [Required]
        [DisplayName("Pet Name")]
        public required string PetName { get; set; }
        [Required]
        [DisplayName("Pet Age")]
        public double PetAge { get; set; }
        [DisplayName("Pet Type")]
        public Species PetType { get; set; }
        public string? Breed { get; set; }
        public required bool IsVaccinated { get; set; }
        public required bool IsSterilized { get; set; }
        public bool IsAdopted { get; set; } = false;


        public virtual List<PetImage> Images { get; set; } = new();
    }
}
