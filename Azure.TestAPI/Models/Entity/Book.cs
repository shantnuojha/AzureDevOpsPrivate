using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Azure.TestAPI.Models.Entity
{
    public class Book
    {
        public Book() { }
        [Key]
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        [Required]
        public string Name { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string Genere { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        [Required]
        public string PublisherName { get; set; }
    }
}
