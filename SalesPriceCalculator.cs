using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesCombination
{
    public class SalesPriceCalculator
    {
        public static Result GetPriceWithSpecialOffers(IEnumerable<CombinationProduct> specialOffers, IEnumerable<Product> productsInTheBasket)
        {
            Dictionary<int, Result> cache = new Dictionary<int, Result>();

            return GetMinPrice(specialOffers, productsInTheBasket, cache);
        }

        private static Result GetMinPrice(IEnumerable<CombinationProduct> specialOffers, IEnumerable<Product> productsInTheBasket, Dictionary<int, Result> cache)
        {
            if (!productsInTheBasket.Any())
                return new Result(0, Array.Empty<ulong>());

            if (specialOffers == null || !specialOffers.Any())
            {
                return new Result(productsInTheBasket.Sum(p=>p.Price), productsInTheBasket.Select(p=>p.ProductId));
            }

            //check if we already computed minimum price for that combination
            var products = productsInTheBasket.Select(p => p.ProductId);
            int key = GetSequenceHashCode(products);

            if (cache.ContainsKey(key))
            {
                return cache[key];
            }

            //price without offers

            int localMinPrice = productsInTheBasket.Sum(n => n.Price);
          

            foreach (var offer in specialOffers)
            {
                //don't go further if there are more products in the offer than we have
                if (offer.SubProductIds.Count() > productsInTheBasket.Count())
                    break;

                List<Product> tempProducts = new List<Product>(productsInTheBasket);

                //are current product actually fit any offer?
                if (IsOfferValid(offer, tempProducts))
                {
                    ReduceByOffer(offer, tempProducts);

                    int offerPrice = offer.Price;
                    
                    Result minPriceResult = GetMinPrice(specialOffers, tempProducts, cache);

                    int tempMinPrice = offerPrice + minPriceResult.Price;

                    if(localMinPrice>tempMinPrice)
                    {
                        products = minPriceResult.ProductIds.Concat(new[] { offer.ProductId });
                        localMinPrice = tempMinPrice;
                    }
                }
            }

            var result = new Result(localMinPrice, products);
            cache[key] = result;
            return result;
        }

        private static void ReduceByOffer(CombinationProduct offer, List<Product> productsInTheBasket)
        {
            foreach (var product in offer.SubProductIds)
            {
                var productToRemove = productsInTheBasket.Find(p => p.ProductId == product);
                productsInTheBasket.Remove(productToRemove);
            }
        }

        private static bool IsOfferValid(CombinationProduct offer, List<Product> productsInTheBasket)
        {
            var productIds = productsInTheBasket.Select(n => n.ProductId);
            var subProductsIds = offer.SubProductIds;

            bool isValid = new HashSet<ulong>(productIds).IsSupersetOf(subProductsIds);
            return isValid;
        }

        private static int GetSequenceHashCode(IEnumerable<ulong> products)
        {
            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                return products.Aggregate(seed, (current, item) =>
                    (current * modifier) + item.GetHashCode());
            }
        }
    }
}
