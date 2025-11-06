using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.RequestPurposes.Dtos
{
    public class CreateUpdateRequestPurposeDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
    }

    public class RequestPurposeDto
    {
        public long Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public RequestType RequestType { get; set; }
    }
}

