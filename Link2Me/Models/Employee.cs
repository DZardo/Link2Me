using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Spatial;
using System.Text.Json;

namespace Link2Me.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required, StringLength(50)]
        public String FirstName { get; set; }
        [Required, StringLength(50)]
        public String LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        [StringLength(100)]
        public String Address { get; set; }
        public int DepartmentId { get; set; }
        [StringLength(50)]
        public String Email { get; set; }
        [StringLength(50)]
        public String Telephone { get; set; }
        public String? Position { get; set; }
    }

    public class Position
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public Position()
        {

        }

        public Position(double latitude, double longitude) {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

        public static double Difference(Position p1, Position p2)
        {
            return Math.Sqrt(Math.Abs(p1.Latitude - p2.Latitude) + Math.Abs(p1.Longitude - p2.Longitude));
        }

        //{"Latitude":999.999,"Longitude":-999.999}
        public static Position ToPosition(String positionString)
        {
            return JsonSerializer.Deserialize<Position>(positionString) ?? new Position();
        }

        public override String ToString()
        {
            return JsonSerializer.Serialize(this ?? new Position());
        }
    }
}