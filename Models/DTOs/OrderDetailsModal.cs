namespace SC.Models.DTOs
{
    public class OrderDetailsModal
    {
        public string DivId { get; set; }
        public IEnumerable<OrderDetail> OrderDetail { get; set; }


    }
}
