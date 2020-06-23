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
    public class SubContractorsController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        // GET: SubContractors
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

            var SubContractor = from s in db.SubContractors
                           select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                SubContractor = SubContractor.Where(s => s.SubContractorName.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    SubContractor = SubContractor.OrderByDescending(s => s.SubContractorName);
                    break;
                case "Date":
                    SubContractor = SubContractor.OrderBy(s => s.ContractorAddress);
                    break;
                case "date_desc":
                    SubContractor = SubContractor.OrderByDescending(s => s.ContractorAddress);
                    break;
                default:
                    SubContractor = SubContractor.OrderBy(s => s.SubContractorName);
                    break;
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(SubContractor.ToPagedList(pageNumber, pageSize));

            //return View(db.SubContractors.ToList());
        }

        // GET: SubContractors/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubContractor subContractor = db.SubContractors.Find(id);
            if (subContractor == null)
            {
                return HttpNotFound();
            }
            return View(subContractor);
        }

        // GET: SubContractors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SubContractors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdSubContractor,SubContractorName,ContractorAddress,Email,Phone,SubContractorStatus")] SubContractor subContractor)
        {
            if (ModelState.IsValid)
            {
                subContractor.SubContractorStatus = 0;
                db.SubContractors.Add(subContractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subContractor);
        }

        // GET: SubContractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubContractor subContractor = db.SubContractors.Find(id);
            if (subContractor == null)
            {
                return HttpNotFound();
            }
            return View(subContractor);
        }

        // POST: SubContractors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdSubContractor,SubContractorName,ContractorAddress,Email,Phone,SubContractorStatus")] SubContractor subContractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subContractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subContractor);
        }

        // GET: SubContractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubContractor subContractor = db.SubContractors.Find(id);
            if (subContractor == null)
            {
                return HttpNotFound();
            }
            return View(subContractor);
        }

        // POST: SubContractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubContractor subContractor = db.SubContractors.Find(id);
            db.SubContractors.Remove(subContractor);
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
