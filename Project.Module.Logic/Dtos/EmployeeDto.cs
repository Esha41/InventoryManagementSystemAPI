namespace Ettad.Module.lookup.Dtos
{
    public class EmployeeDto
    {
        public long Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string IdNo { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Notes { get; set; }
        public long? RankId { get; set; }

        #region Navigation Properties
        public RankDto Rank { get; set; }
        #endregion
    }
}
