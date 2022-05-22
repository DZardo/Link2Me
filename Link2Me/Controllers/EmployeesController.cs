using Link2Me.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace Link2Me.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : Controller
    {
        private Link2MeContext _context;

        public EmployeesController(Link2MeContext context)
        {
            _context = context;
        }

        const String SECURITY_KEY = "123";

        //POST api/employees?firstName=&lastName=&departmentId=
        [HttpGet]
        public IActionResult Get(String? firstName, String? lastName, int? departmentId)
        {
            try
            {
                IEnumerable<Employee> employees = _context.Employees;

                if (!String.IsNullOrEmpty(firstName))
                    employees = employees.Where(e => e.FirstName == firstName);

                if (!String.IsNullOrEmpty(lastName))
                    employees = employees.Where(e => e.LastName == lastName);

                if (departmentId > 0)
                    employees = employees.Where(e => e.DepartmentId == departmentId);

                return StatusCode(StatusCodes.Status200OK, employees.ToList());
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //GET api/employees/5
        [HttpGet("{employeeId}")]
        public IActionResult Get(int employeeId)
        {
            try
            {
                var employee = new Employee();

                if (employeeId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);

                if(employee == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                return StatusCode(StatusCodes.Status200OK, employee);
            }
            catch(Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        /*
        //GET api/employees/getallbydepartment/2
        [HttpGet("[action]/{departmentId}")]
        public IActionResult GetAllByDepartment(int departmentId)
        {
            try
            {
                var employees = new List<Employee>();
                
                if(departmentId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                employees = _context.Employees.Where(e => e.DepartmentId == departmentId).ToList();

                return StatusCode(StatusCodes.Status200OK, employees);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        *///Same purpose of Get() with departmentId = n

        //POST api/employees
        [HttpPost]
        public IActionResult New([FromForm] Employee value)
        {
            try
            {
                StringValues securityKey;
                
                if (!Request.Headers.TryGetValue("securityKey", out securityKey) || !SecurityKeyCheck(securityKey.First()))
                    return StatusCode(StatusCodes.Status401Unauthorized);

                if (value == null)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var employee = _context.Employees.Add(value);

                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //PUT api/employees/5
        [HttpPut("{employeeId}")]
        public IActionResult Update(int employeeId, [FromForm] Employee value)
        {
            try
            {
                if (employeeId <= 0 || value == null)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);

                if (employee == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                value.Id = employeeId;
                
                _context.Entry<Employee>(employee).CurrentValues.SetValues(value);
                _context.SaveChanges();
                
                return StatusCode(StatusCodes.Status200OK, employee);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        //DELETE api/employees/5
        [HttpDelete("{employeeId}")]
        public IActionResult Delete(int employeeId)
        {
            try
            {
                StringValues securityKey;

                if (!Request.Headers.TryGetValue("securityKey", out securityKey) || !SecurityKeyCheck(securityKey.First()))
                    return StatusCode(StatusCodes.Status401Unauthorized);

                if (employeeId <= 0)
                    return StatusCode(StatusCodes.Status400BadRequest);

                var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);

                if (employee == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                _context.Employees.Remove(employee);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("[action]/{employeeId}")]
        public IActionResult SetPosition(int employeeId, [FromForm] double latitude, [FromForm] double longitude)
        {
            try
            {
                var employee = _context.Employees.FirstOrDefault(e => e.Id == employeeId);

                if (employee == null)
                    return StatusCode(StatusCodes.Status404NotFound);

                employee.Position = new Position(latitude, longitude).ToString();

                _context.Entry<Employee>(employee).CurrentValues.SetValues(employee);
                _context.SaveChanges();

                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        private bool SecurityKeyCheck(String securityKey)
        {
            return securityKey == SECURITY_KEY;
        }
    }
}
