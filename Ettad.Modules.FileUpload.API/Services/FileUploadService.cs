using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ettad.Comman.Enums;
using Ettad.CrossCutting.Comman.FileUpload;
using Ettad.CrossCutting.Data.Repository;
using Ettad.Data.Entities;
using Ettad.ResponseHandler.Models;
using Microsoft.AspNetCore.Http;

namespace Ettad.Modules.FileUpload.API.Services
{
    /// <summary>
    /// High-level service that coordinates physical file storage with database tables
    /// FileUplodMaster and FileUplodDetails.
    /// </summary>
    public class FileUploadService : IFileUploadService
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ICrossCuttingRepository<FileUplodMaster> _masterRepository;
        private readonly ICrossCuttingRepository<FileUplodDetails> _detailsRepository;

        public FileUploadService(
            IFileStorageService fileStorageService,
            ICrossCuttingRepository<FileUplodMaster> masterRepository,
            ICrossCuttingRepository<FileUplodDetails> detailsRepository)
        {
            _fileStorageService = fileStorageService;
            _masterRepository = masterRepository;
            _detailsRepository = detailsRepository;
        }

        public async Task<APIOperationResponse<long>> UploadAsync(
            IFormFile file,
            FileEntityType entity,
            long entityId,
            bool isMain,
            CancellationToken cancellationToken = default)
        {
            var storageResult = await _fileStorageService.SaveFileAsync(file, entity, cancellationToken);
            if (!storageResult.Succeeded)
            {
                return APIOperationResponse<long>.BadRequest(storageResult.Message ?? "Failed to store file");
            }

            var master = new FileUplodMaster
            {
                FileUrl = storageResult.Data,
                FileName = System.IO.Path.GetFileName(storageResult.Data),
                OriginalName = file.FileName,
                IsMain = isMain
            };

            master = await _masterRepository.AddAsync(master);

            var detail = new FileUplodDetails
            {
                FileUplodMasterId = master.Id,
                Entity = entity,
                EntityId = entityId
            };

            await _detailsRepository.AddAsync(detail);

            return APIOperationResponse<long>.Success(master.Id, "File uploaded successfully");
        }

        /// <summary>
        /// Saves a list of files to the server and creates FileUplodMaster records.
        /// Returns the list of created FileUplodMaster Ids.
        /// </summary>
        public async Task<APIOperationResponse<List<long>>> SaveFilesAsync(
            List<IFormFile> files,
            FileEntityType fileEntityType,
            CancellationToken cancellationToken = default)
        {
            if (files == null || !files.Any())
            {
                return APIOperationResponse<List<long>>.BadRequest("No files provided.");
            }

            var masterIds = new List<long>();

            foreach (var file in files)
            {
                var storageResult = await _fileStorageService.SaveFileAsync(file, fileEntityType, cancellationToken);
                if (!storageResult.Succeeded)
                {
                    return APIOperationResponse<List<long>>.BadRequest(storageResult.Message ?? "Failed to store file");
                }

                var master = new FileUplodMaster
                {
                    FileUrl = storageResult.Data,
                    FileName = System.IO.Path.GetFileName(storageResult.Data),
                    OriginalName = file.FileName,
                    IsMain = false
                };

                master = await _masterRepository.AddAsync(master);
                masterIds.Add(master.Id);
            }

            return APIOperationResponse<List<long>>.Success(masterIds, "Files uploaded successfully");
        }

        /// <summary>
        /// Saves a list of files and links them to the specified entity (creates details).
        /// Returns the list of created FileUplodDetails Ids.
        /// </summary>
        public async Task<APIOperationResponse<List<long>>> UploadFilesForEntityAsync(
            List<IFormFile> files,
            FileEntityType entity,
            long entityId,
            CancellationToken cancellationToken = default)
        {
            var saveResult = await SaveFilesAsync(files, entity, cancellationToken);
            if (!saveResult.Succeeded || saveResult.Data == null)
            {
                return APIOperationResponse<List<long>>.BadRequest(saveResult.Message ?? "Failed to upload files");
            }

            var detailIds = new List<long>();

            foreach (var masterId in saveResult.Data)
            {
                var detail = new FileUplodDetails
                {
                    FileUplodMasterId = masterId,
                    Entity = entity,
                    EntityId = entityId
                };

                detail = await _detailsRepository.AddAsync(detail);
                detailIds.Add(detail.Id);
            }

            return APIOperationResponse<List<long>>.Success(detailIds, "Files uploaded and linked successfully");
        }

