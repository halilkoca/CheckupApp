using System.ComponentModel.DataAnnotations;

namespace UIWeb.Models
{
    public class CheckAppViewModel
    {
        public int? Id { get; set; }
        [MinLength(3), MaxLength(512), Required]
        public string AppName { get; set; }
        [MinLength(3), MaxLength(512), Required, Url]
        public string AppUrl { get; set; }
        public int Interval { get; set; }
    }
}
