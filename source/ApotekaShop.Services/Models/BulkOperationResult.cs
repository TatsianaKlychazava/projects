namespace ApotekaShop.Services.Models
{
    public class BulkOperationResult
    {
        public int ProcessedCount { get; set; }
        public bool HasErrors { get; set; }
        public int TookMilliseconds { get; set; }
    }
}