        public async Task<APIOperationResponse<List<FileUploadDto>>> GetByEntityAsync(FileEntityType entity, long entityId)
        {
            var details = await _detailsRepository.FindAsync(
                d => d.Entity == entity && d.EntityId == entityId,
                false,
                nameof(FileUplodDetails.FileUplodMaster));

            var result = details
                .Where(d => d.FileUplodMaster != null)
                .Select(d => new FileUploadDto
                {
                    Id = d.FileUplodMasterId,
                    FileUrl = d.FileUplodMaster.FileUrl,
                    FileName = d.FileUplodMaster.FileName,
                    OriginalName = d.FileUplodMaster.OriginalName,
                    IsMain = d.FileUplodMaster.IsMain,
                    Entity = d.Entity,
                    EntityId = d.EntityId
                })
                .ToList();

            return APIOperationResponse<List<FileUploadDto>>.Success(result);
        }

        /// <summary>
        /// Gets files for multiple entities in a single database query.
        /// Returns a dictionary mapping entityId to list of FileUploadDto.
        /// </summary>
        public async Task<APIOperationResponse<Dictionary<long, List<FileUploadDto>>>> GetByEntitiesAsync(FileEntityType entity, List<long> entityIds)
        {
            if (entityIds == null || !entityIds.Any())
            {
                return APIOperationResponse<Dictionary<long, List<FileUploadDto>>>.Success(new Dictionary<long, List<FileUploadDto>>());
            }

            var details = await _detailsRepository.FindAsync(
                d => d.Entity == entity && entityIds.Contains(d.EntityId),
                false,
                nameof(FileUplodDetails.FileUplodMaster));

            var result = details
                .Where(d => d.FileUplodMaster != null)
                .GroupBy(d => d.EntityId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(d => new FileUploadDto
                    {
                        Id = d.FileUplodMasterId,
                        FileUrl = d.FileUplodMaster.FileUrl,
                        FileName = d.FileUplodMaster.FileName,
                        OriginalName = d.FileUplodMaster.OriginalName,
                        IsMain = d.FileUplodMaster.IsMain,
                        Entity = d.Entity,
                        EntityId = d.EntityId
                    }).ToList()
                );

            // Ensure all entityIds are in the dictionary (even if they have no files)
            foreach (var entityId in entityIds)
            {
                if (!result.ContainsKey(entityId))
                {
                    result[entityId] = new List<FileUploadDto>();
                }
            }

            return APIOperationResponse<Dictionary<long, List<FileUploadDto>>>.Success(result);
        }

        public async Task<APIOperationResponse<FileUploadDto>> GetByIdAsync(long id)
        {
            var master = await _masterRepository.FindOneAsync(m => m.Id == id, false, nameof(FileUplodMaster.Details));
            if (master == null)
            {
                return APIOperationResponse<FileUploadDto>.NotFound("File not found");
            }

            var detail = master.Details.FirstOrDefault();

            var dto = new FileUploadDto
            {
                Id = master.Id,
                FileUrl = master.FileUrl,
                FileName = master.FileName,
                OriginalName = master.OriginalName,
                IsMain = master.IsMain,
                Entity = detail?.Entity ?? default,
                EntityId = detail?.EntityId ?? 0
            };

            return APIOperationResponse<FileUploadDto>.Success(dto);
        }

        public async Task<APIOperationResponse<bool>> DeleteAsync(long id)
        {
            var master = await _masterRepository.FindOneAsync(m => m.Id == id, false, nameof(FileUplodMaster.Details));
            if (master == null)
            {
                return APIOperationResponse<bool>.NotFound("File not found");
            }

            // Attempt to delete the physical file from disk if it exists
            try
            {
                var filePath = master.FileUrl;
                if (!string.IsNullOrWhiteSpace(filePath) && System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            catch
            {
                // Swallow errors here; continue deleting DB records to avoid leaving orphans
            }

            await _masterRepository.DeleteAsync(master);

            return APIOperationResponse<bool>.Success(true, "File deleted successfully");
        }

        public async Task<APIOperationResponse<bool>> SetMainAsync(long id)
        {
            var master = await _masterRepository.FindOneAsync(m => m.Id == id, false, nameof(FileUplodMaster.Details));
            if (master == null)
            {
                return APIOperationResponse<bool>.NotFound("File not found");
            }

            var detail = master.Details.FirstOrDefault();
            if (detail != null)
            {
                var siblings = await _detailsRepository.FindAsync(
                    d => d.Entity == detail.Entity &&
                         d.EntityId == detail.EntityId,
                    false,
                    nameof(FileUplodDetails.FileUplodMaster));

                foreach (var s in siblings.Where(s => s.FileUplodMaster != null))
                {
                    if (s.FileUplodMaster.Id == id)
                    {
                        s.FileUplodMaster.IsMain = true;
                    }
                    else
                    {
                        s.FileUplodMaster.IsMain = false;
                    }

                    await _masterRepository.UpdateAsync(s.FileUplodMaster);
                }
            }
            else
            {
                master.IsMain = true;
                await _masterRepository.UpdateAsync(master);
            }

            return APIOperationResponse<bool>.Success(true, "Main file updated successfully");
        }
    }
}

