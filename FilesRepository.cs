using AutoMapper;
using demsworldapi.Data;
using demsworldapi.Dtos;
using demsworldapi.Models;
using demsworldapi.Repository;
using demsworldapi.Service;
using demsworldapi.Services;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace demsworldapi.Repositories
{
    /// <summary>
    /// File Upload repository
    /// </summary>
    public class FilesRepository : GenericRepository<FileOnFileSystem>, IFilesRepository
    {
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public FilesRepository(ApplicationDbContext applicationDbContext, IMapper mapper, IWebHostEnvironment webHostEnvironment)
            :base(applicationDbContext)
        {
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// This will return all files uploaded to the file system
        /// </summary>
        /// <returns></returns>
        public async Task<(bool IsSuccess, IEnumerable<FileOutDTO> files, string Message)> GetAllFiles()
        {
            try
            {
                var result = await Get();
                var files = _mapper.Map<IEnumerable<FileOutDTO>>(result);
                if (files.Count() == 0) return (false, null, "No files found");
                else
                    return (true, files, "");
            }
            catch(Exception ex)
            {
                return (false, null, ex.Message);
            }
          
        }

        /// <summary>
        /// this returns a single file whose Id is provided
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, FileOutDTO file, string Message)> GetFileById(int id)
        {
            try
            {
                var result = await Get(id);
                var file = _mapper.Map<FileOutDTO>(result);
                if (file == null) return (false, null, "No file found with that Id");
                else
                    return (true, file, "");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }

        }

        /// <summary>
        /// This returns list of files by the file type
        /// </summary>
        /// <param name="fileType"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, IEnumerable<FileOutDTO> files, string Message)> GetFilesByType(string fileType)
        {
            try
            {
                var result = await Get();
                var f = _mapper.Map<IEnumerable<FileOutDTO>>(result);
                if (f.Count() == 0) 
                    return (false, null, "No files found");
                var files = f.Where(f => f.FileType == fileType).ToList();
                if (files.Count() == 0)
                    return (false, null, $"No files of {fileType} found");

                return (true, files, "");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }


        /// <summary>
        /// This is used to get files by the person that uploaded them
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, IEnumerable<FileOutDTO> files, string Message)> GetFilesByUploader(string email)
        {
            try
            {
                var result = await Get();
                var f = _mapper.Map<IEnumerable<FileOutDTO>>(result);//mapping file binding model to the out Dto
                if (f.Count() == 0)
                    return (false, null, "No files found");
                var files = f.Where(f => f.UploadedBy == email).ToList();
                if (files.Count() == 0)
                    return (false, null, $"No files uploaded by {email} found");

                return (true, files, "");
            }
            catch (Exception ex)
            {
                return (false, null, ex.Message);
            }
        }

        /// <summary>
        /// Saves file to the server filesstem
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message)> SaveFile(FileInDTO model)
        {
            try
            {
                if (model == null) return (false, "Please Supply file to save");
                if(model.Image==null) return (false, "Please Supply a valid image file to save");
                var guid = Guid.NewGuid();//generates new guid for the file name
                var fileName = Path.GetFileName(model.Image.FileName);// file name with extension
                var fileExtension = Path.GetExtension(model.Image.FileName);//the file extsion
                //the new file path will be something like C:\\webhostenvironment\\wwwroot\\directory\\filename.ext
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "SiteImges", $"{guid}-{fileName}");
                
                var file = new FileOnFileSystem
                {
                    CreatedOn = DateTime.Now.Date,
                    Description = model.Description,
                    Extension = fileExtension.Replace(".", ""),
                    FilesPath = filePath,
                    ImageUrl = filePath,
                    FileType = Path.GetExtension(model.Image.FileName).ToUpper(),
                    IsShown = model.IsShown,
                    UploadedBy = model.UploadedBy,
                    Name = fileName
                };
                var result = await Add(file);
                if (result)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    model.Image.CopyTo(fileStream);
                    return (true, "File saved successfully");
                }
               return (false, "File not saved");
            }
            catch (Exception ex)
            {
                return (false,  ex.Message);
            }
        }

        /// <summary>
        /// This isused to update the file ie make changes to the file.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<(bool IsSuccess, string Message)> UpdateFile(FileInUpdateDTO model)
        {
            try
            {
                if (model == null) return (false, "Please Supply file to save");
                if (model.Image == null) return (false, "Please Supply a valid image file to save");
                var file = await Get(model.Id);
                if(file == null) return (false, "The file does not exist");

                var guid = Guid.NewGuid();
                var fileName = Path.GetFileName(model.Image.FileName);
                var fileExtension = Path.GetExtension(model.Image.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "SiteImges", $"{guid}-{fileName}");

                file.CreatedOn = DateTime.Now.Date;
                file.Description = model.Description;
                file.Extension = fileExtension.Replace(".", "");
                file.FilesPath = filePath;
                file.ImageUrl = filePath;
                file.FileType = Path.GetExtension(model.Image.FileName).ToUpper();
                file.IsShown = model.IsShown;
                file.UploadedBy = model.UploadedBy;
                file.Name = filePath;
                
                var result = await Update(file);
                if (result)
                {
                    var fileStream = new FileStream(filePath, FileMode.Create);
                    model.Image.CopyTo(fileStream);
                    return (true, "File updated successfully");
                }
                return (false, "File not updated");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

       
    }
}
