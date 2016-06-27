using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
