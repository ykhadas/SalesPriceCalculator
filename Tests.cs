using System.Collections.Generic;
using Xunit;
using FluentAssertions;

namespace SalesCombination
{
    public class Tests
    {
        [Fact]
        public static void Test1()
        {
            Product p1 = new Product() { ProductId = 1, Price = 10 };
            Product p2 = new Product() { ProductId = 2, Price = 20 };
            Product p3 = new Product() { ProductId = 3, Price = 30 };

            CombinationProduct c1 = new CombinationProduct() { Price = 15, SubProductIds = new List<ulong> { 1, 1 } };
            CombinationProduct c2 = new CombinationProduct() { Price = 35, SubProductIds = new List<ulong> { 2, 2 } };

            var res = new SalesPriceCalculator().GetPriceWithSpecialOffers(new List<CombinationProduct>() { c1, c2 },
                new List<Product> { p1, p1, p2, p2, p3, p3 });
            res.Should().Be(110);
        }

        [Fact]
        public static void Test2()
        {
            Product p1 = new Product() { ProductId = 1, Price = 10 };
            Product p2 = new Product() { ProductId = 2, Price = 20 };
            Product p3 = new Product() { ProductId = 3, Price = 30 };

            CombinationProduct c1 = new CombinationProduct() { Price = 15, SubProductIds = new List<ulong> { 1, 1 } };
            CombinationProduct c2 = new CombinationProduct() { Price = 24, SubProductIds = new List<ulong> { 1, 2 } };

            var res = new SalesPriceCalculator().GetPriceWithSpecialOffers(new List<CombinationProduct>() { c1, c2 },
                new List<Product> { p1, p1, p1, p1, p2 });
            res.Should().Be(49);
        }
    }
}
