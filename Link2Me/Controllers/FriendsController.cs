using Link2Me.Models;
using Microsoft.AspNetCore.Mvc;

namespace Link2Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FriendsController : Controller
    {
        private Link2MeContext _context;

        public FriendsController(Link2MeContext context)
        {
            _context = context;
        }

        //GET api/friends?userEmployeeId=1
        [HttpGet]
        public IActionResult Get(int userEmployeeId)
        {
            try
            {
                var friends = new List<Friend>();

                if (userEmployeeId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                friends = _context.Friends.Where(f => f.UserEmployeeId == userEmployeeId).ToList();

                return StatusCode(StatusCodes.Status200OK, friends);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //PUT api/friends/9
        [HttpPost("{friendId}")]
        public IActionResult New([FromForm] int userEmployeeId, int friendId)
        {
            try
            {
                Friend friend = new Friend();

                if (userEmployeeId <= 0 || friendId <= 0 || userEmployeeId == friendId)
                    return StatusCode(StatusCodes.Status400BadRequest);

                if (_context.Friends.FirstOrDefault<Friend>(f => f.UserEmployeeId == userEmployeeId && f.FriendId == friendId) != null)
                    return StatusCode(StatusCodes.Status304NotModified);

                var employee = _context.Employees.FirstOrDefault(e => e.Id == friendId);
                if (employee == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                friend.UserEmployeeId = userEmployeeId;
                friend.FriendId = employee.Id;
                friend.FriendSince = DateTime.Now;

                var response = _context.Friends.Add(friend);

                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, response);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //DELETE api/friends/5
        [HttpDelete("{friendId}")]
        public IActionResult Delete([FromForm] int userEmployeeId, int friendId)
        {
            try
            {
                if (friendId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var friend = _context.Friends.FirstOrDefault(f => f.UserEmployeeId == userEmployeeId && f.FriendId == friendId);

                if (friend == null)
                    return StatusCode(StatusCodes.Status404NotFound);
                
                _context.Friends.Remove(friend);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            } 
        }

        //GET api/friends/getdistance/2
        [HttpGet("[action]/{friendId}")]
        public IActionResult GetDistance([FromForm] int userEmployeeId, int friendId)
        {
            try
            {
                var user = new Employee();
                var friend = new Employee();

                if (userEmployeeId <= 0 || friendId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                user = _context.Employees.FirstOrDefault(e => e.Id == userEmployeeId);
                friend = _context.Employees.FirstOrDefault(e => e.Id == friendId);

                if (user == null || friend == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                if (user.Position == null || friend.Position == null)
                    return StatusCode(StatusCodes.Status400BadRequest);

                return StatusCode(StatusCodes.Status200OK, Position.Difference(Position.ToPosition(user.Position), Position.ToPosition(friend.Position)));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}