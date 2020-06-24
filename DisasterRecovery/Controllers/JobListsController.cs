using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DisasterRecovery.Infrastructure;
using DisasterRecovery.Models;


namespace DisasterRecovery.Controllers
{
    [CustomAuthorize("Admin")]
    public class JobListsController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        // GET: JobLists
        public ActionResult Index()
        {
            return View(db.JobLists.ToList());
        }

        // GET: JobLists/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobList jobList = db.JobLists.Find(id);
            if (jobList == null)
            {
                return HttpNotFound();
            }
            return View(jobList);
        }

        // GET: JobLists/Create
        public ActionResult Create()
        {
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName");
            return View();
        }

        // POST: JobLists/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdJobList,IdSubContractor,JobName,JobDescription,Rate,MaxHour,LaborStatus")] JobList jobList)
        {
            if (ModelState.IsValid)
            {
                jobList.LaborStatus = 0;
                db.JobLists.Add(jobList);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", jobList.IdSubContractor);
            return View(jobList);
        }

        // GET: JobLists/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobList jobList = db.JobLists.Find(id);
            if (jobList == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", jobList.IdSubContractor);
            return View(jobList);
        }

        // POST: JobLists/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdJobList,IdSubContractor,JobName,JobDescription,Rate,MaxHour,LaborStatus")] JobList jobList)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jobList).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", jobList.IdSubContractor);
            return View(jobList);
        }

        // GET: JobLists/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            JobList jobList = db.JobLists.Find(id);
            if (jobList == null)
            {
                return HttpNotFound();
            }
            return View(jobList);
        }

        // POST: JobLists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            JobList jobList = db.JobLists.Find(id);
            db.JobLists.Remove(jobList);
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
