using RestApiPractice.DataLayer.Repositories;
using RestApiPractice.LogicLayer.Interfaces;
using RestApiPractice.Models;

namespace RestApiPractice.LogicLayer
{
    public class ProductSelectionLogic : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductSelectionLogic(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductModel>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<ProductModel?> GetProductByIdAsync(int id)
        {
            return await _productRepository.GetByIdAsync(id);
        }

        public async Task CreateProductAsync(ProductModel product)
        {
            await _productRepository.CreateAsync(product);
        }

        public async Task<bool> UpdateProductAsync(int id, ProductModel product)
        {
            return await _productRepository.UpdateAsync(id, product);
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteAsync(id);
        }
    }
}
