using RestApiPractice.Models;

namespace RestApiPractice.DataLayer.Repositories
{
    public class ProductRepository : IProductRepository
    {
        // 假資料模擬資料庫
        private static readonly List<ProductModel> _products = new List<ProductModel>
        {
            new ProductModel { Id = 1, Name = "Apple", Price = 30 },
            new ProductModel { Id = 2, Name = "Banana", Price = 15 }
        };

        public Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<ProductModel>>(_products);
        }

        public Task<ProductModel?> GetByIdAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(product);
        }

        public Task CreateAsync(ProductModel product)
        {
            // 直接模擬新增
            int nextId = _products.Any() ? _products.Max(p => p.Id) + 1 : 1;
            product.Id = nextId;
            _products.Add(product);
            return Task.CompletedTask;
        }

        public Task<bool> UpdateAsync(int id, ProductModel product)
        {
            var existingProduct = _products.FirstOrDefault(p => p.Id == id);
            if (existingProduct == null)
                return Task.FromResult(false);

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            return Task.FromResult(true);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
                return Task.FromResult(false);

            _products.Remove(product);
            return Task.FromResult(true);
        }
    }
}
