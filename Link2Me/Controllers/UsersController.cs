using Link2Me.Models;
using Microsoft.AspNetCore.Mvc;

namespace Link2Me.Controllers
{
    [Route("api")]
    [ApiController]
    public class UsersController : Controller
    {
        private Link2MeContext _context;

        public UsersController(Link2MeContext context)
        {
            _context = context;
        }
        const String SECURITY_KEY = "123";

        //POST api/login
        [HttpPost("[action]")]
        public IActionResult Login([FromForm] String username, [FromForm] String password)
        {
            try
            {
                var user = new User();

                if (username == null || password == null)
                    return StatusCode(StatusCodes.Status400BadRequest);

                user = _context.Users.FirstOrDefault(u => u.Username == username);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                if(user.Password != password)
                    return StatusCode(StatusCodes.Status401Unauthorized);

                return StatusCode(StatusCodes.Status200OK, new {
                    userId = user.Id,
                    username = user.Username,
                    userEmployeeId = user.EmployeeId,
                    securityKey = user.IsAdmin ? SECURITY_KEY : null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
