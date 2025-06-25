using CozyHouse.Core.Domain.Entities.Interfaces;

namespace CozyHouse.Core.Domain.Entities
{
    public class Image : IEntity
    {
        public Guid Id { get; init; }
        public required string Url { get; set; }
        public required Guid PublicationId { get; set; }
        public virtual Publication Publication { get; set; }
    }
}
