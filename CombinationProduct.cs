using System.Collections.Generic;

namespace SalesCombination
{
    public class CombinationProduct
    {
        public ulong ProductId { get; }
        public int Price { get; }
        public IEnumerable<ulong> SubProductIds { get; }

        public CombinationProduct(ulong productId, int price, IEnumerable<ulong> subProducts)
        {
            ProductId = productId;
            Price = price;
            SubProductIds = subProducts;
        }
    }
}
