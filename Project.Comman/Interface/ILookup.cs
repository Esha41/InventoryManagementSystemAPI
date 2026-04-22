using System;
using System.ComponentModel.DataAnnotations;

namespace Ettad.CrossCutting.Comman.Interface
{
    public interface ILookup
    {
        [MaxLength(500)]
        string NameEn { get; set; }
        [MaxLength(500)]
        string NameAr { get; set; }
        bool IsDeleted { get; set; }
    }
}
