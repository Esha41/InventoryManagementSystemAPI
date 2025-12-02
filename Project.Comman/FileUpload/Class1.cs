
using System;
using Ettad.Comman.Enums;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Ettad.CrossCutting.Comman.FileUpload
{
    public interface IFileStorageService
    {
        Task<APIOperationResponse<string>> SaveFileAsync(
            IFormFile file,
            string modelName,
            CancellationToken cancellationToken = default);
    }

    /// <summary>
    /// Low-level storage service that only saves the physical file and returns its relative URL.
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public async Task<APIOperationResponse<string>> SaveFileAsync(
            IFormFile file,
            string modelName,
            CancellationToken cancellationToken = default)
        {
            if (file == null || file.Length == 0)
            {
                return APIOperationResponse<string>.BadRequest("File is empty.");
            }

            try
            {
                var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
                // Save files on the file server \\10.80.72.3\Uploads\<modelName>\<date>
                var uploadsRoot = Path.Combine(@"\\10.80.72.3\SDShare", modelName, today);
                if (!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(uploadsRoot, fileName);

                await using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                // return full path on file server (can be changed to a relative/virtual path if needed)
                var fullPathForResponse = uploadsRoot + Path.DirectorySeparatorChar + fileName;

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
            FileEntityType entityId,
            long primaryId,
            bool isMain,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a list of files to the server and creates FileUplodMaster records.
        /// Returns the list of created FileUplodMaster Ids.
        /// </summary>
        Task<APIOperationResponse<List<long>>> SaveFilesAsync(
            List<IFormFile> files,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Uploads a list of files and links them to a specific entity (creates details).
        /// Returns the list of created FileUplodDetails Ids.
        /// </summary>
        Task<APIOperationResponse<List<long>>> UploadFilesForEntityAsync(
            List<IFormFile> files,
            FileEntityType entityId,
            long primaryId,
            CancellationToken cancellationToken = default);

        Task<APIOperationResponse<List<FileUploadDto>>> GetByEntityAsync(FileEntityType entityId, long primaryId);
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
        public FileEntityType EntityId { get; set; }
        public long PrimaryId { get; set; }
    }
}

