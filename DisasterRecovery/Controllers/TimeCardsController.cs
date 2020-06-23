using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DisasterRecovery.Models;

namespace DisasterRecovery.Controllers
{
    public class TimeCardsController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        // GET: TimeCards
        public ActionResult Index()
        {
            var timeCards = db.TimeCards.Include(t => t.SiteLocation).Include(t => t.SubContractor).Include(t => t.User);
            return View(timeCards.ToList());
        }

        public ActionResult IndexAdm()
        {
            var timeCards = db.TimeCards.Include(t => t.SiteLocation).Include(t => t.SubContractor).Include(t => t.User);
            return View(timeCards.ToList());
        }

        public ActionResult Approve(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeCard timeCard = db.TimeCards.Find(id);
            if (timeCard == null)
            {
                return HttpNotFound();
            }
            
            timeCard.TimeStatus = "Finalized";
            db.Entry(timeCard).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexAdm");
        }
        // GET: TimeCards/ReviewSubmission/5
        public ActionResult ReviewSubmission(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeCard timeCard = db.TimeCards.Find(id);
            if (timeCard == null)
            {
                return HttpNotFound();
            }
            return View(timeCard);
        }

        // GET: TimeCards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeCard timeCard = db.TimeCards.Find(id);
            if (timeCard == null)
            {
                return HttpNotFound();
            }
            return View(timeCard);
        }

        // GET: TimeCards/Create
        public ActionResult Create()
        {
            ViewBag.JobID = new SelectList(db.JobLists, "IdJobList", "JobName");
            ViewBag.SiteID = new SelectList(db.SiteLocations, "SiteID", "LocationName");
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName");
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "UserName");
            return View();
        }

        // POST: TimeCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTimeCard,SiteID,IdSubContractor")] TimeCard timeCard)
        {
           
            if (ModelState.IsValid)
            {
                timeCard.RegDate = DateTime.Now;
                timeCard.TimeStatus = "Open";


                timeCard.IdUser = Convert.ToInt32(Session["LogedUserID"]);

                db.TimeCards.Add(timeCard);
                db.SaveChanges();

                TempData["TimeCardID"] = timeCard.IdTimeCard;
               
                return RedirectToAction("Create", "TimeCardDetails");
            }

            ViewBag.SiteID = new SelectList(db.SiteLocations, "SiteID", "LocationName", timeCard.SiteID);
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", timeCard.IdSubContractor);
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "UserName", timeCard.IdUser);
            return View(timeCard);
        }

        // GET: TimeCards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeCard timeCard = db.TimeCards.Find(id);
            if (timeCard == null)
            {
                return HttpNotFound();
            }
            ViewBag.SiteID = new SelectList(db.SiteLocations, "SiteID", "LocationName", timeCard.SiteID);
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", timeCard.IdSubContractor);
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "UserName", timeCard.IdUser);
            return View(timeCard);
        }

        // POST: TimeCards/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTimeCard,SiteID,IdSubContractor,TotalHours,TotalAmount,TimeStatus,RegDate,IdUser")] TimeCard timeCard)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeCard).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SiteID = new SelectList(db.SiteLocations, "SiteID", "LocationName", timeCard.SiteID);
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", timeCard.IdSubContractor);
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "UserName", timeCard.IdUser);
            return View(timeCard);
        }

        // GET: TimeCards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeCard timeCard = db.TimeCards.Find(id);
            if (timeCard == null)
            {
                return HttpNotFound();
            }
            return View(timeCard);
        }

        // POST: TimeCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeCard timeCard = db.TimeCards.Find(id);
            db.TimeCards.Remove(timeCard);

            var tDetails = db.TimeCardDetails.Where(v => v.IdTimeCard == id);

            if (tDetails != null)
            {
                db.TimeCardDetails.RemoveRange(tDetails);
            }

            db.SaveChanges();
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
