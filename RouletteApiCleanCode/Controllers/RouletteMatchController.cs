using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using RouletteApiCleanCode.Models;
using RouletteApiCleanCode.Contexts;

namespace RouletteApiCleanCode.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouletteMatchController : Controller
    {
        private readonly AppDbContext context;

        public RouletteMatchController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<RouletteMatch> GetMatches()
        {
            return context.RouletteMatch.ToList();
        }

        [HttpPost]
        public ActionResult StartGame([FromBody]RouletteMatch match)
        {
            try
            {
                if (context.Roulette.FirstOrDefault(r => r.RouletteId == match.FK_Roulette).RouletteState)
                {
                    context.RouletteMatch.Add(match);
                    context.SaveChanges();
                    return Ok($"The game has started.");
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an internal error ad was unable to complete your request. {ex.Message} {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("GameClosure/{id}")]
        public ActionResult GameClosure(int id, [FromBody]RouletteMatch match)
        {
            RouletteMatch getMatchInfo = context.RouletteMatch.FirstOrDefault(r => r.MatchId == id);
            try
            {
                if (getMatchInfo != null)
                {
                    if (match.WinningBetNumber != null)
                    {
                        ClosingGame(match);
                        return Ok();
                    }
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an internal error ad was unable to complete your request. {ex.Message} {ex.StackTrace}");
            }
        }

        public void ClosingGame(RouletteMatch match)
        {
            List<RouletteBet> allBets = context.RouletteBet.Where(m => m.FK_MatchId == match.MatchId).ToList();
            context.RouletteMatch.FirstOrDefault(m => m.MatchId == match.MatchId).TotalBets = allBets.Count();
            context.RouletteMatch.FirstOrDefault(m => m.MatchId == match.MatchId).TotalValueBets = allBets.Sum(value => value.BetValue);
            context.Entry(match).Property(m => m.WinningBetNumber).IsModified = true;
            string formattedDate = string.Format("{0:yyyy-MM-ddTHH:mm:ssZ}", DateTime.Now);
            context.RouletteMatch.FirstOrDefault(m => m.MatchId == match.MatchId).FHEndGame = Convert.ToDateTime(formattedDate);
            context.SaveChanges();
        }
    }
}