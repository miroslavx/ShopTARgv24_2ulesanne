using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopTARgv24.ApplicationServices.Services;
using ShopTARgv24.Core.Dto;
using ShopTARgv24.Core.ServiceInterface;
using ShopTARgv24.Data;
using ShopTARgv24.Models.RealEstates;
using ShopTARgv24.Models.Spaceships;

namespace ShopTARgv24.Controllers
{
    public class RealEstateController : Controller
    {
        private readonly ShopTARgv24Context _context;
        private readonly IRealEstateServices _realEstateServices;
        private readonly IFileServices _fileServices;

        public RealEstateController
            (
                ShopTARgv24Context context,
                IRealEstateServices realEstateServices,
                IFileServices fileServices
            )
        {
            _context = context;
            _realEstateServices = realEstateServices;
            _fileServices = fileServices;
        }

        public IActionResult Index()
        {
            var result = _context.RealEstates
                .Select(x => new RealEstatesIndexViewModel
                {
                    Id = x.Id,
                    Area = x.Area,
                    BuildingType = x.BuildingType,
                    RoomNumber = x.RoomNumber,
                    Location = x.Location  
                })
                .ToList(); 
            return View(result);
        }

        [HttpGet]
        public IActionResult Create()
        {
            RealEstateCreateUpdateViewModel result = new();

            return View("CreateUpdate", result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                BuildingType = vm.BuildingType,
                RoomNumber = vm.RoomNumber,
                Location = vm.Location,
                CreateAt = vm.CreateAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files,
                Image = vm.Image
                    .Select(x => new FileToDatabaseDto
                    {
                        Id = x.Id,
                        ImageData = x.ImageData,
                        ImageTitle = x.ImageTitle,
                        RealEstateId = x.RealEstateId,
                    }).ToArray()
            };

            var result = await _realEstateServices.Create(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var realEstate = await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            RealEstateImageViewModel[] photos = await FilesFromDatabase(id);

            var vm = new RealEstateCreateUpdateViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.BuildingType = realEstate.BuildingType;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.Location = realEstate.Location;
            vm.CreateAt = realEstate.CreateAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(photos);

            return View("CreateUpdate", vm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(RealEstateCreateUpdateViewModel vm)
        {
            var dto = new RealEstateDto()
            {
                Id = vm.Id,
                Area = vm.Area,
                BuildingType = vm.BuildingType,
                RoomNumber = vm.RoomNumber,
                Location = vm.Location,
                CreateAt = vm.CreateAt,
                ModifiedAt = vm.ModifiedAt,
                Files = vm.Files
            };

            var result = await _realEstateServices.Update(dto);

            if (result == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var realEstate = await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            RealEstateImageViewModel[] photos = await FilesFromDatabase(id);

            var vm = new RealEstateDeleteViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.BuildingType = realEstate.BuildingType;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.Location = realEstate.Location;
            vm.CreateAt = realEstate.CreateAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(photos);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmation(Guid id)
        {
            var realEstate = await _realEstateServices.Delete(id);

            if (realEstate == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var realEstate = await _realEstateServices.DetailAsync(id);

            if (realEstate == null)
            {
                return NotFound();
            }

            RealEstateImageViewModel[] photos = await FilesFromDatabase(id);

            var vm = new RealEstateDetailsViewModel();

            vm.Id = realEstate.Id;
            vm.Area = realEstate.Area;
            vm.BuildingType = realEstate.BuildingType;
            vm.RoomNumber = realEstate.RoomNumber;
            vm.Location = realEstate.Location;
            vm.CreateAt = realEstate.CreateAt;
            vm.ModifiedAt = realEstate.ModifiedAt;
            vm.Image.AddRange(photos);

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveImage(Guid imageId, Guid realEstateId)
        {
            await _fileServices.RemoveImageFromDatabase(imageId);
            return RedirectToAction("Update", new { id = realEstateId });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveSelectedImages(Guid[] selectedImages, Guid realEstateId)
        {
            if (selectedImages != null && selectedImages.Length > 0)
            {
                await _fileServices.RemoveImagesFromDatabase(selectedImages);
            }
            return RedirectToAction("Update", new { id = realEstateId });
        }

        private async Task<RealEstateImageViewModel[]> FilesFromDatabase(Guid id)
        {
            return await _context.FileToDatabase
                .Where(x => x.RealEstateId == id)
                .Select(y => new RealEstateImageViewModel
                {
                    RealEstateId = y.RealEstateId,
                    Id = y.Id,
                    ImageData = y.ImageData,
                    ImageTitle = y.ImageTitle,
                    Image = string.Format("data:image/gif;base64, {0}", Convert.ToBase64String(y.ImageData))
                }).ToArrayAsync();
        }
    }
}