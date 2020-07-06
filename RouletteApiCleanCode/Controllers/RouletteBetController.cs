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
    public class RouletteBetController : Controller
    {
        private readonly AppDbContext context;

        public RouletteBetController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<RouletteBet> GetBets()
        {
            return context.RouletteBet.ToList();
        }

        [HttpPost]
        public ActionResult CreateBet([FromBody]RouletteBet bet)
        {
            bool isNumber = ValidateBetNumber(bet);
            bool isColor = ValidateBetColor(bet);
            try
            {
                if (!this.Request.Headers.Keys.Contains("UserId"))
                {
                    return StatusCode(403, "La apuesta no tiene un ID de usuario.");
                }

                if (isNumber && bet.BetValue <= 10000)
                {
                    context.RouletteBet.Add(bet);
                    context.SaveChanges(); 

                    return Ok($"La apuesta se ha creado.");
                }

                if (isColor && bet.BetValue <= 10000)
                {
                    context.RouletteBet.Add(bet);
                    context.SaveChanges();

                    return Ok($"La apuesta se ha creado.");
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"The server encountered an internal error ad was unable to complete your request. {ex.Message} {ex.StackTrace}");
            }
        }

        public bool ValidateBetNumber(RouletteBet bet)
        {
            try
            {
                if (Enumerable.Range(1, 36).Contains(Convert.ToInt16(bet.BetOnNumber)))
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public bool ValidateBetColor(RouletteBet bet)
        {
            try
            {
                string color = bet.BetOnNumber.Trim().ToLower(); ;
                if (color == "black" || color == "red")
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }
    }
}