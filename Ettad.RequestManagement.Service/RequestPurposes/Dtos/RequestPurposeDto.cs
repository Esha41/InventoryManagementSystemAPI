using Ettad.Data.Enums;

namespace Ettad.RequestManagement.Service.RequestPurposes.Dtos
{
    public class CreateUpdateRequestPurposeDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public RequestPurposeAllowanceContext? AllowanceContext { get; set; }

        public List<ItemType> ItemTypes { get; set; } = new List<ItemType>();

        public List<CreateUpdateAttachmentRequirementDto> AttachmentRequirements { get; set; }
            = new List<CreateUpdateAttachmentRequirementDto>();
    }

    public class RequestPurposeDto
    {
        public long Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public RequestType RequestType { get; set; }
        public RequestPurposeAllowanceContext AllowanceContext { get; set; }

        public List<ItemType> ItemTypes { get; set; } = new List<ItemType>();

        public List<AttachmentRequirementDto> AttachmentRequirements { get; set; }
            = new List<AttachmentRequirementDto>();
    }

    public class CreateUpdateAttachmentRequirementDto
    {
        /// <summary>
        /// Identifier of an existing requirement when updating. Leave null/0 to create.
        /// </summary>
        public long? Id { get; set; }

        public string NameAr { get; set; }
        public string NameEn { get; set; }

        public bool IsRequired { get; set; } = true;

        public int MinCount { get; set; } = 1;
        public int MaxCount { get; set; } = 1;

        public int DisplayOrder { get; set; } = 0;
    }

    public class AttachmentRequirementDto
    {
        public long Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsRequired { get; set; }
        public int MinCount { get; set; }
        public int MaxCount { get; set; }
        public int DisplayOrder { get; set; }
    }
}

