using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using AccessControlSystem.Domain;
using AccessControlSystem.Models;

namespace AccessControlSystem.Controllers
{
    public class VaultController : Controller
    {
        private ISession _session;

        public ISession DbSession
        {
            get { return _session ?? (_session = HttpContext.GetOwinContext().Get<ISession>()); }
        }


        // GET: Vault
        public ActionResult Index()
        {
            var allVault = DbSession.Query<Vault>().Select(x => new VaultViewModel {
                AdminId = x.Admin.Id,
                Id = x.Id,
                Name = x.Name
            }).ToArray();

            return View(allVault);
        }

        // GET: Vault/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(VaultViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var transaction = DbSession.BeginTransaction())
                    {
                        var vault = new Vault { Name = model.Name };
                        DbSession.Save(vault);

                        transaction.Commit();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Неизвестная ошибка");
                }
            }

            return View(model);
        }

        public ActionResult Edit(int id)
        {
            var vault = DbSession.Get<Vault>(id);
            if (vault == null)
                return this.HttpNotFound();

            return View(new VaultViewModel { Id = vault.Id, Name = vault.Name });
        }

        [HttpPost]
        public ActionResult Edit(VaultViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (var transaction = DbSession.BeginTransaction())
                    {
                        var vault = DbSession.Get<Vault>(model.Id);
                        vault.Name = model.Name;

                        DbSession.Save(vault);

                        transaction.Commit();
                    }

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Неизвестная ошибка");
                }
            }

            return View(model);
        }

        // GET: Vault/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Vault/Delete/5
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
