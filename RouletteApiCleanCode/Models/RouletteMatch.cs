using System;
using System.ComponentModel.DataAnnotations;

namespace RouletteApiCleanCode.Models
{
    public class RouletteMatch
    {
        [Key]
        public int MatchId { get; set; }
        public int? TotalBets { get; set; }
        public int? TotalValueBets { get; set; }
        public string WinningBetNumber { get; set; }
        public DateTime? FHEndGame { get; set; }
        public int FK_Roulette { get; set; }
    }
}
