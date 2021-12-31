using demsworldapi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace demsworldapi.Services
{
    public interface IFilesRepository
    {
        Task<(bool IsSuccess, IEnumerable<FileOutDTO> files, string Message)> GetAllFiles();
        Task<(bool IsSuccess, IEnumerable<FileOutDTO> files, string Message)> GetFilesByType(string fileType);
        Task<(bool IsSuccess, IEnumerable<FileOutDTO> files, string Message)> GetFilesByUploader(string email);
        Task<(bool IsSuccess, FileOutDTO file, string Message)> GetFileById(int id);
        Task<(bool IsSuccess, string Message)> SaveFile(FileInDTO model);
        Task<(bool IsSuccess, string Message)> UpdateFile(FileInUpdateDTO model);
    }
}
