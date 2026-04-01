namespace Ettad.RequestManagement.Service.Returns.Dtos
{
    public class ProcessReturnItemsDto
    {
        public List<ReturnAmmoExplosiveItemDto> AmmoExplosiveItems { get; set; } = new();
        public List<ReturnWeaponItemDto> WeaponItems { get; set; } = new();
    }

    public class ReturnAmmoExplosiveItemDto
    {
        public long ItemId { get; set; }
        public long Quantity { get; set; }
        public string Lot { get; set; }
    }

    public class ReturnWeaponItemDto
    {
        public long ItemId { get; set; }
        public string SerialNumber { get; set; }
        public string BatchNumber { get; set; }
    }
}
