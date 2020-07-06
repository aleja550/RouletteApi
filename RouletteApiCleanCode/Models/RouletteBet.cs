using System.ComponentModel.DataAnnotations;

namespace RouletteApiCleanCode.Models
{
    public class RouletteBet
    {
        [Key]
        public int BetId { get; set; }
        public string BetOnNumber { get; set; }
        public int BetValue { get; set; }
        public int FK_MatchId { get; set; }
        public int FK_CodUser { get; set; }
    }
}
