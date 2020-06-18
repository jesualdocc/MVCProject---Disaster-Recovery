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
    public class TimeCardDetailsController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        // GET: TimeCardDetails
        public ActionResult Index()
        {
            var timeCardDetails = db.TimeCardDetails.Include(t => t.JobList).Include(t => t.TimeCard);
            return View(timeCardDetails.ToList());
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
            ViewBag.IdTimeCard = new SelectList(db.TimeCards, "IdTimeCard", "TimeStatus");
            return View();
        }

        // POST: TimeCardDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdTimeCard,IdJobList,Hours,Amount")] TimeCardDetail timeCardDetail)
        {
            if (ModelState.IsValid)
            {
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
