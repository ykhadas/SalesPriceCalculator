using System.Collections.Generic;
using Xunit;
using FluentAssertions;
using FluentAssertions.Execution;

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

            CombinationProduct c1 = new CombinationProduct(
              productId: 4,
              price: 15,
              subProducts: new List<ulong> { 1, 1 });

            CombinationProduct c2 = new CombinationProduct(
                productId: 5,
                price: 35,
                subProducts: new List<ulong> { 2, 2 });

            var res = SalesPriceCalculator.GetPriceWithSpecialOffers(new List<CombinationProduct>() { c1, c2 },
                new List<Product> { p1, p1, p2, p2, p3, p3 });

            using (new AssertionScope())
            {
                res.Price.Should().Be(110);
                res.ProductIds.Should().Contain(new ulong[] { 3, 3, 4, 5 });
            }
        }

        [Fact]
        public static void Test2()
        {
            Product p1 = new Product() { ProductId = 1, Price = 10 };
            Product p2 = new Product() { ProductId = 2, Price = 20 };

            CombinationProduct c1 = new CombinationProduct(
                productId: 4,
                price: 15,
                subProducts: new List<ulong> { 1, 1 });

            CombinationProduct c2 = new CombinationProduct(
                productId: 5,
                price: 24,
                subProducts: new List<ulong> { 1, 2 });

            var res = SalesPriceCalculator.GetPriceWithSpecialOffers(new List<CombinationProduct>() { c1, c2 },
                new List<Product> { p1, p1, p1, p1, p2 });

            using (new AssertionScope())
            {
                res.Price.Should().Be(49);
                res.ProductIds.Should().Contain(new ulong[] { 1, 4, 5 });
            }
        }

        [Fact]
        public static void Test3()
        {
            Product p1 = new Product() { ProductId = 1, Price = 10 };
            Product p2 = new Product() { ProductId = 2, Price = 20 };
            Product p3 = new Product() { ProductId = 3, Price = 30 };

            CombinationProduct c1 = new CombinationProduct(
              productId: 4,
              price: 35,
              subProducts: new List<ulong> { 1, 2 });

            CombinationProduct c2 = new CombinationProduct(
                productId: 5,
                price: 15,
                subProducts: new List<ulong> { 1, 3 });

            var res = SalesPriceCalculator.GetPriceWithSpecialOffers(new List<CombinationProduct>() { c1, c2 },
                new List<Product> { p1, p2, p3 });

            using (new AssertionScope())
            {
                res.Price.Should().Be(35);
                res.ProductIds.Should().Contain(new ulong[] { 2, 5 });
            }
        }

        [Fact]
        public static void Test4()
        {
            Product p1 = new Product() { ProductId = 1, Price = 10 };
            Product p2 = new Product() { ProductId = 2, Price = 20 };
            Product p3 = new Product() { ProductId = 3, Price = 30 };
            Product p4 = new Product() { ProductId = 4, Price = 40 };

            CombinationProduct c1 = new CombinationProduct(
                productId: 5,
                price: 15,
                subProducts: new List<ulong> { 1, 2 });

            CombinationProduct c2 = new CombinationProduct(
                productId: 6,
                price: 35,
                subProducts: new List<ulong> { 2, 3 });
            
            var res = SalesPriceCalculator.GetPriceWithSpecialOffers(new List<CombinationProduct>() { c1 , c2},
                new List<Product> { p1, p2, p2, p3, p3, p4});

            using (new AssertionScope())
            {
                res.Price.Should().Be(120);
                res.ProductIds.Should().Contain(new ulong[] { 5, 6, 3, 4 });
            }
        }

        [Fact]
        public static void GetPriceWithSpecialOffers_WithNoSpecialOffers_ReturnsOriginalPriceOfProducts()
        {
            Product p1 = new Product() { ProductId = 1, Price = 10 };
            Product p2 = new Product() { ProductId = 2, Price = 20 };
            Product p3 = new Product() { ProductId = 3, Price = 30 };

            var res = SalesPriceCalculator.GetPriceWithSpecialOffers(null,
                new List<Product> { p1, p2, p3 });

            using (new AssertionScope())
            {
                res.Price.Should().Be(60);
                res.ProductIds.Should().Contain(new ulong[] { 1, 2, 3 });
            }
        }
    }
}
