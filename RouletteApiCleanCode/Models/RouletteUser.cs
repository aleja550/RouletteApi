using System.ComponentModel.DataAnnotations;

namespace RouletteApiCleanCode.Models
{
    public class RouletteUser
    {
        [Key]
        public int CodUser { get; set; }
        public string UserName { get; set; }
    }
}

