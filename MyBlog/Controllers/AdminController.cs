using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Areas.Identity.Data;
using MyBlog.Models;
using MyBlog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    public class AdminController : Controller
    {
        
        private readonly RoleManager<IdentityRole> roleManager;

        private readonly UserManager<MyBlogUser> userManager;

        private readonly IDbRepository repository;

        public AdminController(RoleManager<IdentityRole> roleManager, UserManager<MyBlogUser> _userManager, IDbRepository dbRepository)
        {
            this.roleManager = roleManager;
            repository = dbRepository;
            userManager = _userManager;
        }

        [Authorize(Policy = "readpolicy")]
        public IActionResult Index()
        {
            var roles = roleManager.Roles.ToList();
            return View(roles);
        }

        [Authorize(Policy = "writepolicy")]
        public IActionResult Create()
        {
            return View(new IdentityRole());
        }
        
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role) 
        {
            await roleManager.CreateAsync(role);
            return RedirectToAction("Index");
        }

        public IActionResult UserList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getUserList() {
            return Json(new { data = await repository.GetUsersAsync() });
        }

        [HttpGet]
        public async Task<IActionResult> editUserRole(string userId) {

            ViewBag.userId = userId;
            var user = await userManager.FindByIdAsync(userId);

            var userRoleViews = new List<UserRoleView>();
            foreach (var role in roleManager.Roles) {
                
                var userRoleView = new UserRoleView();
      
                userRoleView.RoleId = role.Id;
                userRoleView.RoleName = role.Name;

                userRoleView.isChecked = (await userManager.IsInRoleAsync(user, role.Name)) ? true : false;

                userRoleViews.Add(userRoleView);
            }
            
            return View(userRoleViews);
        }

        [HttpPost]
        public async Task<IActionResult> editUserRole(List<UserRoleView> model,string userId) {

            var user =await userManager.FindByIdAsync(userId);

            if (user == null) {
                ViewBag.ErrorMessage = "User with " + userId + " can't find";
                return NotFound();
            }

            var roles = await userManager.GetRolesAsync(user);
            var result = await userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded) {

                ModelState.AddModelError("","Cannot remove user roles.");
                return View(model);

            }
            var rolesAdded = await userManager.AddToRolesAsync(user, model.Where(x => x.isChecked).Select(r => r.RoleName));

            if (!rolesAdded.Succeeded) {
                ModelState.AddModelError("", "Cannot add roles to user.");
                return View(model);
            }

            return RedirectToAction("UserList");
        }

    }
}
