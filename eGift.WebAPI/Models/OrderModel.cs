using System.ComponentModel.DataAnnotations.Schema;

namespace eGift.WebAPI.Models
{
    public class OrderModel : BaseModel
    {
        #region Data Model Properties
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalTax { get; set; }
        public required string OrderNumber { get; set; }
        public string? Notes { get; set; }
        public DateTime? DispatchedDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public DateTime? DeliveryDate { get; set; }
        public DateTime? CancelDate { get; set; }
        public int StatusId { get; set; }
        #endregion

        #region Reference Model Properties
        [NotMapped]
        public List<OrderDetailsModel> Items { get; set; } = new List<OrderDetailsModel>();
        #endregion
    }
}