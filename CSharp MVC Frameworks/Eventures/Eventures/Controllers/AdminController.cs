using AutoMapper;
using Eventures.Models;
using Eventures.Models.ViewModel;
using Eventures.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Eventures.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminService adminService;
        private readonly IMapper mapper;

        public AdminController(IAdminService adminService, IMapper mapper)
        {
            this.adminService = adminService;
            this.mapper = mapper;
        }

        public IActionResult ChangeRole()
        {
            IList<EventuresUser> users = this.adminService.GetAllUsers();

            var usersViewModel = mapper.Map<IList<EventuresUser>, IList<AdminUserViewModel>>(users);

            return View(usersViewModel);
        }

        public IActionResult Demote(string id)
        {
            this.adminService.Demote(id);

            return RedirectToAction(nameof(ChangeRole));
        }

        public IActionResult Promote(string id)
        {
            this.adminService.Promote(id);

            return RedirectToAction(nameof(ChangeRole));
        }
    }
}