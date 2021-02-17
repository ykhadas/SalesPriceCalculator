using System.Collections.Generic;
using System.Linq;

namespace SalesCombination
{
    public class SalesPriceCalculator
    {
        public int GetPriceWithSpecialOffers(List<CombinationProduct> specialOffers, List<Product> productsInTheBasket)
        {
            Dictionary<int, int> cache = new Dictionary<int, int>();

            return GetMinPrice(specialOffers, productsInTheBasket, cache);
        }

        private int GetMinPrice(List<CombinationProduct> specialOffers, List<Product> productsInTheBasket, Dictionary<int, int> cache)
        {
            if (!productsInTheBasket.Any())
                return 0;

            //check if we already computed minimum price for that combination
            int key = GetSequenceHashCode(productsInTheBasket);

            if (cache.ContainsKey(key))
            {
                return cache[key];
            }

            //price without offers
            int localMinPrice = productsInTheBasket.Sum(n => n.Price);

            foreach (var offer in specialOffers)
            {
                //don't go further if there are more products in the offer than we have
                if (offer.SubProductIds.Count > productsInTheBasket.Count)
                    break;

                List<Product> tempProducts = new List<Product>(productsInTheBasket);

                //are current product actually fit any offer?
                if (IsOfferValid(offer, tempProducts))
                {
                    ReduceByOffer(offer, tempProducts);

                    int offerPrice = offer.Price;
                    int tempMinPrice = offerPrice + GetMinPrice(specialOffers, tempProducts, cache);

                    localMinPrice = localMinPrice < tempMinPrice ? localMinPrice : tempMinPrice;
                }
            }

            cache[key] = localMinPrice;
            return localMinPrice;
        }

        private void ReduceByOffer(CombinationProduct offer, List<Product> productsInTheBasket)
        {
            foreach (var product in offer.SubProductIds)
            {
                var productToRemove = productsInTheBasket.Find(p => p.ProductId == product);
                productsInTheBasket.Remove(productToRemove);
            }
        }

        private bool IsOfferValid(CombinationProduct offer, List<Product> productsInTheBasket)
        {
            var productIds = productsInTheBasket.Select(n => n.ProductId);
            var subProductsIds = offer.SubProductIds;

            bool isValid = new HashSet<ulong>(productIds).IsSupersetOf(subProductsIds);
            return isValid;
        }

        //here should be some smart hash code function. for now hello stackoverflow 
        private int GetSequenceHashCode(List<Product> products)
        {
            var sequence = products.Select(p => p.ProductId);

            const int seed = 487;
            const int modifier = 31;

            unchecked
            {
                return sequence.Aggregate(seed, (current, item) =>
                    (current * modifier) + item.GetHashCode());
            }
        }
    }
}
