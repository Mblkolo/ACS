﻿using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using AccessControlSystem.Domain;
using AccessControlSystem.Models;
using NHibernate.AspNet.Identity;

namespace AccessControlSystem.Controllers
{
    [Authorize]
    public class VaultAdminController : Controller
    {
        private ISession _session;

        public ISession DbSession
        {
            get { return _session ?? (_session = HttpContext.GetOwinContext().Get<ISession>()); }
        }

        // GET: VaultAdmin
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();

            //Все доступные пользователю хранилища
            var vaults = DbSession.Query<Vault>()
                .Where(x => x.Admin.Id == currentUserId)
                .Select(x => new VaultAdminIndexViewModel
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToArray();

            return View(vaults);
        }

        public ActionResult Details(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            var vault = DbSession.Query<Vault>()
                .Where(x => x.Admin.Id == currentUserId && x.Id == id)
                .SingleOrDefault();

            if (vault == null)
                return HttpNotFound();

            var vaultDetailVM = new VaultAdminDetaisViewModel
                {
                    Id = vault.Id,
                    Name = vault.Name,
                    CloseTime = vault.ClosingTime,
                    OpeningTime = vault.OpeningTime,
                    Users = vault.Users.Select(y => y.User.UserName).ToArray()
                };

            return View(vaultDetailVM);
        }

        public ActionResult UsersAccess(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            var vault = DbSession.Query<Vault>()
                .Where(x => x.Admin.Id == currentUserId && x.Id == id)
                .SingleOrDefault();

            if (vault == null)
                return HttpNotFound();


            var trustedUsers = vault.Users.ToDictionary(x => x.User.Id);
            var users = DbSession.Query<IdentityUser>()
                .Select(x => new UserSelectViewModel
                {
                    Email = x.Email,
                    Id = x.Id,
                    Name = x.UserName,
                    IsSelected = trustedUsers.ContainsKey(x.Id)
                })
                .ToArray();

            var model = new VaultAdminUsersAccessViewModel
            {
                Id = vault.Id,
                Name = vault.Name,
                Users = users
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult UserAccess(string id, string[] userId)
        {
            //Даёт и забирает права пользователя на посещение хранилища
            return View();
        }

        public ActionResult OpenHours(string id)
        {
            return View();
        }


        [HttpPost]
        public ActionResult ChangeOpenHours(string id, DateTime openTime, DateTime closeTime)
        {
            return View();
        }

    }
}
