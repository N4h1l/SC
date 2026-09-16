using System.ComponentModel.DataAnnotations;

namespace SC.Models.DTOs
{
    public class StockDTO
    {
        public int BookId { get; set;  }
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
