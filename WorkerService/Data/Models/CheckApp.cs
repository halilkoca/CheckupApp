using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class CheckApp : BaseEntity
    {
        [MinLength(3), MaxLength(512),Required]
        public string AppName { get; set; }
        [MinLength(3), MaxLength(512), Required]
        public string AppUrl { get; set; }
        public int Interval { get; set; }
    }
}
