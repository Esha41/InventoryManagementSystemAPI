using Ettad.CrossCutting.Comman.Base;
using Ettad.CrossCutting.Comman.Idenitity;
using System;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Junction entity for many-to-many relationship between Reports and Roles
    /// Used to specify which roles can access a published report
    /// </summary>
    public class ReportRole : FullAuditEntity<Guid>
    {
        /// <summary>
        /// Report ID (foreign key to Reports table)
        /// </summary>
        public Guid ReportId { get; set; }

        /// <summary>
        /// Role ID (foreign key to Roles table)
        /// </summary>
        public string RoleId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property for report
        /// </summary>
        public virtual ReportEntity Report { get; set; }

        /// <summary>
        /// Navigation property for role
        /// </summary>
        public virtual ApplicationRole Role { get; set; }
    }
}
