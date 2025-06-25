using System.ComponentModel.DataAnnotations;

namespace CozyHouse.Core.Domain.Entities.BaseEntities
{
    public interface IEntity
    {
        [Key]
        public Guid Id { get; set; }
    }
}
