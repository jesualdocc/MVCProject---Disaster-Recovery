using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using DisasterRecovery.Infrastructure;
using DisasterRecovery.Models;


namespace DisasterRecovery.Controllers
{
    public class TimeCardsController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();
        private static int userID;

        // GET: TimeCards
        [CustomAuthorize("Admin", "Contractor")]
        public ActionResult Index()
        {
         
        var timeCards = db.TimeCards.Include(t => t.SiteLocation).Include(t => t.SubContractor).Include(t => t.User);

            userID = Convert.ToInt32(Session["LogedUserID"]);
            timeCards = timeCards.Where(u => u.IdUser == userID);
            timeCards = timeCards.OrderByDescending(d => d.RegDate);


            return View(timeCards.ToList());
        }
        [CustomAuthorize("Admin")]
        public ActionResult IndexAdm()
        {
            var timeCards = db.TimeCards.Include(t => t.SiteLocation).Include(t => t.SubContractor).Include(t => t.User);
            timeCards = timeCards.OrderByDescending(d => d.RegDate);
            timeCards = timeCards.Where(t => t.TimeStatus != "Incomplete");

            return View(timeCards.ToList());
        }
        [CustomAuthorize("Admin")]
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
            
            timeCard.TimeStatus = "Approved";
            db.Entry(timeCard).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexAdm");
        }
        [CustomAuthorize("Admin")]
        public ActionResult Reject(int? id)
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

            timeCard.TimeStatus = "Rejected";
            db.Entry(timeCard).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("IndexAdm");
        }

        // GET: TimeCards/ReviewSubmission/5
        [CustomAuthorize("Admin", "Contractor")]
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
        [CustomAuthorize("Admin", "Contractor")]
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
        [CustomAuthorize("Contractor")]
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
        [CustomAuthorize("Contractor")]
        public ActionResult Create([Bind(Include = "IdTimeCard,SiteID")] TimeCard timeCard)
        {
           
            if (ModelState.IsValid)
            {
                timeCard.RegDate = DateTime.Now;
                timeCard.TimeStatus = "Open";

                userID = Convert.ToInt32(Session["LogedUserID"]);
                timeCard.IdUser = userID;

                var contractorID = db.Users.Find(userID).IdSubContractor;

                if(contractorID == null)
                {
                    return RedirectToAction("Index", "TimeCardDetails");
                }

                timeCard.IdSubContractor = Convert.ToInt32(contractorID);

                db.TimeCards.Add(timeCard);
                db.SaveChanges();

                Session["TimeCardID"] = timeCard.IdTimeCard;
               
                return RedirectToAction("Index", "TimeCardDetails");
            }

            ViewBag.SiteID = new SelectList(db.SiteLocations, "SiteID", "LocationName", timeCard.SiteID);
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", timeCard.IdSubContractor);
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "UserName", timeCard.IdUser);
            return View(timeCard);
        }

        // GET: TimeCards/Edit/5
        [CustomAuthorize("Contractor")]
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
        [CustomAuthorize("Contractor")]
        public ActionResult Edit([Bind(Include = "IdTimeCard,SiteID,IdSubContractor,TotalHours,TotalAmount,TimeStatus,RegDate")] TimeCard timeCard)
        {
            if (ModelState.IsValid)
            {
                timeCard.IdUser = userID;
                timeCard.RegDate = DateTime.Now;
               
                var contractorID = db.Users.Find(userID).IdSubContractor;

                if (contractorID == null)
                {
                    return RedirectToAction("Index", "TimeCardDetails");
                }

                timeCard.IdSubContractor = Convert.ToInt32(contractorID);

                db.Entry(timeCard).State = EntityState.Modified;
                db.SaveChanges();

                Session["TimeCardID"] = timeCard.IdTimeCard;

                return RedirectToAction("IndexEdit", "TimeCardDetails");
            }
            ViewBag.SiteID = new SelectList(db.SiteLocations, "SiteID", "LocationName", timeCard.SiteID);
            ViewBag.IdSubContractor = new SelectList(db.SubContractors, "IdSubContractor", "SubContractorName", timeCard.IdSubContractor);
            ViewBag.IdUser = new SelectList(db.Users, "IdUser", "UserName", timeCard.IdUser);
            return View(timeCard);
        }

        // GET: TimeCards/Delete/5
        [CustomAuthorize("Admin", "Contractor")]
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
           
            
           // return RedirectToAction("Delete", new RouteValueDictionary( new { controller = "TimeCards", action = "Delete", Id = id }));
       
        }

        // POST: TimeCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [CustomAuthorize("Admin", "Contractor")]
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
            if(Session["LogedUserRole"].ToString() == "Admin")
                return RedirectToAction("IndexAdm");
            else
                return RedirectToAction("Index");
        }

        [CustomAuthorize("Admin", "Contractor")]
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
