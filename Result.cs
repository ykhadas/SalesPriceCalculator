using System.Collections.Generic;

namespace SalesCombination
{
    public class Result
    {
        public int Price { get; }
        public IEnumerable<ulong> ProductIds { get; }

        public Result(int price, IEnumerable<ulong> productIds)
        {
            Price = price;
            ProductIds = productIds;
        }
    }
}
