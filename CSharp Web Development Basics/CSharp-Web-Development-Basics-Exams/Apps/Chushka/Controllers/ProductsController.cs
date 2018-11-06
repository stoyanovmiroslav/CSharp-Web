using Chushka.Models;
using Chushka.Models.Enums;
using Chushka.ViewModels.Products;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Linq;

namespace Chushka.Controllers
{
    public class ProductsController : BaseController
    {
        [Authorize("Admin")]
        public IHttpResponse Delete(int id)
        {
            var product = this.db.Products.Where(x => x.IsDeleted == false)
                                          .FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Invalid product!");
            }

            return this.View(product);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Delete(int id, string name)
        {
            var product = this.db.Products.Where(x => x.IsDeleted == false)
                                           .FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Invalid product!");
            }

            product.IsDeleted = true;
            this.db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Edit(int id)
        {
            var product = this.db.Products.Where(x => x.IsDeleted == false)
                                          .FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Invalid product!");
            }


            return this.View(product);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Edit(EditProductViewModel model)
        {
            var product = this.db.Products.Where(x => x.IsDeleted == false)
                                         .FirstOrDefault(x => x.Id == model.Id);

            if (product == null)
            {
                return this.BadRequestError("Invalid product!");
            }

            if (!Enum.TryParse(model.ProductType, out ProductType productType))
            {
                return this.BadRequestError("Invalid product type!");
            }

            product.Name = model.Name;
            product.Price = model.Price;
            product.Description = model.Description;
            product.ProductType = productType;

            this.db.SaveChanges();

            return this.Redirect("/Products/Details?id=" + model.Id);
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            return this.View();
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(CreteProductViewModel model)
        {

            if (!Enum.TryParse(model.ProductType, out ProductType productType))
            {
                return this.BadRequestErrorWithView("Invalid product type!");
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

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var product = this.db.Products.Where(x => x.IsDeleted == false)
                                           .FirstOrDefault(x => x.Id == id);

            if (product == null)
            {
                return this.BadRequestError("Invalid product!");
            }

            return this.View(product);
        }
    }
}