using System.ComponentModel.DataAnnotations;

namespace SC.Models.DTOs
{
    public class GenreDTO
    {
        public int Id { get; set; }
        [Required] 
        public string GenreName { get; set; }
    }
}
