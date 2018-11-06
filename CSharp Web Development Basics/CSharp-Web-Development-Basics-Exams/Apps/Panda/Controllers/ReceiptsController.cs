using Microsoft.EntityFrameworkCore;
using Panda.Models;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Globalization;
using System.Linq;
using Panda.ViewModels.Receipts;
using Panda.Models.Enums;

namespace Panda.Controllers
{
    public class ReceiptsController : BaseController
    {
        [Authorize]
        public IHttpResponse Receipts()
        {
            var receipts = this.db.Receipts.Where(x => x.Recipient.Username == this.User.Username)
                                           .Include(x => x.Recipient)
                                           .Include(x => x.Package)
                                           .ToArray();

            return this.View(receipts);
        }


        [Authorize]
        public IHttpResponse Index()
        {
           return this.View();
        }

        [Authorize]
        public IHttpResponse Create(int id)
        {
            var package = this.db.Packages
                                 .Include(x => x.Recipient)
                                 .FirstOrDefault(x => x.Id == id && x.Recipient.Username == this.User.Username);

            if (package == null)
            {
                return this.BadRequestError("Invalid package!");
            }

            package.Status = Status.Acquired;
            decimal fee = (decimal)package.Weight * 2.67M;

            var receipe = new Receipt
            { 
                Recipient = package.Recipient,
                Package = package,
                Fee = fee,
                IssuedOn = DateTime.Now
            };

            this.db.Receipts.Add(receipe);
            this.db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            var receipt = this.db.Receipts.Include(x => x.Package)
                                          .Include(x => x.Recipient)
                                          .FirstOrDefault(x => x.Id == id);

            if (receipt == null)
            {
                return this.BadRequestError("Invalid receipt!");
            }

            var receiptViewModel = new ReceiptDetailsViewModel
            {
                Description = receipt.Package.Description,
                Fee = receipt.Fee,
                Recipient = receipt.Recipient.Username,
                IssuedOn = receipt.IssuedOn.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                Id = receipt.Id,
                ShippingAddress = receipt.Package.ShippingAddress,
                Weight = receipt.Package.Weight
            };

            return this.View(receiptViewModel);
        }
    }
}