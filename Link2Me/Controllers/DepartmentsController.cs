using Link2Me.Models;
using Microsoft.AspNetCore.Mvc;

namespace Link2Me.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentsController : Controller
    {
        private Link2MeContext _context;

        public DepartmentsController(Link2MeContext context)
        {
            _context = context;
        }

        //GET api/departments
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var departments = new List<Department>();
                departments = _context.Departments.ToList();

                return StatusCode(StatusCodes.Status200OK, departments);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //GET api/departments/5
        [HttpGet("{departmentId}")]
        public IActionResult Get(int departmentId)
        {
            try
            {
                var department = new Department();

                if (departmentId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                department = _context.Departments.FirstOrDefault(d => d.Id == departmentId);

                if (department == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, department);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
