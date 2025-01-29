using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Models
{
    [Table("Note")]
    public class Note
    {
        [Key]
        [Column("id_note")]
        public long IdNote { get; set; }
        [Column("content")]
        public string Content { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }

        [ForeignKey("username")]
        public string Username { get; set; }
    }
}
