using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.AboutUs.Abstract;
using OnePage2ABussiness.AboutUs.Models;
using OnePage2ACore.Abstract;
using OnePage2ADataAccess.Repositories.Abstract;

namespace OnePage2ABussiness.AboutUs.Concrete
{
    public class AboutUsServices : IAboutUsServices
    {
        private readonly IRepository<OnePage2AEntity.Entites.AboutUs> _repository;
        private readonly IFileService _fileService;

        public AboutUsServices(IRepository<OnePage2AEntity.Entites.AboutUs> repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task AddAboutUsAsync(AddAboutUsModel aboutUsModel, IFormFile imageFile, string createdByName)
        {
            if (imageFile != null)
            {
                aboutUsModel.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/aboutUs");
            }

            var aboutUsEntity = new OnePage2AEntity.Entites.AboutUs
            {
                AboutUsDescription = aboutUsModel.AboutUsDescription,
                ImgUrl = aboutUsModel.ImgUrl,
                CreatedByName = createdByName,
                CreatedAt = DateTime.UtcNow,
                IsActive = aboutUsModel.IsActive
            };

            await _repository.AddAsync(aboutUsEntity);
        }

        public async Task UpdateAboutUsAsync(UpdateAboutUsModel aboutUsModel, IFormFile imageFile)
        {
            var existingAboutUs = await _repository.GetByIdAsync(aboutUsModel.Id);
            if (existingAboutUs == null)
            {
                throw new Exception("AboutUs not found");
            }

            existingAboutUs.AboutUsDescription = aboutUsModel.AboutUsDescription ?? existingAboutUs.AboutUsDescription;
            existingAboutUs.IsActive = aboutUsModel.IsActive;

            if (imageFile != null)
            {
                if (!string.IsNullOrEmpty(existingAboutUs.ImgUrl))
                {
                    _fileService.DeleteFile(existingAboutUs.ImgUrl);
                }

                existingAboutUs.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/aboutUss");
            }

            await _repository.UpdateAsync(existingAboutUs);
        }

        public async Task SetActiveAboutUsAsync(int selectedAboutUsId)
        {
            var aboutUss = await _repository.GetAllAsync();
            foreach (var aboutUs in aboutUss)
            {
                aboutUs.IsActive = aboutUs.Id == selectedAboutUsId;
                await _repository.UpdateAsync(aboutUs);
            }
        }
    }
}
