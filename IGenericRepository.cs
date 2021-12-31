using demsworld.Infrastructure;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace demsworld.Services
{
    public interface IGenericRepository
    {
        Task<HttpResponseMessage> GetAsync(string url, string token);
        //Task<HttpResponseMessage> GetAsync(string url, string token);
        //Task<HttpResponseMessage> GetAsync(string url, string token);
        Task<HttpResponseMessage> PostAsync<T>(string url, T objToCreate, string token) where T: class;
        Task<HttpResponseMessage> PostFormContentAsync<T>(string url, T objToCreate, string token) where T : class;
        Task<HttpResponseMessage> UpdateAsync<T>(string url, T objToUpdate, string token) where T: class;
        Task<HttpResponseMessage> DeleteAsync(string url, string token);
        //Task<HttpResponseMessage> DeleteAsync(string url, string Id, string token);
        Task<TResponse> CreateJsonResponse<TResponse>(HttpResponseMessage response)
            where TResponse : ApiResponse, new();
    }
}
