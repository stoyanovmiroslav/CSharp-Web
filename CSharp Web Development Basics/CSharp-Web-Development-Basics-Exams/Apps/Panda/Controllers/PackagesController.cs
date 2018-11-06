using Microsoft.EntityFrameworkCore;
using Panda.Models;
using Panda.Models.Enums;
using Panda.ViewModels.Packages;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Globalization;
using System.Linq;

namespace Panda.Controllers
{
    public class PackagesController : BaseController
    {
        [Authorize]
        public IHttpResponse Details(int id)
        {
            var package = this.db.Packages
                                 .Include(x => x.Recipient)
                                 .FirstOrDefault(x => x.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Invalid Package");
            }

            string estimatedDeliveryDate = "N/A";

            if (package.Status == Status.Delivered || package.Status == Status.Acquired)
            {
                estimatedDeliveryDate = "Delivered";
            }
            else if (package.Status == Status.Shipped)
            {
                estimatedDeliveryDate = package.EstimatedDeliveryDate?
                                               .ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
            }

            var packageViewModel = new DetailsPackageViewModel
            {
                Recipient = package.Recipient.Username,
                ShippingAddress = package.ShippingAddress,
                Status = package.Status.ToString(),
                Weight = package.Weight.ToString(),
                Description = package.Description,
                EstimatedDeliveryDate = estimatedDeliveryDate
            };

            return this.View(packageViewModel);
        }

        [Authorize("Admin")]
        public IHttpResponse Delivered()
        {
            Package[] packages = this.db.Packages.Include(x => x.Recipient)
                                                 .Include(x => x.Receipt)
                                                 .Where(x => x.Status == Status.Delivered || x.Status == Status.Acquired)
                                                 .ToArray();

            return this.View(packages);
        }

        [Authorize("Admin")]
        public IHttpResponse ShippedAction(int id)
        {
            var package = this.db.Packages.Include(x => x.Recipient)
                                          .Include(x => x.Receipt)
                                          .FirstOrDefault(x => x.Status == Status.Shipped && x.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Invalid package");
            }

            var randomDays = new Random().Next(20, 41);

            package.Status = Status.Delivered;
            this.db.SaveChanges();


            return this.Redirect("/");
        }


        [Authorize("Admin")]
        public IHttpResponse Shipped()
        {
            Package[] packages = this.db.Packages.Include(x => x.Recipient)
                                                 .Include(x => x.Receipt)
                                                 .Where(x => x.Status == Status.Shipped)
                                                 .ToArray();

            return this.View(packages);
        }

        [Authorize("Admin")]
        public IHttpResponse PendingAction(int id)
        {
            var package = this.db.Packages.Include(x => x.Recipient)
                                           .Include(x => x.Receipt)
                                           .FirstOrDefault(x => x.Status == Status.Pending && x.Id == id);

            if (package == null)
            {
                return this.BadRequestError("Invalid package");
            }

            var randomDays = new Random().Next(20, 41);
            var estimatedDeliveryDate = DateTime.UtcNow.AddDays(randomDays);

            package.Status = Status.Shipped;
            package.EstimatedDeliveryDate = estimatedDeliveryDate;
            this.db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize("Admin")]
        public IHttpResponse Pending()
        {
            Package[] packages = null;

            packages = this.db.Packages.Include(x => x.Recipient)
                                       .Include(x => x.Receipt)
                                       .Where(x => x.Status == Status.Pending)
                                       .ToArray();

            return this.View(packages);
        }

        [Authorize("Admin")]
        public IHttpResponse Create()
        {
            var users = this.db.Users.ToArray();

            return this.View(users);
        }

        [Authorize("Admin")]
        [HttpPost]
        public IHttpResponse Create(CreatePackageViewModel model)
        {
            var user = this.db.Users.FirstOrDefault(x => x.Id == model.UserId);

            if (user == null)
            {
                return this.BadRequestError("Invalid user!");
            }

            var package = new Package
            {
                Description = model.Description,
                ShippingAddress = model.ShippingAddress,
                Weight = model.Weight,
                Status = Status.Pending,
                EstimatedDeliveryDate = null,
                Recipient = user
            };

            user.Packages.Add(package);
            this.db.SaveChanges();


            return this.Redirect("/");
        }
    }
}