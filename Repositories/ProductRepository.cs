using Backend.Models;
using Backend.Services;

namespace Backend.Repositories
{
    public interface IProductRepository
    {
        Task<string> AddPoductToDatabase(Product product);
        List<Product> GetAll();
    }
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoDBService<Product> _mongoService;
        const string collectionName = "products";
        public ProductRepository(IMongoDBService<Product> mongoService)
        {
            _mongoService = mongoService;
        }
        public async Task<string> AddPoductToDatabase(Product product)
        {
            product.Id = Guid.NewGuid().ToString("N");
            return await _mongoService.AddToDatabase(product,collectionName);
        }

        public List<Product> GetAll()
        {
            return _mongoService.GetAll(collectionName);
        }
    }
}
