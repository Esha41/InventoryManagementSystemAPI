namespace Ettad.RequestManagement.Service.Orders.Dto
{
    /// <summary>
    /// DTO representing allowance verification details for an item
    /// </summary>
    public class AllowanceVerificationDto
    {
        /// <summary>
        /// The item ID being verified
        /// </summary>
        public long ItemId { get; set; }

        /// <summary>
        /// The requested quantity for this item
        /// </summary>
        public long RequestedQuantity { get; set; }

        /// <summary>
        /// The department ID
        /// </summary>
        public long DepartmentId { get; set; }

        /// <summary>
        /// The year for which allowance is being checked
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Original allowance quantity allocated for this item in this department for this year
        /// </summary>
        public int OriginalAllowanceQuantity { get; set; }

        /// <summary>
        /// Quantity reserved by orders that are under processing (Draft supplies)
        /// </summary>
        public long ReservedByOrdersUnderProcessing { get; set; }

        /// <summary>
        /// Quantity already used/consumed from Submitted supplies
        /// </summary>
        public long UsedQuantity { get; set; }

        /// <summary>
        /// Available quantity = OriginalAllowanceQuantity - ReservedByOrdersUnderProcessing - UsedQuantity
        /// </summary>
        public long AvailableQuantity => Math.Max(0, OriginalAllowanceQuantity - ReservedByOrdersUnderProcessing - UsedQuantity);

        /// <summary>
        /// Indicates if the requested quantity can be fulfilled from available allowance
        /// </summary>
        public bool CanFulfillRequest => AvailableQuantity >= RequestedQuantity;

        /// <summary>
        /// Indicates if this item exists in the department's allowance
        /// </summary>
        public bool ItemExistsInAllowance { get; set; }
    }
}
