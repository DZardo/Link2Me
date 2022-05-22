using System.ComponentModel.DataAnnotations;

namespace Link2Me.Models
{
    public class Department
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(100)]
        public String Name { get; set; }
    }
}
