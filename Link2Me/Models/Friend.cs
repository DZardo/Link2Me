using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Link2Me.Models
{
    public class Friend
    {
        [Key]
        [Column(Order = 0)]
        public int UserEmployeeId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int FriendId { get; set; }
        public DateTime FriendSince { get; set; }
    }
}
