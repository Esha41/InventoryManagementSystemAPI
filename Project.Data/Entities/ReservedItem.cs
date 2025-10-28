namespace BrzanData.Models
{
    public class ReservedItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int DepartmentId { get; set; }
        public int Year { get; set; }
        public decimal ReservedQuantity { get; set; }
    }
}
