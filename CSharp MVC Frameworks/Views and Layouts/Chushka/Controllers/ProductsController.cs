using Models.ViewModels.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Chushka.Controllers
{
    public class ProductsController : BaseController
    {
        private IProductService productsService;

        public ProductsController(IProductService productsService)
        {
            this.productsService = productsService;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            productsService.Create(model);

            return this.Redirect("/");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            var product = productsService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                ProductType = product.ProductType.ToString(),
            };

            return this.View(productViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var product = productsService.GetProduct(model.Id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            productsService.EditProduct(product, model);

            return this.Redirect("/Products/Details?id=" + model.Id);
        }

        [Authorize]
        public IActionResult Details(int id)
        {
            var product = productsService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                ProductType = product.ProductType.ToString(),
            };

            return this.View(productViewModel);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var product = productsService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            var productViewModel = new ProductViewModel
            {
                Id = product.Id,
                Description = product.Description,
                Name = product.Name,
                Price = product.Price,
                ProductType = product.ProductType.ToString(),
                DisableValue = "disabled"
            };

            return this.View(productViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAction(int id)
        {
            var product = productsService.GetProduct(id);

            if (product == null)
            {
                return this.Redirect("/");
            }

            productsService.Delete(product);

            return this.Redirect("/");
        }
    }
}