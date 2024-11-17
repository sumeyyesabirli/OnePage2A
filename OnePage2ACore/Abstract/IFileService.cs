using Microsoft.AspNetCore.Http;

namespace OnePage2ACore.Abstract
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(IFormFile file, string folderName);
        void DeleteFile(string filePath);
    }

}
