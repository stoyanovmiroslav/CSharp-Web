using System;
using System.Linq;
using Models;
using Models.Enums;
using Data;
using Services.Contracts;
using Models.ViewModels.Products;

namespace Services
{
    public class ProductService : IProductService
    {
        private ChushkaDbContext db;

        public ProductService(ChushkaDbContext db)
        {
            this.db = db;
        }

        public bool Create(ProductViewModel model)
        {
            if (!Enum.TryParse(model.ProductType, out ProductType productType))
            {
                return false;
            }

            var product = new Product
            {
                Price = model.Price,
                Name = model.Name,
                ProductType = productType,
                Description = model.Description
            };

            this.db.Products.Add(product);
            this.db.SaveChanges();

            return true;
        }

        public void Delete(Product product)
        {
            product.IsDeleted = true;
            this.db.SaveChanges();
        }

        public bool EditProduct(Product product, ProductViewModel model)
        {
            if (!Enum.TryParse(model.ProductType, out ProductType productType))
            {
                return false;
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.ProductType = productType;

            this.db.SaveChanges();

            return true;
        }

        public Product[] GetProducts()
        {
            var products = this.db.Products.Where(x => x.IsDeleted == false).ToArray();

            return products;
        }

        public Product GetProduct(int id)
        {
            var product = this.db.Products.Where(x => x.IsDeleted == false)
                                          .FirstOrDefault(x => x.Id == id);

            return product;
        }
    }
}