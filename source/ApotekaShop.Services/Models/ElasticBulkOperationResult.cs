namespace ApotekaShop.Services.Models
{
    public class ElasticBulkOperationResult
    {
        public int ProcessedCount { get; set; }
        public bool HasErrors { get; set; }
        public int TookMilliseconds { get; set; }
    }
}