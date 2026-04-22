using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Net;

namespace Ettad.Modules.FileUpload.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileUploadController : ApiControllerBase
    {
        private readonly IFileUploadService _fileUploadService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FileUploadController> _logger;

        public FileUploadController(
            IFileUploadService fileUploadService, 
            IConfiguration configuration,
            ILogger<FileUploadController> logger)
        {
            _fileUploadService = fileUploadService;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Gets all files for a given entity type and primary id.
        /// </summary>
        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByEntity([FromQuery] FileEntityType entity, [FromQuery] long entityId)
        {
            var result = await _fileUploadService.GetByEntityAsync(entity, entityId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Gets a single file record by its master id.
        /// </summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetById(long id)
        {
            var result = await _fileUploadService.GetByIdAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Deletes a file (master and its details).
        /// </summary>
        [HttpDelete("{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<IActionResult> Delete(long id)
        {
            var result = await _fileUploadService.DeleteAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Marks this file as the main file for its entity.
        /// </summary>
        [HttpPut("{id:long}/set-main")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> SetMain(long id)
        {
            var result = await _fileUploadService.SetMainAsync(id);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Uploads a single file and links it to an entity.
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadFile(
            IFormFile file,
            [FromQuery] FileEntityType entity,
            [FromQuery] long entityId,
            [FromQuery] bool isMain = false)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file provided");
            }

            var result = await _fileUploadService.UploadAsync(file, entity, entityId, isMain);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Uploads multiple files and links them to an entity.
        /// </summary>
        [HttpPost("upload-for-entity")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UploadFilesForEntity(
            List<IFormFile> files,
            [FromQuery] FileEntityType entity,
            [FromQuery] long entityId)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files provided");
            }

            var result = await _fileUploadService.UploadFilesForEntityAsync(files, entity, entityId);
            return ProcessResponse(result);
        }

        /// <summary>
        /// Serves a file from the file server by file ID.
        /// This endpoint reads files from the network share and serves them to the browser.
        /// </summary>
        [HttpGet("serve/{id:long}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> ServeFile(long id)
        {
            _logger.LogInformation("ServeFile endpoint called with ID: {FileId}", id);
            
            var fileResult = await _fileUploadService.GetByIdAsync(id);
            if (!fileResult.Succeeded || fileResult.Data == null)
            {
                _logger.LogWarning("File record not found in database for ID: {FileId}", id);
                return NotFound("File not found");
            }

            var fileUrl = fileResult.Data.FileUrl;
            if (string.IsNullOrWhiteSpace(fileUrl))
            {
                _logger.LogWarning("File {FileId} has empty file URL in database", id);
                return NotFound("File not found on server");
            }

            _logger.LogInformation("File {FileId} found in database. FileUrl: {FileUrl}", id, fileUrl);

            if (!System.IO.File.Exists(fileUrl))
            {
                _logger.LogWarning("File not found at path: {FilePath} (FileId: {FileId}). UploadPath config: {UploadPath}", 
                    fileUrl, id, _configuration["FileSettings:UploadPath"]);
                return NotFound("File not found on server");
            }

            _logger.LogInformation("File exists at path: {FilePath}. Proceeding to read file.", fileUrl);

            try
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(fileUrl);
                var contentType = GetContentType(fileUrl);
                return File(fileBytes, contentType, fileResult.Data.OriginalName ?? fileResult.Data.FileName);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "NO READ ACCESS: Cannot read file at path: {FilePath} (FileId: {FileId}). " +
                    "The service account does not have read permissions to access the file server. " +
                    "Please ensure the service account has read access to: {UploadPath}",
                    fileUrl, id, _configuration["FileSettings:UploadPath"]);
                return NotFound("File access denied - check service account permissions");
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.LogError(ex, "Directory not found for file path: {FilePath} (FileId: {FileId})", fileUrl, id);
                return NotFound("File directory not found");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "IO error reading file at path: {FilePath} (FileId: {FileId})", fileUrl, id);
                return NotFound("Error reading file");
            }
        }

        /// <summary>
        /// Serves a file from the file server by file path (relative to UploadPath).
        /// This endpoint reads files from the network share and serves them to the browser.
        /// </summary>
        [HttpGet("serve")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ServeFileByPath([FromQuery] string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return BadRequest("Path parameter is required");
            }

            var baseUploadPath = _configuration["FileSettings:UploadPath"];
            if (string.IsNullOrWhiteSpace(baseUploadPath))
            {
                _logger.LogError("File upload path is not configured in appsettings.json");
                return BadRequest("File upload path is not configured");
            }

            // Remove leading slashes/backslashes from path
            path = path.TrimStart('\\', '/');

            // Construct full path
            var fullPath = Path.Combine(baseUploadPath, path);

            // Security: Ensure the path is within the base upload path
            var basePath = Path.GetFullPath(baseUploadPath);
            var resolvedPath = Path.GetFullPath(fullPath);
            if (!resolvedPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogWarning("Invalid file path attempt: {ResolvedPath} is not within base path: {BasePath}", resolvedPath, basePath);
                return BadRequest("Invalid file path");
            }

            if (!System.IO.File.Exists(resolvedPath))
            {
                _logger.LogWarning("File not found at path: {FilePath}", resolvedPath);
                return NotFound("File not found");
            }

            try
            {
                var fileBytes = await System.IO.File.ReadAllBytesAsync(resolvedPath);
                var fileName = Path.GetFileName(resolvedPath);
                var contentType = GetContentType(resolvedPath);
                return File(fileBytes, contentType, fileName);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogError(ex, "NO READ ACCESS: Cannot read file at path: {FilePath}. " +
                    "The service account does not have read permissions to access the file server. " +
                    "Please ensure the service account has read access to: {UploadPath}",
                    resolvedPath, baseUploadPath);
                return NotFound("File access denied - check service account permissions");
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.LogError(ex, "Directory not found for file path: {FilePath}", resolvedPath);
                return NotFound("File directory not found");
            }
            catch (IOException ex)
            {
                _logger.LogError(ex, "IO error reading file at path: {FilePath}", resolvedPath);
                return NotFound("Error reading file");
            }
        }

        private string GetContentType(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLowerInvariant();
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                _ => "application/octet-stream"
            };
        }
    }
}

