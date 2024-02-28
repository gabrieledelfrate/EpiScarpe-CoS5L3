namespace EpiScarpe_Co.Models
{
    public static class ProductRepository
    {
        private static List<Product> products = new List<Product>
    {
        new Product
        {
            Id = 1,
            Name = "Tennis Shoes 1",
            Price = 49.99m,
            Description = "High-quality tennis shoes with advanced features.",
            CoverImage = "/images/shoes1.jpg",
            AdditionalImage1 = "/images/shoes1_1.jpg",
            AdditionalImage2 = "/images/shoes1_2.jpg",
            IsDisplayedOnHomePage = true
        },
       
    };

        public static List<Product> GetAllProducts()
        {
            return products;
        }

        public static void AddProduct(Product product)
        {
            product.Id = products.Count + 1;
            products.Add(product);
        }

        public static void RemoveProduct(int productId)
        {
            var productToRemove = products.FirstOrDefault(p => p.Id == productId);
            if (productToRemove != null)
            {
                products.Remove(productToRemove);
            }
        }
    }

}
