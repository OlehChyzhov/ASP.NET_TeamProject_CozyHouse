using CozyHouse.Core.Domain.Entities.Interfaces;
using CozyHouse.Core.Domain.Enums;

namespace CozyHouse.Core.Domain.Entities
{
    public class Pet : IEntity
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public double Age { get; set; }
        public Species Species { get; set; }

        public Guid PublicationId { get; set; }
        public Publication Publication { get; set; }
    }
}
