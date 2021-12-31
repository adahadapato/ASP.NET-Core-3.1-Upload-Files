using demsworld.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace demsworld.Services
{
    public interface IFilesRepository
    {
        Task<(bool IsSuccess, IEnumerable<FilesViewModel> files, string Message)> GetAllFiles();
        Task<(bool IsSuccess, FilesViewModel file, string Message)> GetFile(int Id);
        Task<(bool IsSuccess, string Message)> Create(FilesViewModel model);
    }
}
