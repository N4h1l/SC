
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SC.Models
{
    [Table("Order")]
    public class Order
    {
        public int Id { get; set; }
        [Required] public string UserId { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        [Required] public int OrderStatusId { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        [MaxLength(100)]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? MobileNumber { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? PaymentMethod { get; set;  }
        public bool Ispaid { get; set; }
        public OrderStatus OrderStatus { get; set;  }
        public List<OrderDetail> OrderDetail {  get; set; }
       
    }
}
