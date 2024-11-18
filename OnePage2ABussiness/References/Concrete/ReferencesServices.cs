using Microsoft.AspNetCore.Http;
using OnePage2ABussiness.References.Abstract;
using OnePage2ABussiness.References.Models;
using OnePage2ACore.Abstract;
using OnePage2ADataAccess.Repositories.Abstract;

namespace OnePage2ABussiness.References.Concrete
{
    public class ReferencesServices : IReferencesServices
    {
        private readonly IRepository<OnePage2AEntity.Entites.References> _repository;
        private readonly IFileService _fileService;

        public ReferencesServices(IRepository<OnePage2AEntity.Entites.References> repository, IFileService fileService)
        {
            _repository = repository;
            _fileService = fileService;
        }

        public async Task AddReferencesAsync(AddReferencesModel referencesModel, IFormFile imageFile, string createdByName)
        {
            if (imageFile != null)
            {
                referencesModel.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/references");
            }

            var referencesEntity = new OnePage2AEntity.Entites.References
            {
                ImgUrl = referencesModel.ImgUrl,
                CreatedByName = createdByName,
                ReferemcesTitle = referencesModel.ReferemcesTitle,
                CreatedAt = DateTime.UtcNow,
                IsActive = referencesModel.IsActive
            };

            await _repository.AddAsync(referencesEntity);
        }

        public async Task UpdateReferencesAsync(UpdateReferencesModel referencesModel, IFormFile imageFile)
        {
            var existingReferences = await _repository.GetByIdAsync(referencesModel.Id);
            if (existingReferences == null)
            {
                throw new Exception("References not found");
            }
            existingReferences.ReferemcesTitle = referencesModel.ReferemcesTitle ?? existingReferences.ReferemcesTitle;
            existingReferences.IsActive = referencesModel.IsActive;

            if (imageFile != null)
            {
                if (!string.IsNullOrEmpty(existingReferences.ImgUrl))
                {
                    _fileService.DeleteFile(existingReferences.ImgUrl);
                }

                existingReferences.ImgUrl = await _fileService.SaveFileAsync(imageFile, "images/references");
            }

            await _repository.UpdateAsync(existingReferences);
        }

        public async Task SetActiveReferencesAsync(int selectedReferencesId)
        {
            var referencess = await _repository.GetAllAsync();
            foreach (var references in referencess)
            {
                references.IsActive = references.Id == selectedReferencesId;
                await _repository.UpdateAsync(references);
            }
        }
    }
}
