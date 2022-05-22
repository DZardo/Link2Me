using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Link2Me.Models
{
    [Index(nameof(Username), IsUnique = true)]
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public String Username { get; set; }
        [Required, StringLength(50)]
        public String Password { get; set; }
        public Boolean IsAdmin { get; set; }
        public int EmployeeId { get; set; }
    }
}
