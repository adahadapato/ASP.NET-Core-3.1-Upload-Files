using demsworldapi.Dtos;
using demsworldapi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace demsworldapi.Controllers
{
    /// <summary>
    /// Files Controller, Used tupload and download fils
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFilesRepository _filesRepository;
        public FilesController(IFilesRepository filesRepository)
        {
            _filesRepository = filesRepository;
        }

        /// <summary>
        /// Gets all files from the server
        /// </summary>
        /// <returns></returns>
        [HttpGet("getfiles")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<FileOutDTO>))]
        [ProducesResponseType(400, Type =typeof(string))]
        public async Task<IActionResult> GetFiles()
        {
            var result = await _filesRepository.GetAllFiles();
            if (result.IsSuccess)
                return Ok(result.files);
            else
                return BadRequest(new { Message = result.Message });
        }


        /// <summary>
        /// Gets a single file from the server
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("getfile/{id}")]
        [ProducesResponseType(200, Type = typeof(FileOutDTO))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> GetFile(int id)
        {
            var result = await _filesRepository.GetFileById(id);
            if (result.IsSuccess)
                return Ok(result.file);
            else
                return BadRequest(new { Message = result.Message });
        }


        /// <summary>
        /// Saves the file to the server
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("savefile")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Post([FromForm] FileInDTO model)
        {
            var result = await _filesRepository.SaveFile(model);
            if (result.IsSuccess)
                return Ok(new { Message = result.Message });
            else
                return BadRequest(new { Message = result.Message });
        }


        /// <summary>
        /// Updates file information
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPatch("updatefile")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Patch([FromForm] FileInUpdateDTO model)
        {
            var result = await _filesRepository.UpdateFile(model);
            if (result.IsSuccess)
                return Ok(new { Message = result.Message });
            else
                return BadRequest(new { Message = result.Message });
        }

        //[HttpGet("getfromdb")]
        //private async Task<FileUploadViewModel> GetAllFilesFromDb()
        //{
        //    var viewModel = new FileUploadViewModel();
        //    viewModel.FilesOnDatabase = await context.FilesOnDatabase.ToListAsync();
        //    viewModel.FilesOnFileSystem = await context.FilesOnFileSystem.ToListAsync();
        //    return viewModel;
        //}

        //// GET api/<FilesController>/5
        //[HttpGet("downloadfromfs/{id}")]
        //public async Task<IActionResult> DownloadFileFromFileSystem(string id)
        //{
        //    var file = await context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
        //    if (file == null) return null;
        //    var memory = new MemoryStream();
        //    using (var stream = new FileStream(file.FilePath, FileMode.Open))
        //    {
        //        await stream.CopyToAsync(memory);
        //    }
        //    memory.Position = 0;
        //    return File(memory, file.FileType, file.Name + file.Extension);
        //}

        //[HttpGet("downloadfromdb/{id}")]
        //public async Task<IActionResult> DownloadFileFromDatabase(string id)
        //{
        //    var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
        //    if (file == null) return null;
        //    return File(file.Data, file.FileType, file.Name + file.Extension);
        //}
        //// POST api/<FilesController>
        //[HttpPost("uploadtodb")]
        //public async Task<IActionResult> UploadToDatabase(List<IFormFile> files, string description)
        //{
        //    foreach (var file in files)
        //    {
        //        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //        var extension = Path.GetExtension(file.FileName);
        //        var fileModel = new FileOnDatabaseModel
        //        {
        //            CreatedOn = DateTime.UtcNow,
        //            FileType = file.ContentType,
        //            Extension = extension,
        //            Name = fileName,
        //            Description = description
        //        };
        //        using (var dataStream = new MemoryStream())
        //        {
        //            await file.CopyToAsync(dataStream);
        //            fileModel.Data = dataStream.ToArray();
        //        }
        //        context.FilesOnDatabase.Add(fileModel);
        //        context.SaveChanges();
        //    }
        //    TempData["Message"] = "File successfully uploaded to Database";
        //    return RedirectToAction("Index");
        //}


        //[HttpPost("uploadtofs")]
        //public async Task<IActionResult> UploadToFileSystem(List<IFormFile> files, string description)
        //{
        //    foreach (var file in files)
        //    {
        //        var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\Files\\");
        //        bool basePathExists = System.IO.Directory.Exists(basePath);
        //        if (!basePathExists) Directory.CreateDirectory(basePath);
        //        var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //        var filePath = Path.Combine(basePath, file.FileName);
        //        var extension = Path.GetExtension(file.FileName);
        //        if (!System.IO.File.Exists(filePath))
        //        {
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await file.CopyToAsync(stream);
        //            }
        //            var fileModel = new FileOnFileSystemModel
        //            {
        //                CreatedOn = DateTime.UtcNow,
        //                FileType = file.ContentType,
        //                Extension = extension,
        //                Name = fileName,
        //                Description = description,
        //                FilePath = filePath
        //            };
        //            context.FilesOnFileSystem.Add(fileModel);
        //            context.SaveChanges();
        //        }
        //    }
        //    TempData["Message"] = "File successfully uploaded to File System.";
        //    return RedirectToAction("Index");
        //}

        //// PUT api/<FilesController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<FilesController>/5
        //[HttpDelete("deletefromfs/{id}")]
        //public async Task<IActionResult> DeleteFileFromFileSystem(string id)
        //{
        //    var file = await context.FilesOnFileSystem.Where(x => x.Id == id).FirstOrDefaultAsync();
        //    if (file == null) return null;
        //    if (System.IO.File.Exists(file.FilePath))
        //    {
        //        System.IO.File.Delete(file.FilePath);
        //    }
        //    context.FilesOnFileSystem.Remove(file);
        //    context.SaveChanges();
        //    TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from File System.";
        //    return RedirectToAction("Index");
        //}

        //[HttpDelete("deletefromdb/{id}")]
        //public async Task<IActionResult> DeleteFileFromDatabase(string id)
        //{
        //    var file = await context.FilesOnDatabase.Where(x => x.Id == id).FirstOrDefaultAsync();
        //    context.FilesOnDatabase.Remove(file);
        //    context.SaveChanges();
        //    TempData["Message"] = $"Removed {file.Name + file.Extension} successfully from Database.";
        //    return RedirectToAction("Index");
        //}
    }
}
