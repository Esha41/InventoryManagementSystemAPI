using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace Ettad.CrossCutting.Comman.FileUpload
{
    /// <summary>
    /// Helper class for validating file uploads.
    /// Implements defense-in-depth validation using file extensions, MIME types, and file signatures (magic bytes).
    /// </summary>
    public static class FileValidationHelper
    {
        /// <summary>
        /// Allowed file extensions (case-insensitive)
        /// </summary>
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".xlsx",
            ".docx",
            ".jpeg",
            ".jpg",
            ".png"
        };

        /// <summary>
        /// Allowed MIME types
        /// </summary>
        private static readonly HashSet<string> AllowedMimeTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "application/pdf",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", // XLSX
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document", // DOCX
            "image/jpeg",
            "image/jpg",
            "image/png"
        };

        /// <summary>
        /// File signatures (magic bytes) for each allowed file type.
        /// Key: file extension, Value: array of possible magic byte sequences
        /// </summary>
        private static readonly Dictionary<string, byte[][]> FileSignatures = new Dictionary<string, byte[][]>(StringComparer.OrdinalIgnoreCase)
        {
            [".pdf"] = new[] { new byte[] { 0x25, 0x50, 0x44, 0x46 } }, // %PDF
            [".xlsx"] = new[] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } }, // ZIP signature (XLSX is a ZIP file)
            [".docx"] = new[] { new byte[] { 0x50, 0x4B, 0x03, 0x04 } }, // ZIP signature (DOCX is a ZIP file)
            [".jpg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
            [".jpeg"] = new[] { new byte[] { 0xFF, 0xD8, 0xFF } },
            [".png"] = new[] { new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A } }
        };

        /// <summary>
        /// Default maximum file size in bytes (30 MB) - used if not provided
        /// </summary>
        private const long DefaultMaxFileSizeBytes = 30 * 1024 * 1024;

        /// <summary>
        /// Validates a file upload.
        /// </summary>
        /// <param name="file">The file to validate</param>
        /// <param name="maxFileSizeBytes">Maximum allowed file size in bytes. If not provided, defaults to 30 MB.</param>
        /// <returns>Validation result with success status and error message</returns>
        public static FileValidationResult ValidateFile(IFormFile file, long? maxFileSizeBytes = null)
        {
            if (file == null)
            {
                return FileValidationResult.Failure("No file provided.");
            }

            if (file.Length == 0)
            {
                return FileValidationResult.Failure($"File \"{file.FileName}\" is empty. Please select a valid file.");
            }

            // Validate file extension FIRST (before checking size - reject invalid types early)
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrWhiteSpace(extension))
            {
                return FileValidationResult.Failure($"File \"{file.FileName}\" does not have a file extension. Only the following file types are allowed: PDF, XLSX, DOCX, JPEG, JPG, PNG.");
            }

            if (!AllowedExtensions.Contains(extension))
            {
                return FileValidationResult.Failure($"File \"{file.FileName}\" has an invalid file type (extension: {extension}). Only the following file types are allowed: PDF, XLSX, DOCX, JPEG, JPG, PNG.");
            }

            // Validate MIME type if provided
            if (!string.IsNullOrWhiteSpace(file.ContentType) && !AllowedMimeTypes.Contains(file.ContentType))
            {
                return FileValidationResult.Failure($"File \"{file.FileName}\" has an invalid file type (MIME type: {file.ContentType}). Only the following file types are allowed: PDF, XLSX, DOCX, JPEG, JPG, PNG.");
            }

            // Validate file size (after type validation - only check size for valid file types)
            var maxSize = maxFileSizeBytes ?? DefaultMaxFileSizeBytes;
            if (file.Length > maxSize)
            {
                var fileSizeMB = (file.Length / (1024.0 * 1024.0)).ToString("F2");
                var maxSizeMB = (maxSize / (1024.0 * 1024.0)).ToString("F0");
                return FileValidationResult.Failure($"File \"{file.FileName}\" size ({fileSizeMB} MB) exceeds the maximum allowed size of {maxSizeMB} MB.");
            }

            // Validate file signature (magic bytes) for additional security
            var signatureValidation = ValidateFileSignature(file, extension);
            if (!signatureValidation.IsValid)
            {
                return signatureValidation;
            }

            return FileValidationResult.Success();
        }

        /// <summary>
        /// Validates multiple files.
        /// </summary>
        /// <param name="files">The files to validate</param>
        /// <param name="maxFileSizeBytes">Maximum allowed file size in bytes. If not provided, defaults to 30 MB.</param>
        /// <returns>Validation result with success status and error message</returns>
        public static FileValidationResult ValidateFiles(IEnumerable<IFormFile> files, long? maxFileSizeBytes = null)
        {
            if (files == null)
            {
                return FileValidationResult.Failure("No files provided.");
            }

            var fileList = files.ToList();
            if (fileList.Count == 0)
            {
                return FileValidationResult.Failure("No files provided.");
            }

            foreach (var file in fileList)
            {
                var result = ValidateFile(file, maxFileSizeBytes);
                if (!result.IsValid)
                {
                    return result;
                }
            }

            return FileValidationResult.Success();
        }

        /// <summary>
        /// Validates file signature (magic bytes) to ensure the file content matches its extension.
        /// This provides additional security against file type spoofing.
        /// </summary>
        private static FileValidationResult ValidateFileSignature(IFormFile file, string extension)
        {
            if (!FileSignatures.TryGetValue(extension, out var expectedSignatures))
            {
                // If we don't have a signature for this extension, skip validation
                // (shouldn't happen if extension validation passed, but be defensive)
                return FileValidationResult.Success();
            }

            try
            {
                // Read the first bytes of the file to check the signature
                using var stream = file.OpenReadStream();
                var buffer = new byte[Math.Min(8, file.Length)]; // Read up to 8 bytes (enough for all our signatures)
                var bytesRead = stream.Read(buffer, 0, buffer.Length);

                if (bytesRead < buffer.Length)
                {
                    // File is too small to validate signature
                    return FileValidationResult.Failure($"File \"{file.FileName}\" is too small or corrupted. Please ensure the file is complete and not damaged.");
                }

                // Check if the file signature matches any of the expected signatures for this extension
                var matches = expectedSignatures.Any(signature =>
                {
                    if (bytesRead < signature.Length)
                        return false;

                    return signature.SequenceEqual(buffer.Take(signature.Length));
                });

                if (!matches)
                {
                    // Special handling for XLSX and DOCX - both are ZIP files, so we need additional validation
                    if (extension.Equals(".xlsx", StringComparison.OrdinalIgnoreCase) ||
                        extension.Equals(".docx", StringComparison.OrdinalIgnoreCase))
                    {
                        // For Office Open XML files, we check for ZIP signature and then verify internal structure
                        // This is a simplified check - in production, you might want more thorough validation
                        if (buffer[0] == 0x50 && buffer[1] == 0x4B && buffer[2] == 0x03 && buffer[3] == 0x04)
                        {
                            // ZIP signature found - this could be XLSX, DOCX, or another ZIP file
                            // We'll accept it since extension validation already passed
                            return FileValidationResult.Success();
                        }
                    }

                    var extensionUpper = extension.TrimStart('.').ToUpperInvariant();
                    return FileValidationResult.Failure($"File \"{file.FileName}\" does not match the expected {extensionUpper} file format. The file may be corrupted, has an incorrect extension, or is not a valid {extensionUpper} file. Please ensure the file is a valid {extensionUpper} file.");
                }

                return FileValidationResult.Success();
            }
            catch (System.Exception ex)
            {
                // If we can't read the file, fail validation
                return FileValidationResult.Failure($"Unable to validate file signature: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Result of file validation
    /// </summary>
    public class FileValidationResult
    {
        public bool IsValid { get; private set; }
        public string ErrorMessage { get; private set; }

        private FileValidationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }

        public static FileValidationResult Success() => new FileValidationResult(true, string.Empty);
        public static FileValidationResult Failure(string errorMessage) => new FileValidationResult(false, errorMessage);
    }
}

