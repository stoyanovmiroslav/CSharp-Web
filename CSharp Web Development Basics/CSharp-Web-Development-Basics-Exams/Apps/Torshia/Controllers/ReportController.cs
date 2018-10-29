using Microsoft.EntityFrameworkCore;
using SIS.HTTP.Responses;
using SIS.MvcFramework;
using System;
using System.Globalization;
using System.Linq;
using Torshia.Models;
using Torshia.Models.Enums;
using Torshia.ViewModels.Report;

namespace Torshia.Controllers
{
    public class ReportController : BaseController
    {
        [Authorize]
        public IHttpResponse Create(int id)
        {
            var task = db.Tasks.Include(x => x.Report).FirstOrDefault(x => x.Id == id);

            if (task == null)
            {
                return BadRequestError("Invalid task!");
            }

            //75 % chance - Completed(1)
            //25 % chance – Archived(2)
            int[] status = { 1, 1, 1, 2 };
            int statusIndex = new Random().Next(0, 4);

            var user = db.Users.FirstOrDefault(x => x.Username == this.User.Username);

            task.IsReported = true;

            task.Report = new Report
            {
                Reporter = user,
                ReportedOn = DateTime.UtcNow,
                Status = (Status)status[statusIndex]
            };

            db.SaveChanges();

            return this.Redirect("/");
        }

        [Authorize]
        public IHttpResponse All()
        {
            if (!db.Users.Any(x => x.Username == this.User.Username && x.Role == Role.Admin))
            {
                return this.View("/User/Login");
            }

            var reports = db.Reports.Include(x => x.Task.AffectedSectors)
                                    .ToArray();

            return this.View(reports);
        }

        [Authorize]
        public IHttpResponse Details(int id)
        {
            if (!db.Users.Any(x => x.Username == this.User.Username && x.Role == Role.Admin))
            {
                return this.View("/User/Login");
            }

            var report = db.Reports.Include(x => x.Task)
                                   .Include(x => x.Reporter)
                                   .Include(x => x.Task.AffectedSectors)
                                   .FirstOrDefault(x => x.Id == id);

            var affectedSectors = db.TaskSectors.Where(x => x.TaskId == report.TaskId)
                                                .Select(x => x.Sector.Name).ToList();

            var reportModed = new ReportDetailsViewModel
            {
                ReportId = report.Id,
                Reporter = report.Reporter.Username,
                Description = report.Task.Description,
                Title = report.Task.Title,
                Status = report.Status.ToString(),
                Participants = report.Task.Participants,
                ReportedOn = report.ReportedOn.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                DueDate = report.Task.DueDate?.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture),
                AffectedSectors = string.Join(", ", affectedSectors),
                AffectedSectorsCount = affectedSectors.Count
            };
            
            return this.View(reportModed);
        }
    }
}