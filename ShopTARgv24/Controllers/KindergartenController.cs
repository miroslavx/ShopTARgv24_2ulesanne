using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;
using ShopTARgv24.Models.Kindergartens;

namespace ShopTARgv24.Controllers
{
    public class KindergartenController : Controller
    {
        private readonly ShopTARgv24Context _context;
        private readonly IKindergartenServices _kindergartenServices;
        private readonly IFileServices _fileServices;

        public KindergartenController(
            ShopTARgv24Context context,
            IKindergartenServices kindergartenServices,
            IFileServices fileServices)
        {
            _context = context;
            _kindergartenServices = kindergartenServices;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.Kindergartens
                .Select(x => new KindergartenIndexViewModel
                {
                    Id = x.Id,
                    GroupName = x.GroupName,
                    ChildrenCount = x.ChildrenCount,
                    KindergartenName = x.KindergartenName,
                    Teacher = x.Teacher,
                    EstablishedDate = x.EstablishedDate
                });

            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            KindergartenCreateUpdateViewModel result = new();
            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                Teacher = vm.Teacher,
                EstablishedDate = vm.EstablishedDate,
                Address = vm.Address,
                ContactPhone = vm.ContactPhone,
                Email = vm.Email,
                Description = vm.Description,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                Images = vm.Images.Select(x => new FileToDatabaseDto
                {
                    Id = x.Id,
                    ImageData = x.ImageData,
                    ImageTitle = x.ImageTitle,
                    KindergartenId = x.KindergartenId,
                }).ToArray()
            };

            var result = await _kindergartenServices.Create(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] photos = await FilesFromDatabase(id);

            var vm = new KindergartenCreateUpdateViewModel();
            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.Teacher = kindergarten.Teacher;
            vm.EstablishedDate = kindergarten.EstablishedDate;
            vm.Address = kindergarten.Address;
            vm.ContactPhone = kindergarten.ContactPhone;
            vm.Email = kindergarten.Email;
            vm.Description = kindergarten.Description;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.ModifiedAt = kindergarten.ModifiedAt;
            vm.Images.AddRange(photos);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(KindergartenCreateUpdateViewModel vm)
        {
            var dto = new KindergartenDto()
            {
                Id = vm.Id,
                GroupName = vm.GroupName,
                ChildrenCount = vm.ChildrenCount,
                KindergartenName = vm.KindergartenName,
                Teacher = vm.Teacher,
                EstablishedDate = vm.EstablishedDate,
                Address = vm.Address,
                ContactPhone = vm.ContactPhone,
                Email = vm.Email,
                Description = vm.Description,
                CreatedAt = vm.CreatedAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files 
            };

            var result = await _kindergartenServices.Update(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] photos = await FilesFromDatabase(id);

            var vm = new KindergartenDeleteViewModel();
            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.Teacher = kindergarten.Teacher;
            vm.EstablishedDate = kindergarten.EstablishedDate;
            vm.Address = kindergarten.Address;
            vm.ContactPhone = kindergarten.ContactPhone;
            vm.Email = kindergarten.Email;
            vm.Description = kindergarten.Description;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.ModifiedAt = kindergarten.ModifiedAt;
            vm.Images.AddRange(photos);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var kindergarten = await _kindergartenServices.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var kindergarten = await _kindergartenServices.DetailAsync(id);

            if (kindergarten == null)
            {
                return NotFound();
            }

            KindergartenImageViewModel[] photos = await FilesFromDatabase(id);

            var vm = new KindergartenDetailsViewModel();
            vm.Id = kindergarten.Id;
            vm.GroupName = kindergarten.GroupName;
            vm.ChildrenCount = kindergarten.ChildrenCount;
            vm.KindergartenName = kindergarten.KindergartenName;
            vm.Teacher = kindergarten.Teacher;
            vm.EstablishedDate = kindergarten.EstablishedDate;
            vm.Address = kindergarten.Address;
            vm.ContactPhone = kindergarten.ContactPhone;
            vm.Email = kindergarten.Email;
            vm.Description = kindergarten.Description;
            vm.CreatedAt = kindergarten.CreatedAt;
            vm.ModifiedAt = kindergarten.ModifiedAt;
            vm.Images.AddRange(photos);

            return View(vm);
        }

       
        [HttpPost]
        public async Task<IActionResult> RemoveImage(Guid imageId, Guid kindergartenId)
        {
            await _fileServices.RemoveImageFromDatabase(imageId);
            return RedirectToAction("Update", new { id = kindergartenId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSelectedImages(Guid[] selectedImages, Guid kindergartenId)
        {
            if (selectedImages != null && selectedImages.Length > 0)
            {
                await _fileServices.RemoveImagesFromDatabase(selectedImages);
            }
            return RedirectToAction("Update", new { id = kindergartenId });
        }

        private async Task<KindergartenImageViewModel[]> FilesFromDatabase(Guid id)
        {
            return await _context.FileToDatabase
                .Where(x => x.KindergartenId == id)
                .Select(y => new KindergartenImageViewModel
                {
                    KindergartenId = y.KindergartenId,
                    Id = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();
        }
    }
}