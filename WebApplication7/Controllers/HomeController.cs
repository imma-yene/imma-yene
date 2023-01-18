using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication7.Models;

namespace WebApplication7.Controllers
{
    public class HomeController : Controller
    {

    
        public ActionResult Index()
        {
           
            using (firstEntities db = new firstEntities())
            {
                String Profile;
                int prof = Convert.ToInt32(Session["id"]);
                TempData["note"] = null;
                int count = 0;
                var mod = db.Users.Where(model => model.id == prof);
                var obj = db.Users.Where(a => a.status.Equals(false)).ToList();
         
                foreach (var i in obj)
                {
                    count++;
                }

                if (count > 0)
                {
                    TempData["note"] = ( count.ToString());
               
                }
                foreach (var i in mod)
                {
                    Profile = i.profile;
                    ViewBag.profile = Profile;


                }


            }



            if (Session["username"] == null || !Session["role"].ToString().Contains("manager"))
                {
                    return RedirectToAction("../users/login");
                }
                else
                    return View();
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}