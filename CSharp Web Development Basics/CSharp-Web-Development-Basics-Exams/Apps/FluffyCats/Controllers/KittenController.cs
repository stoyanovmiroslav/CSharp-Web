using FluffyCats.Models;
using FluffyCats.ViewModels.Kitten;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Linq;

namespace FluffyCats.Controllers
{
    public class KittenController : BaseController
    {
        [Authorize]
        public IHttpResponse All()
        {
            var kitten = db.Kittens.ToArray();

            return this.View(kitten);
        }

        [Authorize]
        public IHttpResponse Add()
        {
            return this.View();
        }

        [Authorize]
        [HttpPost]
        public IHttpResponse Add(CreateKittenViewModel model)
        {
            var breedAsString = model.Breed.Trim().Replace(" ", "_");

            if (!Enum.TryParse(typeof(Breed), breedAsString, out object breed))
            {
               return this.BadRequestErrorWithView("Invalid Breed, please try again!", "/Kitten/Add");
            }

            var kitten = new Kitten
            {
                Name = model.Name,
                Age = model.Age,
                Breed = (Breed)breed,
                Url = model.Url
            };

            db.Kittens.Add(kitten);
            db.SaveChanges();

            return this.Redirect("/Kitten/All");
        }
    }
}