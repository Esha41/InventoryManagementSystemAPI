using Ettad.Module.lookup.Dtos;

namespace Ettad.RequestManagement.Service.Common.Dtos
{
    public class RequesterDto
    {
        public string Id { get; set; }
        public string UserName {get; set; }
        public string FullNameEN { get; set; }
        public string FullNameAR { get; set; }
        public string? MilitoryId { get; set; }
        public string? Email { get; set; }
        
        public RankDto? Rank { get; set; }
        public DepartmentDto? Department { get; set; }
    }
}
