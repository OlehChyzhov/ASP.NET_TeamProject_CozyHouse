using CozyHouse.Core.Domain.Entities.Interfaces;

namespace CozyHouse.Core.Domain.Entities
{
    public class Publication : IEntity
    {
        public Guid Id { get; init; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Location { get; set; }

        public Guid PetId { get; set; }
        public Pet Pet { get; set; }
        List<Image> PetImages { get; set; }
    }
}
