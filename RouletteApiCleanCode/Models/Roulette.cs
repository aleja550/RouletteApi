using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteApiCleanCode.Models
{
    public class Roulette
    {
        [Key]
        public int RouletteId { get; set; }
        public bool RouletteState { get; set; }
        public DateTime? FHOpening { get; set; }
        public DateTime? FHClosing { get; set; }
    }
}
