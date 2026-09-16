using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SC.Models
{
    [Table("Genre")]
    public class Genre
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")] //above a class property, specifies a property must have a value, cant null
        [MaxLength(40)]
        public string GenreName { get; set; }


    }
}
