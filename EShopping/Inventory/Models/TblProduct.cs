using System;
using System.Collections.Generic;

namespace Inventory.Models
{
    public partial class TblProduct
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public int? Quantity { get; set; }
        public decimal? Price { get; set; }
        public int? Discount { get; set; }
    }
}
