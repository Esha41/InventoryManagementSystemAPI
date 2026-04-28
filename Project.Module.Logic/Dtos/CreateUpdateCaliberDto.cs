using Ettad.Data.Enums;

namespace Ettad.Module.lookup.Dtos
{
    public class CreateUpdateCaliberDto
    {
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        /// <summary>Ammunition or Weapon only.</summary>
        public ItemType ItemType { get; set; }
        public bool IsDeleted { get; set; }
    }
}
