using AutoMapper;
using demsworld.Infrastructure;
using demsworld.Models;
using demsworld.Models.Dtos;
using demsworld.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace demsworld.Repositories
{
    /// <summary>
    /// Cleint side files repository
    /// </summary>
    public class FilesRepository : GenericRepository, IFilesRepository
    {
        private readonly ITokenContainer _tokenContainer;
        private readonly IMapper _mapper;
        public FilesRepository(IHttpClientFactory httpClientFactory, ITokenContainer tokenContainer, IMapper mapper)
            :base(httpClientFactory)
        {
            _tokenContainer = tokenContainer;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates upload file to the server
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message)> Create(FilesViewModel model)
        {
            if (model == null) return (false, "Please supply file to save");
            var file = _mapper.Map<FileInDto>(model);
            var response = await PostFormContentAsync<FileInDto>(FilesRepositoryHelper.SaveFile, file, _tokenContainer.ApiToken);
            if (response != null)
            {
                var result = await CreateJsonResponse<GeneralResponse>(response);
                if (result.StatusIsSuccessful)
                    return (true, result.ResponseResult);
                else
                    return (false, result.ResponseResult);
            }
            return (false, "Error occured");
        }

        public async Task<(bool IsSuccess, IEnumerable<FilesViewModel> files, string Message)> GetAllFiles()
        {
            var response = await GetAsync(FilesRepositoryHelper.GetAllFiles, _tokenContainer.ApiToken);
            var result = await CreateJsonResponse<HttpResponse>(response);
            if (result.StatusIsSuccessful)
            {
                var data = await Decode<IEnumerable<FileOutDto>>(result.ResponseResult);
                var _files = _mapper.Map<IEnumerable<FilesViewModel>>(data);
                return (true, _files, "");

            }
            return (false, null, result.ResponseResult);
        }

        public async Task<(bool IsSuccess, FilesViewModel file, string Message)> GetFile(int Id)
        {
            var response = await GetAsync($"{FilesRepositoryHelper.GetFile}/{Id}", _tokenContainer.ApiToken);
            var result = await CreateJsonResponse<HttpResponse>(response);
            if (result.StatusIsSuccessful)
            {
                var data = await Decode<FileOutDto>(result.ResponseResult);
                var _file = _mapper.Map<FilesViewModel>(data);
                return (true, _file, "");

            }
            return (false, null, result.ResponseResult);
        }
    }

    /// <summary>
    /// the endpoints for the file
    /// </summary>
    public static class FilesRepositoryHelper
    {
        public static string SaveFile
        => $"{HttpClientInstance.BaseAddressInstance}Files/savefile";
        public static string GetAllFiles
          => $"{HttpClientInstance.BaseAddressInstance}Files/getfiles";

        public static string GetFile
         => $"{HttpClientInstance.BaseAddressInstance}Files/getfile";
    }
}
