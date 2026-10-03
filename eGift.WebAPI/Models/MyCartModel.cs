namespace eGift.WebAPI.Models
{
    public class MyCartModel : BaseModel
    {
        #region Data Model Properties

        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }
        public int Quantity { get; set; }
        public bool IsOrdered { get; set; }

        #endregion
    }
}