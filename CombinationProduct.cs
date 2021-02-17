using System.Collections.Generic;

namespace SalesCombination
{
    public class CombinationProduct
    {
        public ulong ProductId { get; set; }
        public int Price { get; set; }
        public List<ulong> SubProductIds { get; set; }
    }

}
