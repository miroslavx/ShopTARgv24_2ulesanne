using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Domain;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;

namespace ShopTARgv24.ApplicationServices.Services
{
    public class KindergartenServices : IKindergartenServices
    {
        private readonly ShopTARgv24Context _context;
        private readonly IFileServices _fileServices;

        public KindergartenServices(
            ShopTARgv24Context context,
            IFileServices fileServices)
        {
            _context = context;
            _fileServices = fileServices;
        }

        public async Task<Kindergarten> Create(KindergartenDto dto)
        {
            Kindergarten domain = new Kindergarten();

            domain.Id = Guid.NewGuid();
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.Teacher = dto.Teacher;
            domain.EstablishedDate = dto.EstablishedDate;
            domain.Address = dto.Address;
            domain.ContactPhone = dto.ContactPhone;
            domain.Email = dto.Email;
            domain.Description = dto.Description;
            domain.CreatedAt = DateTime.Now;
            domain.ModifiedAt = DateTime.Now;

            if (dto.Files != null)
            {
                _fileServices.UploadFilesToDatabaseForKindergarten(dto, domain);
            }

            await _context.Kindergartens.AddAsync(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarten> Update(KindergartenDto dto)
        {
            Kindergarten domain = new Kindergarten();

            domain.Id = dto.Id;
            domain.GroupName = dto.GroupName;
            domain.ChildrenCount = dto.ChildrenCount;
            domain.KindergartenName = dto.KindergartenName;
            domain.Teacher = dto.Teacher;
            domain.EstablishedDate = dto.EstablishedDate;
            domain.Address = dto.Address;
            domain.ContactPhone = dto.ContactPhone;
            domain.Email = dto.Email;
            domain.Description = dto.Description;
            domain.CreatedAt = dto.CreatedAt;
            domain.ModifiedAt = DateTime.Now;

            
            if (dto.Files != null && dto.Files.Count > 0)
            {
                _fileServices.UploadFilesToDatabaseForKindergarten(dto, domain);
            }

            _context.Kindergartens.Update(domain);
            await _context.SaveChangesAsync();

            return domain;
        }

        public async Task<Kindergarten> DetailAsync(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            return result;
        }

        public async Task<Kindergarten> Delete(Guid id)
        {
            var result = await _context.Kindergartens
                .FirstOrDefaultAsync(x => x.Id == id);

            var images = await _context.FileToDatabase
                .Where(x => x.KindergartenId == id)
                .ToListAsync();

            if (images.Any())
            {
                _context.FileToDatabase.RemoveRange(images);
            }

            _context.Kindergartens.Remove(result);
            await _context.SaveChangesAsync();

            return result;
        }
    }
}