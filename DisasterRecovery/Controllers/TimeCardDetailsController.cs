using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DisasterRecovery.Models;

namespace DisasterRecovery.Controllers
{
    public class TimeCardDetailsController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();
        private static int? timeCardID;
        // GET: TimeCardDetails
        public ActionResult Index()
        {
            var laborPerformed = db.TimeCardDetails.Where(v => v.IdTimeCard == timeCardID);
            var totalHours = laborPerformed.Sum(a => a.Hours);
            var totalAmount = laborPerformed.Sum(l => l.Amount);

            ViewBag.TotalHours = totalHours;
            ViewBag.TotalAmount = totalAmount;
          
            var timeCardDetails = db.TimeCardDetails.Include(t => t.JobList).Include(t => t.TimeCard);
            
            if (timeCardID != null)
            {
                timeCardDetails = timeCardDetails.Where(t => t.IdTimeCard == timeCardID);
            }

            return View(timeCardDetails.ToList());
        }

        public ActionResult Submit()
        {
            if (timeCardID != null)
            {
                var laborPerformed = db.TimeCardDetails.Where(v => v.IdTimeCard == timeCardID);
                var totalHours = laborPerformed.Sum(l => l.Hours);
                var totalAmount = laborPerformed.Sum(l => l.Amount);

                db.TimeCards.Find(timeCardID).TotalHours = totalHours;
                db.TimeCards.Find(timeCardID).TotalAmount = totalAmount;
                db.SaveChanges();
            }

            return RedirectToAction("Index", "TimeCards");
        }

        public ActionResult Cancel()
        {
            var recordsToRemove = db.TimeCardDetails.Find(timeCardID);
            db.TimeCardDetails.Remove(recordsToRemove);
            return RedirectToAction("Index", "TimeCards");
        }


            // GET: TimeCardDetails/Details/5
            public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeCardDetail timeCardDetail = db.TimeCardDetails.Find(id);
            if (timeCardDetail == null)
            {
                return HttpNotFound();
            }
            return View(timeCardDetail);
        }

        // GET: TimeCardDetails/Create
        public ActionResult Create()
        {
            ViewBag.IdJobList = new SelectList(db.JobLists, "IdJobList", "JobName");
            ViewBag.IdTimeCard = new SelectList(db.TimeCards, "IdTimeCard", "IdTimeCard");
            return View();
        }

        // POST: TimeCardDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdJobList,Hours,Amount")] TimeCardDetail timeCardDetail)
        {
            timeCardID = (int)TempData["TimeCardID"];

            if (ModelState.IsValid)
            {
                
                timeCardDetail.IdTimeCard = (int) timeCardID;
                var rate = db.JobLists.Find(timeCardDetail.IdJobList).Rate;
                timeCardDetail.Amount = Convert.ToDouble( timeCardDetail.Hours * rate);

                db.TimeCardDetails.Add(timeCardDetail);
                db.SaveChanges();

            
                return RedirectToAction("Index");
            }


            ViewBag.IdJobList = new SelectList(db.JobLists, "IdJobList", "JobName", timeCardDetail.IdJobList);
            ViewBag.IdTimeCard = new SelectList(db.TimeCards, "IdTimeCard", "TimeStatus", timeCardDetail.IdTimeCard);
            return View(timeCardDetail);
        }

        // GET: TimeCardDetails/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeCardDetail timeCardDetail = db.TimeCardDetails.Find(id);
            if (timeCardDetail == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdJobList = new SelectList(db.JobLists, "IdJobList", "JobName", timeCardDetail.IdJobList);
            ViewBag.IdTimeCard = new SelectList(db.TimeCards, "IdTimeCard", "TimeStatus", timeCardDetail.IdTimeCard);
            return View(timeCardDetail);
        }

        // POST: TimeCardDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdTimeCard,IdJobList,Hours,Amount")] TimeCardDetail timeCardDetail)
        {
            if (ModelState.IsValid)
            {
                db.Entry(timeCardDetail).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdJobList = new SelectList(db.JobLists, "IdJobList", "JobName", timeCardDetail.IdJobList);
            ViewBag.IdTimeCard = new SelectList(db.TimeCards, "IdTimeCard", "TimeStatus", timeCardDetail.IdTimeCard);
            return View(timeCardDetail);
        }

        // GET: TimeCardDetails/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TimeCardDetail timeCardDetail = db.TimeCardDetails.Find(id);
            if (timeCardDetail == null)
            {
                return HttpNotFound();
            }
            return View(timeCardDetail);
        }

        // POST: TimeCardDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TimeCardDetail timeCardDetail = db.TimeCardDetails.Find(id);
            db.TimeCardDetails.Remove(timeCardDetail);
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
