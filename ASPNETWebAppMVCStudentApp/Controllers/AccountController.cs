
using System.Linq;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using ASPNETWebAppMVCStudentApp.Models;

namespace ASPNETWebAppMVCStudentApp.Controllers
{
    public class AccountController : Controller
    {
        private SchoolDBEntities db = new SchoolDBEntities();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = db.tblUsers.FirstOrDefault(
                u => u.Username == model.Username);

            if (user != null &&
                Crypto.VerifyHashedPassword(
                    user.PasswordHash, model.Password))
            {
                FormsAuthentication.SetAuthCookie(
                    user.Username, false);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(
                "", "Invalid username or password");

            return View(model);
        }

        [HttpGet]
        public ActionResult SetupAdmin()
        {
            if (!Request.IsLocal)
                return new HttpStatusCodeResult(403);

            if (db.tblUsers.Any())
                return Content("An account already exists.");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SetupAdmin(
            string username, string password)
        {
            if (!Request.IsLocal || db.tblUsers.Any())
                return new HttpStatusCodeResult(403);

            if (string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                password.Length < 8)
            {
                ModelState.AddModelError(
                    "", "Enter a username and password of at least 8 characters.");

                return View();
            }

            db.tblUsers.Add(new tblUser
            {
                Username = username.Trim(),
                PasswordHash = Crypto.HashPassword(password)
            });

            db.SaveChanges();

            return RedirectToAction("Login");
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();

            base.Dispose(disposing);
        }
    }
}
