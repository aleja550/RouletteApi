using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

using RouletteApiCleanCode.Models;
using RouletteApiCleanCode.Contexts;

namespace RouletteApiCleanCode.Controllers
{
    [Route("api/[controller]")]
    public class RouletteUserController : Controller
    {
        private readonly AppDbContext context;

        public RouletteUserController(AppDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<RouletteUser> GetUsers()
        {
            return context.RouletteUser.ToList();
        }

        [HttpGet("{id}")]
        public RouletteUser GetUser(int id)
        {
            RouletteUser user = context.RouletteUser.FirstOrDefault(u => u.CodUser == id);
            return user;
        }

        [HttpPost]
        public ActionResult CreateUser([FromBody]RouletteUser user)
        {
            try
            {
                context.RouletteUser.Add(user);
                context.SaveChanges();
                return Ok($"El usuario {user.UserName} con identificador: {user.CodUser} se ha creado.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Disculpa, tenemos algunos inconvenientes. Intentelo más tarde. {ex.Message} {ex.StackTrace}");
            }
        }
    }
}