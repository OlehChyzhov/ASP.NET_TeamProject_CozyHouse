using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.Domain.IdentityEntities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CozyHouse.Infrastructure.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
    {
        public ApplicationDbContext(DbContextOptions options): base(options) {}

        public virtual DbSet<ShelterPetPublication> ShelterPetPublications { get; set; }
        public virtual DbSet<UserPetPublication> UserPetPublications { get; set; }
        public virtual DbSet<ShelterAdoptionRequest> ShelterAdoptionRequests { get; set; }
        public virtual DbSet<UserAdoptionRequest> UserAdoptionRequests { get; set; }
        public virtual DbSet<PetImage> PetImages { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Cascade delete: User → UserPetPublication
            builder.Entity<UserPetPublication>()
                .HasOne(p => p.Owner)
                .WithMany()
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade delete: User → UserAdoptionRequest (Adopter)
            builder.Entity<UserAdoptionRequest>()
                .HasOne(r => r.Adopter)
                .WithMany()
                .HasForeignKey(r => r.AdopterId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade delete: User → UserAdoptionRequest (Owner)
            builder.Entity<UserAdoptionRequest>()
                .HasOne(r => r.Owner)
                .WithMany()
                .HasForeignKey(r => r.OwnerId)
                .OnDelete(DeleteBehavior.Cascade);

            // Cascade delete: User → ShelterAdoptionRequest (Adopter)
            builder.Entity<ShelterAdoptionRequest>()
                .HasOne(r => r.Adopter)
                .WithMany()
                .HasForeignKey(r => r.AdopterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
