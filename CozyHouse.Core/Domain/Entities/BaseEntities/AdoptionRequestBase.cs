using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CozyHouse.Core.Domain.Enums;

namespace CozyHouse.Core.Domain.Entities.BaseEntities
{
    public abstract class AdoptionRequestBase : IEntity
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [DisplayName("Request Date")]
        public DateTime RequestDate { get; set; } = DateTime.UtcNow;
        public string? Message { get; set; }
        [Required]
        public AdoptionStatus Status { get; set; } = AdoptionStatus.Pending;
        [DisplayName("Approved Date")]
        public DateTime? ApprovedDate { get; set; }
        [DisplayName("Rejected Date")]
        public DateTime? RejectedDate { get; set; }
    }
}
