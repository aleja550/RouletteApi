using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using RouletteApiCleanCode.Models;
using RouletteApiCleanCode.Contexts;

namespace RouletteApiCleanCode.Controllers
{
    [Route("api/[controller]")]
    public class RouletteController : Controller
    {
        private readonly AppDbContext context;

        public RouletteController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Roulette> GetRoulettes()
        {
            return context.Roulette.ToList();
        }

        [HttpGet("{id}")]
        public Roulette GetRoulette(int id)
        {
            Roulette roulette = context.Roulette.FirstOrDefault(r => r.RouletteId == id);
            return roulette;
        }

        [HttpPost]
        public ActionResult CreateRoulette([FromBody]Roulette roulette = null)
        {
            try
            {
                context.Roulette.Add(roulette);
                context.SaveChanges();
                return Ok($"{roulette.RouletteId}.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an internal error was unable to complete your request. {ex.Message} {ex.StackTrace}");
            }
        }

        [HttpPut]
        [Route("SetStateRoulette/{id}")]
        public ActionResult OpeningOrClosingRoulette(int id, [FromBody]Roulette roulette)
        {
            Roulette getRouletteInfo = context.Roulette.FirstOrDefault(r => r.RouletteId == id);
            try
            {
                if (getRouletteInfo != null)
                {
                    if (getRouletteInfo.RouletteState == false && roulette.RouletteState == true)
                    {
                        OpeningRoulette(roulette);
                        return Ok($"La ruleta ha sido abierta exitosamente.");
                    }

                    if (getRouletteInfo.RouletteState == true && roulette.RouletteState == false)
                    {
                        ClosingRoulette(roulette);

                        Roulette getRoulette = context.Roulette.FirstOrDefault(r => r.RouletteId == id);
                        List<RouletteMatch> allMatches = context.RouletteMatch
                                                         .Where(r => r.FK_Roulette == getRoulette.RouletteId
                                                                && r.FHEndGame > getRoulette.FHOpening && r.FHEndGame < getRoulette.FHClosing)
                                                         .ToList();
                        int? totalValueOnBets = allMatches.Sum(v => v.TotalValueBets);
                        int? totalBets = allMatches.Sum(b => b.TotalBets);
                        return Ok($"La ruleta ha sido cerrada exitosamente. Total apuestas realizadas: {totalBets}. " +
                                  $"Total partidas jugadas: {allMatches.Count}. Total dinero jugado en las apuestas: {totalValueOnBets}");
                    }

                    return StatusCode(304, $"La ruleta no tuvo ningún cambio.");
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an internal error was unable to complete your request. {ex.Message} {ex.StackTrace}");
            }
        }

        public void OpeningRoulette(Roulette roulette)
        {
            string formattedDate = string.Format("{0:yyyy-MM-ddTHH:mm:ssZ}", DateTime.Now);
            context.Entry(roulette).Property(r => r.RouletteState).IsModified = true;
            context.Roulette.FirstOrDefault(r => r.RouletteId == roulette.RouletteId).FHOpening = Convert.ToDateTime(formattedDate);
            context.Roulette.FirstOrDefault(r => r.RouletteId == roulette.RouletteId).FHClosing = null;
            context.SaveChanges();
        }

        public void ClosingRoulette(Roulette roulette)
        {
            string formattedDate = string.Format("{0:yyyy-MM-ddTHH:mm:ssZ}", DateTime.Now);
            context.Entry(roulette).Property(r => r.RouletteState).IsModified = true;
            context.Roulette.FirstOrDefault(r => r.RouletteId == roulette.RouletteId).FHClosing = Convert.ToDateTime(formattedDate);
            context.SaveChanges();
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteRoulette(int id)
        {
            try
            {
                Roulette roulette = context.Roulette.FirstOrDefault(r => r.RouletteId == id);
                if (roulette != null)
                {
                    context.Roulette.Remove(roulette);
                    context.SaveChanges();
                    return Ok($"The Roulette {roulette.RouletteId} has been deleted.");
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an internal error ad was unable to complete your request. {ex.Message} {ex.StackTrace}");
            }
        }
    }
}