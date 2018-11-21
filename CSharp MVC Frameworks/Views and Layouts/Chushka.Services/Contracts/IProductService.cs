using Chushka.Models.ViewModels.Products;
using Chushka.Models;

namespace Chushka.Services.Contracts
{
    public interface IProductService
    {
        Product GetProduct(int id);

        bool Create(ProductViewModel model);

        bool EditProduct(Product product, ProductViewModel model);

        void Delete(Product product);

        Product[] GetProducts();
    }
}