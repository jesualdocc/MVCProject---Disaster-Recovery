using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DisasterRecovery.Models;
using PagedList;

namespace DisasterRecovery.Controllers
{
    public class SiteLocationsController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        // GET: SiteLocations
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var SiteLocations = from s in db.SiteLocations
                                select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                SiteLocations = SiteLocations.Where(s => s.LocationName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    SiteLocations = SiteLocations.OrderByDescending(s => s.LocationName);
                    break;
                case "Date":
                    SiteLocations = SiteLocations.OrderBy(s => s.SiteCode);
                    break;
                case "date_desc":
                    SiteLocations = SiteLocations.OrderByDescending(s => s.SiteCode);
                    break;
                default:
                    SiteLocations = SiteLocations.OrderBy(s => s.LocationName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(SiteLocations.ToPagedList(pageNumber, pageSize));
            //return View(db.SiteLocations.ToList());
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
