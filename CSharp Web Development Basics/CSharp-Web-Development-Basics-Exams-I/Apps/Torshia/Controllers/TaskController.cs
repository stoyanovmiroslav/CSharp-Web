using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Torshia.Models;
using Torshia.ViewModels;
using Torshia.ViewModels.Task;

namespace Torshia.Controllers
{
    public class TaskController : BaseController
    {
        public IHttpResponse Details(int id)
        {
            if (this.User.Username == null)
            {
                return this.View("/Home/GuestIndex");
            }

            var task = db.Tasks.Include(x => x.AffectedSectors)
                               .FirstOrDefault(x => x.Id == id);

            var sectors = db.TaskSectors.Include(x => x.Sector)
                                        .Where(x => x.TaskId == task.Id)
                                        .Select(x => x.Sector.Name).ToList();

            var model = new DetailsTaskViewModel
            {
                AffectedSectorsCount = sectors.Count(),
                AffectedSectors = string.Join(", ", sectors),
                Description = task.Description,
                DueDate = task.DueDate?.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                Participants = task.Participants,
                Title = task.Title
            };
            
            return this.View(model);
        }

        public IHttpResponse Create()
        {
            if (this.User.Username == null)
            {
                return this.View("/Home/GuestIndex");
            }

            return this.View();
        }

        [HttpPost]
        public IHttpResponse Create(CreateTaskViewModel model)
        {
            if (this.User.Username == null)
            {
                return this.View("/Home/GuestIndex");
            }

            var task = model.To<Task>();
            if (DateTime.TryParse(model.DueDate, out DateTime dueDate))
            {
                task.DueDate = dueDate;
            }

            var properties = model.GetType().GetProperties();

            var taskSectors = new List<TaskSector>();
            foreach (var property in properties)
            {
                var value = property.GetValue(model);
                if (value != null)
                {
                    bool isValidSector = Enum.TryParse(typeof(Models.Enums.Sector), value.ToString(), out object sectorValue);

                    if (isValidSector)
                    {
                        var sector = new Models.Sector { Name = (Models.Enums.Sector)sectorValue };
                        var taskSector = new TaskSector { Task = task, Sector = sector };
                        taskSectors.Add(taskSector);
                    }
                }
            }

            task.AffectedSectors.AddRange(taskSectors);
            db.Tasks.Add(task);
            db.SaveChanges();

            return this.Redirect("/");
        }
    }
}