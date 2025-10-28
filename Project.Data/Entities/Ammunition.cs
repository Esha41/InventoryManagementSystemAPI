namespace BrzanData.Models
{
    public class Ammunition :BaseItem
    {
        public int BulletDiameters { get; set; }
        public int CaseLength {  get; set; }
        public bool IsLinked { get; set; }
        public int NatureOptionId { get; set; }
        public string Ncn { get; set; }
        public int PrimaryPurpos { get; set; }
        public int ProjectileColor { get; set; }
        public int TotalWeight { get; set; }
        public int ProjectileMaterial { get; set; }
        public int CaseType { get; set; }
        public int Primer { get; set; }
        public int Probelant { get; set; }
        public int HazardDivsion { get; set; }
        public string CompabilityGroup { get;set; }
        public string Compatibility { get; set; }
    }
}
