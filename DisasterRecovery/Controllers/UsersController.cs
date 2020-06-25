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
        [HttpPost]
        public ActionResult RecoverAccount(string receiver, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var senderEmail = new MailAddress("jamilmoughal786@gmail.com", "Jamil");
                    var receiverEmail = new MailAddress(receiver, "Receiver");
                    var password = "Your Email Password here";
                    var sub = "Recover Your Password";
                    var body = "Use this Link to Recover your Password" ;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = sub,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
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
