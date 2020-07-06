using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

using RouletteApiCleanCode.Contexts;
using RouletteApiCleanCode.Models;

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
                return StatusCode(500, $"Disculpa, tenemos algunos inconvenientes. Intentelo más tarde. {ex.Message} {ex.StackTrace}");
            }
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
                    return Ok($"Se ha eliminado correctamente la ruleta con identificador: {roulette.RouletteId}.");
                }
                return BadRequest($"La solicitud es incorrecta y no ha podido ser procesada. Intentelo de nuevo.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Disculpa, tenemos algunos inconvenientes. Intentalo más tarde. {ex.Message} {ex.StackTrace}");
            }
        }
    }
}