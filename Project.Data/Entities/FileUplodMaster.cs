using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Master record for uploaded files. Stores the physical file information.
    /// </summary>
    public class FileUplodMaster : BaseEntity<long>
    {
        /// <summary>
        /// Relative or absolute URL to access the file.
        /// </summary>
        public string FileUrl { get; set; }

        /// <summary>
        /// Generated file name stored on disk.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Original file name as uploaded by the user.
        /// </summary>
        public string OriginalName { get; set; }

        /// <summary>
        /// Indicates if this file is the main file for the entity.
        /// </summary>
        public bool IsMain { get; set; } = false;

        #region Navigation Properties

        public ICollection<FileUplodDetails> Details { get; set; } = new List<FileUplodDetails>();

        #endregion
    }
}


