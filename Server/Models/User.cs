using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    [Table("User")]
    public class User
    {
        [Key]
        [Column("username")]
        public string Username { get; set; }
        [Column("password")]
        public string Password { get; set; }
    }
}
