using Ettad.Data.Enums;

namespace Ettad.Module.lookup.Dtos
{
    public class RequestPurposeDto
    {
        public long Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string RequestType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
