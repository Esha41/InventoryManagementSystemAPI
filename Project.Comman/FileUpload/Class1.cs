
using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Ettad.ResponseHandler.Models;

namespace Ettad.CrossCutting.Comman.FileUpload
{
        public interface IFileStorageService
        {
        Task<APIOperationResponse<string>> SaveFileAsync(
                IFormFile file,
                string modelName,
                CancellationToken cancellationToken = default);
    }

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
                var uploadsRoot = Path.Combine(_environment.WebRootPath ?? "wwwroot", "uploads", modelName, DateTime.UtcNow.ToString("yyyy-MM-dd"));
                if (!Directory.Exists(uploadsRoot))
                    Directory.CreateDirectory(uploadsRoot);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(uploadsRoot, fileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream, cancellationToken);
                }

                // optional: return relative path instead of absolute
                var relativePath = Path.Combine("uploads", modelName, DateTime.UtcNow.ToString("yyyy-MM-dd"), fileName)
                                   .Replace("\\", "/");

                return APIOperationResponse<string>.Success(relativePath, "File uploaded successfully.");
            }
            
            catch (System.Exception ex)
            {
                return APIOperationResponse<string>.BadRequest($"File upload failed: {ex.Message}");
            }
        }
    }
    }


