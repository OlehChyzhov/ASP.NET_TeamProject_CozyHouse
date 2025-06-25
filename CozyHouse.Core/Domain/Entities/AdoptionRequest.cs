using CozyHouse.Core.Domain.Entities.Interfaces;
using CozyHouse.Core.Domain.Enums;

namespace CozyHouse.Core.Domain.Entities
{
    public class AdoptionRequest : IEntity
    {
        public Guid Id { get; init; }
        public DateTime RequestDate { get; set; }
        public AdoptionStatus Status { get; set; }
        public DateTime? SetStatusDate { get; set; }
    }
}
