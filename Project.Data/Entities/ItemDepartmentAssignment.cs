using Ettad.CrossCutting.Comman.Base;

namespace Ettad.Data.Entities
{
    /// <summary>
    /// Represents an assignment of an item to a department.
    /// This allows departments to have access to specific items.
    /// </summary>
    public class ItemDepartmentAssignment : AuditEntity<long>
    {
        /// <summary>
        /// The item being assigned to the department
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// The department the item is assigned to
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// Optional notes about this assignment
        /// </summary>
        public string? Notes { get; set; }

        #region Navigation Properties

        public BaseItem Item { get; set; }
        public Department Department { get; set; }

        #endregion
    }
}
