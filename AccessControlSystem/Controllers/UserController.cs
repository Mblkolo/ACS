using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using AccessControlSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;


namespace AccessControlSystem.Controllers
{
    public class UserController : Controller
    {
        private ISession _session;
        //private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public ISession DbSession
        {
            get { return _session ?? (_session = HttpContext.GetOwinContext().Get<ISession>()); }
        }

        //public ApplicationSignInManager SignInManager
        //{
        //    get
        //    {
        //        return _signInManager ?? (_signInManager = HttpContext.GetOwinContext().Get<ApplicationSignInManager>());
        //    }
        //}

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? (_userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>());
            }
        }

        // GET: User
        public ActionResult Index()
        {
            var users = UserManager.Users
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    Name = x.UserName
                })
                .ToArray();

            return View(users);
        }

        // GET: User/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var transaction = DbSession.BeginTransaction())
                {
                    var user = new ApplicationUser { UserName = model.Name, Email = model.Email, EmailConfirmed = true };
                    var result = await UserManager.CreateAsync(user, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        return View(model);
                    }

                    transaction.Commit();
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Что-то пошло не так");
                return View(model);
            }
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            ApplicationUser user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            var userVM = new UserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.UserName
            };
            return View(userVM);
        }

        // POST: User/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(UserViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                using (var transaction = DbSession.BeginTransaction())
                {
                    ApplicationUser user = await UserManager.FindByIdAsync(model.Id);
                    if (user == null)
                        return HttpNotFound();

                    user.UserName = model.Name;
                    user.Email = model.Email;
                    //user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.Password);

                    IdentityResult result = await UserManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        return View(model);
                    }

                    string resetToken = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    result = await UserManager.ResetPasswordAsync(user.Id, resetToken, model.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                        return View(model);
                    }

                    transaction.Commit();
                }

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Неизвестная ошибка");
                return View(model);
            }
        }

        // GET: User/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: User/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
