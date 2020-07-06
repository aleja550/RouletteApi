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
                return StatusCode(500, $"The server encountered an internal error ad was unable to complete your request. {ex.Message} {ex.StackTrace}");
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