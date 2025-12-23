using Ettad.Data.Enums;

namespace Ettad.Module.lookup.Dtos
{
    public class CreateUpdateItemTypeLookupDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public ItemType ItemType { get; set; }
        public bool IsDeleted { get; set; }
    }
}

