using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class ADVISORsController : Controller
    {
        private firstEntities db = new firstEntities();

        // GET: ADVISORs
        public ActionResult Index()
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            var mod = db.Users.Where(model => model.id == prof);
            foreach (var i in mod)
            {
                Profile = i.profile;
                ViewBag.profile = Profile;


            }
            if (Session["id"] == null)
            {
                return RedirectToAction("../Users/login");
            }
            else
                return View(db.ADVISORs.ToList());
        }
     

        // GET: ADVISORs/Details/5
        public ActionResult Details(int? id)
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ADVISOR aDVISOR = db.ADVISORs.Find(id);
            var mod = db.Users.Where(model => model.id == prof);
            if (aDVISOR == null)
            {
                return HttpNotFound();
            }
            foreach (var i in mod)
            {
                Profile = i.profile;
                ViewBag.profile = Profile;


            }
            if (Session["id"] == null)
            {
                return RedirectToAction("../Users/login");
            }
            else
                return View(aDVISOR);
        }

        // GET: ADVISORs/Create
        public ActionResult Create()
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            var mod = db.Users.Where(model => model.id == prof);
            foreach (var i in mod)
            {
                Profile = i.profile;
                ViewBag.profile = Profile;


            }
            if (Session["id"] == null)
            {
                return RedirectToAction("../Users/login");
            }
            else
                return View();
        }

        // POST: ADVISORs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AID,AFNAME,ALNAME,PHONE,EMAIL")] ADVISOR aDVISOR)
        {
            using (firstEntities db = new firstEntities())
            {
                TempData["create"] = null;
                
                var obj = db.ADVISORs.Where(a => a.PHONE.Equals(aDVISOR.EMAIL) || a.PHONE.Equals(aDVISOR.PHONE)).FirstOrDefault();
               
                if (ModelState.IsValid)
                {
                    if (obj == null)
                    {
                        db.ADVISORs.Add(aDVISOR);
                        db.SaveChanges();
                        TempData["create"] = "successfuly registered";
                        return RedirectToAction("create");
                        
                    }
                    else
                    {
                        TempData["create"] = "this advisor registered before or check email and phone ";
                         ViewBag.create=TempData["create"];
                    }

                }
               
                return View(aDVISOR);
            }
        }
            // GET: ADVISORs/Edit/5
            public ActionResult Edit(int? id)
            {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                ADVISOR aDVISOR = db.ADVISORs.Find(id);
            var mod = db.Users.Where(model => model.id == prof);
            if (aDVISOR == null)
                {
                    return HttpNotFound();
                }
            foreach (var i in mod)
            {
                Profile = i.profile;
                ViewBag.profile = Profile;


            }
            if (Session["id"] == null)
            {
                return RedirectToAction("../Users/login");
            }
            else
                return View(aDVISOR);
            }
        

        // POST: ADVISORs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AID,AFNAME,ALNAME,PHONE,EMAIL")] ADVISOR aDVISOR)
        {
            if (ModelState.IsValid)
            {
                db.Entry(aDVISOR).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(aDVISOR);
        }
        [HttpGet]
        public ActionResult delete1()
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            var mod = db.Users.Where(model => model.id == prof);
            foreach (var i in mod)
            {
                Profile = i.profile;
                ViewBag.profile = Profile;


            }
            if (Session["id"] == null)
            {
                return RedirectToAction("../Users/login");
            }
            else
                return View();
        }
        [HttpPost]
        public ActionResult delete1([Bind(Include ="AID")] ADVISOR aDVISOR)
        {
            int id = aDVISOR.AID;
            ADVISOR aDVISO = db.ADVISORs.Find(id);
            if (aDVISO == null)
            {
                return HttpNotFound();
            }
            else
            return RedirectToAction("Delete","ADVISORs",new { id});
        }
        // GET: ADVISORs/Delete/5
        public ActionResult Delete(int? id)
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);

            if (id == null)
            {
                return RedirectToAction("delete1");
             
             
            }
            ADVISOR aDVISOR = db.ADVISORs.Find(id);
            var mod = db.Users.Where(model => model.id == prof);
            if (aDVISOR == null)
            {
                return HttpNotFound();
            }
            foreach (var i in mod)
            {
                Profile = i.profile;
                ViewBag.profile = Profile;


            }
            if (Session["id"] == null)
            {
                return RedirectToAction("../Users/login");
            }
            else
                return View(aDVISOR);
        }

        // POST: ADVISORs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ADVISOR aDVISOR = db.ADVISORs.Find(id);
            db.ADVISORs.Remove(aDVISOR);
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
