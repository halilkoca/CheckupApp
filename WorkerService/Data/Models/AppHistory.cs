using System;
using System.ComponentModel.DataAnnotations;

namespace Data.Models
{
    public class AppHistory : BaseEntity
    {
        public int CheckAppId { get; set; }
        public CheckApp CheckApp { get; set; }
        public DateTime RequestDateUtc { get; set; }
        public int StatusCode { get; set; }
        public string RequestId { get; set; }
        [MaxLength(1024)]
        public string ErrorMessage { get; set; }
    }
}
