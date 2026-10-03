namespace eGift.WebAPI.Models
{
    public class OrderDetailsModel : BaseModel
    {
        #region Data Model Properties
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal? Discount { get; set; }
        public decimal? Tax { get; set; }
        public decimal NetAmount { get; set; }
        #endregion
    }
}