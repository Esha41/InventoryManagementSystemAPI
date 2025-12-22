
using System;
using Ettad.Comman.Enums;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Ettad.CrossCutting.Comman.FileUpload
{
    public interface IFileStorageService
    {
        Task<APIOperationResponse<string>> SaveFileAsync(
            IFormFile file,
            FileEntityType fileEntityType,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Low-level storage service that only saves the physical file and returns its relative URL.
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;

        public FileStorageService(IWebHostEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }

        public async Task<APIOperationResponse<string>> SaveFileAsync(
            IFormFile file,
            FileEntityType fileEntityType,
            CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
            {
                return APIOperationResponse<string>.BadRequest("File is empty.");
            }

            try
            {
                // Get upload path from appsettings.json
                var baseUploadPath = _configuration["FileSettings:UploadPath"];
                if (string.IsNullOrWhiteSpace(baseUploadPath))
                {
                    return APIOperationResponse<string>.BadRequest("File upload path is not configured. Please set FileSettings:UploadPath in appsettings.json");
                }

                // Generate file name as FileEntityType_datetime (e.g., Item_2024-01-15_14-30-45)
                var dateTime = DateTime.UtcNow.ToString("yyyy-MM-dd_HH-mm-ss");
                var fileNamePrefix = $"{fileEntityType}_{dateTime}";
                
                // Save files on the file server <UploadPath>\Uploads\{FileEntityType}
                var uploadsRoot = Path.Combine(baseUploadPath, "Uploads", fileEntityType.ToString());
                if (!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                // Generate file name: {FileType}_{date}_{guid}.{extension}
                var fileExtension = Path.GetExtension(file.FileName);
                var uniqueFileName = $"{fileNamePrefix}_{Guid.NewGuid()}{fileExtension}";
                var fullPath = Path.Combine(uploadsRoot, uniqueFileName);

                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                // return full path on file server (can be changed to a relative/virtual path if needed)
                var fullPathForResponse = uploadsRoot + Path.DirectorySeparatorChar + uniqueFileName;

                return APIOperationResponse<string>.Success(fullPathForResponse, "File uploaded successfully.");
            }
            catch (System.Exception ex)
            {
                return APIOperationResponse<string>.BadRequest($"File upload failed: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Contract for high-level file upload operations (DB + storage).
    /// Implemented in the API/service layer to keep this library free of repository dependencies.
    /// </summary>
    public interface IFileUploadService
    {
        Task<APIOperationResponse<long>> UploadAsync(
            IFormFile file,
            FileEntityType entity,
            long entityId,
            bool isMain,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a list of files to the server and creates FileUplodMaster records.
        /// Returns the list of created FileUplodMaster Ids.
        /// </summary>
        Task<APIOperationResponse<List<long>>> SaveFilesAsync(
            List<IFormFile> files,
            FileEntityType fileEntityType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a list of files and links them to a specific entity (creates details).
        /// Returns the list of created FileUplodDetails Ids.
        /// </summary>
        Task<APIOperationResponse<List<long>>> UploadFilesForEntityAsync(
            List<IFormFile> files,
            FileEntityType entity,
            long entityId,
            CancellationToken cancellationToken = default);

        Task<APIOperationResponse<List<FileUploadDto>>> GetByEntityAsync(FileEntityType entity, long entityId);
        
        /// <summary>
        /// Gets files for multiple entities in a single database query.
        /// Returns a dictionary mapping entityId to list of FileUploadDto.
        /// </summary>
        Task<APIOperationResponse<Dictionary<long, List<FileUploadDto>>>> GetByEntitiesAsync(FileEntityType entity, List<long> entityIds);
        
        Task<APIOperationResponse<FileUploadDto>> GetByIdAsync(long id);
        Task<APIOperationResponse<bool>> DeleteAsync(long id);
        Task<APIOperationResponse<bool>> SetMainAsync(long id);
    }

    public class FileUploadDto
    {
        public long Id { get; set; }
        public string FileUrl { get; set; }
        public string FileName { get; set; }
        public string OriginalName { get; set; }
        public bool IsMain { get; set; }
        public FileEntityType Entity { get; set; }
        public long EntityId { get; set; }
    }
}


