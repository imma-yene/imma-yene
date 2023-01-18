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
    public class DEVELOPERsController : Controller
    {
        private firstEntities db = new firstEntities();

        // GET: DEVELOPERs
        public ActionResult Index()
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            var dEVELOPERs = db.DEVELOPERs.Include(d => d.ADVISOR).Include(d=>d.Project) ;
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
                return View(dEVELOPERs.ToList());
        }
        public ActionResult Dashboard()
        {
            ViewBag.profile = null;
            using (firstEntities db = new firstEntities())
            {
                String Profile;
                int prof = Convert.ToInt32(Session["id"]);
                var mod = db.Users.Where(model => model.id == prof);
                TempData["note1"] = null;
                int count = 0;
                
                var obj = db.Projects.Where(a => a.deadline.Equals(DateTime.Today)).ToList();

                foreach (var i in obj)
                {
                    count++;
                }

                if (count > 0)
                {
                    TempData["note1"] = (count.ToString());
                }

                foreach (var i in mod)
                {
                    Profile = i.profile;
                    ViewBag.profile = Profile;


                }


            }



            if (Session["username"] == null || !Session["role"].ToString().Contains("developer"))
            {
                return RedirectToAction("../users/login");
            }
            else
                return View();
        }

        // GET: DEVELOPERs/Details/5
        public ActionResult Details(int? id)
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEVELOPER dEVELOPER = db.DEVELOPERs.Find(id);
            var mod = db.Users.Where(model => model.id == prof);
            if (dEVELOPER == null)
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
                return View(dEVELOPER);
        }

        // GET: DEVELOPERs/Create
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
            ViewBag.AID = new SelectList(db.ADVISORs, "AID", "AFNAME");
            ViewBag.Pid = new SelectList(db.Projects, "pid", "Title");
            if (Session["id"] == null)
            {
                return RedirectToAction("../Users/login");
            }
            else
                return View();
        }

        // POST: DEVELOPERs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AID,FNAME,LNAME,PHONE,EMAIL,pid")] DEVELOPER dEVELOPER)
        {
            try {
                using (firstEntities db = new firstEntities())
                {
                    var obj = db.DEVELOPERs.Where(model => model.EMAIL.Equals(dEVELOPER.EMAIL) || model.PHONE.Equals(dEVELOPER.PHONE)).FirstOrDefault();
                    TempData["create1"] = null;
                    if (ModelState.IsValid)
                    {
                        if (obj == null)
                        {
                            db.DEVELOPERs.Add(dEVELOPER);
                            db.SaveChanges();
                            TempData["create1"] = "successfully regestered";
                            return RedirectToAction("create");
                        }
                        else
                        {
                            TempData["create1"] = "this developer already assigned";
                            ViewBag.create1 = TempData["create1"];
                        }

                    }

                 
                }
            }
            catch(Exception ex)
            {
                ViewBag.ex = ex;

            }
            ViewBag.AID = new SelectList(db.ADVISORs, "AID", "AFNAME", dEVELOPER.AID);
            ViewBag.Pid = new SelectList(db.Projects, "pid", "Title", dEVELOPER.pid);
            return View(dEVELOPER);
        }

        // GET: DEVELOPERs/Edit/5
        public ActionResult Edit(int? id)
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEVELOPER dEVELOPER = db.DEVELOPERs.Find(id);
            var mod = db.Users.Where(model => model.id == prof);
            if (dEVELOPER == null)
            {
                return HttpNotFound();
            }
            foreach (var i in mod)
            {
                Profile = i.profile;
                ViewBag.profile = Profile;


            }
            ViewBag.AID = new SelectList(db.ADVISORs, "AID", "AFNAME", dEVELOPER.AID);
            if (Session["id"] == null)
            {
                return RedirectToAction("../Users/login");
            }
            else
                return View(dEVELOPER);
        }

        // POST: DEVELOPERs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AID,FNAME,LNAME,PHONE,EMAIL,PROJECT")] DEVELOPER dEVELOPER)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dEVELOPER).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AID = new SelectList(db.ADVISORs, "AID", "AFNAME", dEVELOPER.AID);
            return View(dEVELOPER);
        }

        // GET: DEVELOPERs/Delete/5
        public ActionResult Delete(int? id)
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DEVELOPER dEVELOPER = db.DEVELOPERs.Find(id);
            var mod = db.Users.Where(model => model.id == prof);
            if (dEVELOPER == null)
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
                return View(dEVELOPER);
        }

        // POST: DEVELOPERs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DEVELOPER dEVELOPER = db.DEVELOPERs.Find(id);
            db.DEVELOPERs.Remove(dEVELOPER);
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
