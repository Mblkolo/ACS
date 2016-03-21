using NHibernate;
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
    public class HomeController : Controller
    {
        private ISession _session;

        public ISession DbSession
        {
            get { return _session ?? (_session = HttpContext.GetOwinContext().Get<ISession>()); }
        }

        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            var allVault = DbSession.Query<Vault>()
                .Where(x => x.Users.Any(y => y.User.Id == currentUserId) )
                .Select(x => new VaultViewModel
                {
                    AdminId = x.Admin.Id,
                    Id = x.Id,
                    Name = x.Name
                })
                .ToArray();

            return View(allVault);
        }

        public ActionResult Details(int id)
        {
            var vault = DbSession.Get<Vault>(id);
            if (vault == null)
                return HttpNotFound();

            string currentUserId = User.Identity.GetUserId();
            var user = DbSession.Get<IdentityUser>(currentUserId);
            bool success = CheckAccess(vault, user, DateTime.Now.Hour);



            using (var transaction = DbSession.BeginTransaction())
            {
                //По идее пользователь не должен быть равен null, ибо есть проверка на авторизацию, но где-то кэшируется
                var info = user == null ? "Неизвестный пользователь" : String.Format("{0} ( {1} )", user.UserName, user.Email);

                var logMessage = new VaultAccess
                {
                    AccessTime = DateTime.Now,
                    IsSuccess = success,
                    UserInfo = info,
                    Vault = vault
                };

                DbSession.Save(logMessage);

                transaction.Commit();
            }

            if (!success)
                return HttpNotFound("За вами уже выехали");

            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }

        //нужен тест
        private bool CheckAccess(Vault vault, IdentityUser user, int hour)
        {
            

            if(user == null)
                return false;

            if (vault.Users.All(x => x.User != user))
                return false;

            if (vault.OpeningTime == vault.ClosingTime)
                return false;

            if (vault.OpeningTime < vault.ClosingTime)
                if (hour >= vault.OpeningTime && hour < vault.ClosingTime)
                    return true;

            //через полночь
            if (vault.OpeningTime > vault.ClosingTime)
                if (hour >= vault.OpeningTime || hour < vault.ClosingTime)
                    return true;

            return false;
        }
    }
}