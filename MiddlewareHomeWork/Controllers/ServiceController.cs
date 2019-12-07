using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MiddlewareHomeWork.DataAccess;
using MiddlewareHomeWork.Models;

namespace MiddlewareHomeWork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly UserContext _context;

        public ServiceController(UserContext context)
        {
            _context = context;
        }



        // GET: api/Service/5
        [HttpGet]
        public ActionResult<User> GetUser() { 

            var users = _context.Users.ToList();
            foreach (var user in users)
            {
                if (user.IsAuth)
                {
                    return user;
                }
            }
            //var user = await _context.Users.FindAsync(id);
            return NotFound();
        }


        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
