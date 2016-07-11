using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using ApotekaShop.Services.Interfaces;
using ApotekaShop.Services.Models;

namespace ApotekaShop.Services
{
    public class PaymentService : IPaymentService
    {
        public object AggregatePaymentData(IEnumerable<OrderItemModel> items, string baseUrl)
        {
            Random rnd = new Random();

            var vesrion = "v10";
            var merchantId = "19072";
            var agreementId = "67857";
            var orderId = rnd.Next(0, int.MaxValue);
            var amount = items.Sum(s => s.PricePerUnit * s.Count);
            var currency = "DKK";
            var continueUrl = BuildUri(baseUrl, "complete");
            var cancelUrl = BuildUri(baseUrl, "cancel");
            var callbackUrl = BuildUri(baseUrl, "callback");

            var parameters = new Dictionary<string, string>
            {
                {"version", vesrion},
                {"merchant_id", merchantId},
                {"agreement_id", agreementId},
                {"order_id", orderId.ToString()},
                {"amount", amount.ToString()},
                {"currency", currency},
                {"continue_url", continueUrl},
                {"cancel_url", cancelUrl},
                {"callback_url", callbackUrl}
            };

            object data = new
            {
                Version = vesrion,
                OrderId = orderId.ToString(),
                MerchantId = merchantId,
                AgreementId = agreementId,
                Amount = amount,
                Currency = currency,
                ContinueUrl = continueUrl,
                CancelUrl = cancelUrl,
                CallbackUrl = callbackUrl,
                Checksum = Sign(parameters, "3de53a2bd161c0393fe2d00147792fb37caa89e833a5073d998ec9cb2865e589")
            };

            return data;
        }

        private string BuildUri(string baseUrl, string status)
        {
            return  string.Format("{0}/?status={1}", baseUrl, status);
        }

        private string Sign(Dictionary<string, string> parameters, string api_key)
        {
            var result = String.Join(" ", parameters.OrderBy(c => c.Key).Select(c => c.Value).ToArray());
            var e = Encoding.UTF8;
            var hmac = new HMACSHA256(e.GetBytes(api_key));
            byte[] b = hmac.ComputeHash(e.GetBytes(result));
            var s = new StringBuilder();
            for (int i = 0; i < b.Length; i++)
            {
                s.Append(b[i].ToString("x2"));
            }
            return s.ToString();
        }
    }
}
