using RestApiPractice.Models;

namespace RestApiPractice.DataLayer.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductModel>> GetAllAsync();
        Task<ProductModel?> GetByIdAsync(int id);
        Task CreateAsync(ProductModel product);
        Task<bool> UpdateAsync(int id, ProductModel product);
        Task<bool> DeleteAsync(int id);
    }
}
