using System.Collections.Generic;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services.Interfaces
{
    public interface IPaymentService
    {
        object AggregatePaymentData(IEnumerable<OrderItemModel> items, string baseUrl);
    }
}
