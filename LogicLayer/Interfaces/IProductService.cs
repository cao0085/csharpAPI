using RestApiPractice.Models;

namespace RestApiPractice.LogicLayer.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductModel>> GetAllProductsAsync();
        Task<ProductModel?> GetProductByIdAsync(int id);
        Task CreateProductAsync(ProductModel product);
        Task<bool> UpdateProductAsync(int id, ProductModel product);
        Task<bool> DeleteProductAsync(int id);
    }
}
