using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using DisasterRecovery.Infrastructure;
using DisasterRecovery.Models;

namespace DisasterRecovery.Controllers
{
   // [CustomAuthenticationFilter]
    public class UsersController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        // GET: Users
        [CustomAuthorize("Admin")]
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.SubContractor);
            return View(users.ToList());
        }

        // GET: Users/Details/5
        [CustomAuthorize("Admin", "Contractor")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // GET: Users/Create
        [CustomAuthorize("Admin")]
        public ActionResult Create()
        {
            ViewBag.IdSubContractor = new SelectList(db.SubContractors.Where(s => s.SubContractorStatus == 1), "IdSubContractor", "SubContractorName");
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public ActionResult Create([Bind(Include = "IdUser,UserName,FirstName,LastName,Email,UserPassWord,UserStatus,IsAdm,IdSubContractor")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserPassWord = LoginController.HashPaswd(user.UserPassWord);
                db.Users.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", user.IdSubContractor);
            return View(user);
        }

        // GET: Users/Edit/5
        [CustomAuthorize("Admin", "Contractor")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", user.IdSubContractor);
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin","Contractor")]
        public ActionResult Edit([Bind(Include = "IdUser,UserName,FirstName,LastName,Email,UserPassWord,UserStatus,IsAdm,IdSubContractor")] User user)
        {
            if (ModelState.IsValid)
            {
                user.UserPassWord = LoginController.HashPaswd(user.UserPassWord);
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", user.IdSubContractor);
            return View(user);
        }
        [CustomAuthorize("Admin", "Contractor")]
        public ActionResult EditPass(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", user.IdSubContractor);
            return View(user);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin", "Contractor")]
        public ActionResult EditPass([Bind(Include = "IdUser,UserName,FirstName,LastName,Email,UserPassWord,UserStatus,IsAdm,IdSubContractor")] User user, string LastPassword, string NewPassword, string ComparePassword)
        {
            if (ModelState.IsValid)
            {
                LastPassword = LoginController.HashPaswd(LastPassword);
                if (LastPassword == user.UserPassWord)
            {
                if(NewPassword == ComparePassword)
                {
                        user.UserPassWord = LoginController.HashPaswd(NewPassword);
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                       Session["Message"] = "Password Updated, Please Login again.";
                        return RedirectToAction("Index", "Login");
                       
                    }
            }
                
            }
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", user.IdSubContractor);
            ViewBag.Alert = 1;
            return View(user);
        }

        // GET: Users/Delete/5
        [CustomAuthorize("Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin")]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult UnAuthorized()
        {
            return View();
        }

        public ActionResult RecoverAccount()
        {
            return View();
        }
        public ActionResult ResetPaswd()
        {
            return View();
        }

        [HttpPost, ActionName("ResetPaswd")]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPaswd(int? id)
        {
            if (ModelState.IsValid)
            {
                var userID = db.Users.Find(id);
                userID.UserPassWord = LoginController.HashPaswd(userID.UserName + "1234");
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
