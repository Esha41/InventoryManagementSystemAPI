using System.ComponentModel.DataAnnotations;

namespace Ettad.CrossCutting.Comman.Base
{
    public class BaseEntity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
