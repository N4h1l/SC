using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
//Nuget PM: add-migration -ErrorAction Ignore
//update-database -ErrorAction Ignore

namespace SC.Models
{
    [Table("Book")] //map an entity class to a database table name and schema
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")] //above a class property, specifies a property must have a value, cant null
        [MaxLength(40)]
        public string? BookName { get; set; }
        [Required]
        [MaxLength(40)]
        public string? AuthorName { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; } 
        public string? Image { get; set; }
        public double Price { get; set; }
        public List<OrderDetail> OrderDetail { get; set; }
        public List<CartDetail> CartDetail { get; set; }
        public Stock Stock { get; set; }
        [NotMapped] 
        public string GenreName { get; set; }
        [NotMapped]
        public int Quantity { get; set; }
    }
}
