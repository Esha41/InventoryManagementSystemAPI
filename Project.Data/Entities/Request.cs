using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrzanData.Models
{
    public class Request 
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
    public string RequestNumber { get; set; }
    public string RequestDate { get; set; }
    public string RequestStatus { get; set; }
    public int DepotId { get; set; }
    public int ItemId { get; set; }
    public int Quantity { get; set; }
    public string RequestPriority { get; set; }
   
    public string RequestDepartment { get; set; }
   
    public int ReciverId { get; set; }
    }
}