﻿using System;
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
    public class SiteLocationsController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        // GET: SiteLocations
        public ActionResult Index()
        {
            return View(db.SiteLocations.ToList());
        }

        // GET: SiteLocations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLocation siteLocation = db.SiteLocations.Find(id);
            if (siteLocation == null)
            {
                return HttpNotFound();
            }
            return View(siteLocation);
        }

        // GET: SiteLocations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SiteLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SiteID,LocationName,SiteCode,ZipCode")] SiteLocation siteLocation)
        {
            if (ModelState.IsValid)
            {
                db.SiteLocations.Add(siteLocation);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(siteLocation);
        }

        // GET: SiteLocations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLocation siteLocation = db.SiteLocations.Find(id);
            if (siteLocation == null)
            {
                return HttpNotFound();
            }
            return View(siteLocation);
        }

        // POST: SiteLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SiteID,LocationName,SiteCode,ZipCode")] SiteLocation siteLocation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(siteLocation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(siteLocation);
        }

        // GET: SiteLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SiteLocation siteLocation = db.SiteLocations.Find(id);
            if (siteLocation == null)
            {
                return HttpNotFound();
            }
            return View(siteLocation);
        }

        // POST: SiteLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteLocation siteLocation = db.SiteLocations.Find(id);
            db.SiteLocations.Remove(siteLocation);
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
