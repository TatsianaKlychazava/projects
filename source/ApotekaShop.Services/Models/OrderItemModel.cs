namespace ApotekaShop.Services.Models
{
    public class OrderItemModel
    {
        /// <summary>
        /// Package Id
        /// </summary>
        public long Id { get; set; }

        public string Name { get; set; }

        public int Count { get; set; }

        public int PricePerUnit { get; set; }
    }
}
