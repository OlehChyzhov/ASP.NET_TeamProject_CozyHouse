﻿using CozyHouse.Core.Domain.Entities;
using CozyHouse.Core.RepositoryInterfaces;
using CozyHouse.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CozyHouse.Infrastructure.Repositories
{
    public class ShelterAdoptionRequestRepository : IShelterAdoptionRequestRepository
    {
        ApplicationDbContext _db;
        public ShelterAdoptionRequestRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void Create(ShelterAdoptionRequest request)
        {
            var context = new ValidationContext(request, serviceProvider: null, items: null);
            Validator.ValidateObject(request, context, validateAllProperties: true);
            _db.ShelterAdoptionRequests.Add(request);
            _db.SaveChanges();
        }
        public ShelterAdoptionRequest Read(Guid Id)
        {
            return _db.ShelterAdoptionRequests.Include(r => r.Adopter).Include(r => r.PetPublication).First(request => request.Id == Id);
        }
        public void Update(ShelterAdoptionRequest request)
        {
            var context = new ValidationContext(request, serviceProvider: null, items: null);
            Validator.ValidateObject(request, context, validateAllProperties: true);

            _db.ShelterAdoptionRequests.Update(request);
            _db.SaveChanges();
        }
        public void Delete(Guid id)
        {
            ShelterAdoptionRequest request = _db.ShelterAdoptionRequests.First(req => req.Id == id);
            _db.ShelterAdoptionRequests.Remove(request);
            _db.SaveChanges();
        }
        public IEnumerable<ShelterAdoptionRequest> GetAll()
        {
            return _db.ShelterAdoptionRequests.Include(r => r.Adopter).Include(r => r.PetPublication);
        }
    }
}
