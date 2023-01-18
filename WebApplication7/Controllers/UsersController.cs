using System;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebApplication7.Models;


namespace WebApplication7.Controllers
{
    public class UsersController : Controller
    {
        private firstEntities db = new firstEntities();

        // GET: Users
        public ActionResult Index()
        {
            using (firstEntities db = new firstEntities())

            {
                String Profile;
                int prof = Convert.ToInt32(Session["id"]);
                var obj = db.Users.FirstOrDefault();
                var mod = db.Users.Where(model => model.id == prof);
                if (obj.status == true)
                {
                    Session["status"] = "Active";
                }
                else if (obj.status == false)
                    Session["status"] = "Deactive";
                else
                    Session["status"] = null;
                foreach (var i in mod)
                {
                    Profile = i.profile;
                    ViewBag.profile = Profile;


                }

            }
            if (Session["id"] == null)
            {
                return RedirectToAction("login");
            }
            else
             return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            var mod = db.Users.Where(model => model.id == prof);
            if (user == null)
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
                return RedirectToAction("login");
            }
            else
                return View(user);
        }
        [HttpGet]
        public ActionResult Login()
        {
            if (Session["username"] != null)
            {
                return RedirectToAction("../home/index");
            }
            else
                return View();
        }

        public ActionResult Logout()
        {
            Session.Remove("ID");
            Session.Remove("username");
            Session.Remove("role");
            Session.Abandon();

            Session.Contents.Abandon();
            Session.Contents.RemoveAll();
            Session["ID"] = null;
            Session["username"] = null;
            Session["role"] = null;
            Session["profile"] = null;
            TempData["login"] = null;
            return View("Login");


        }

        [HttpGet]
        public ActionResult AccApro()
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
                    TempData["note"] = (count.ToString());
                }

                foreach (var i in mod)
                {
                    Profile = i.profile;
                    ViewBag.profile = Profile;


                }
                if (Session["id"] == null)
                {
                    return RedirectToAction("login");
                }
                else


                    return View(db.Users.Where(a => a.status.Equals(false)).ToList());
            }


        }

        [HttpGet]
        public ActionResult AccDeact()
        {
            using (firstEntities db = new firstEntities())
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
                    return RedirectToAction("login");
                }
                else
                    return View(db.Users.Where(a => a.status.Equals(true)).ToList());

            }


        }

        [HttpPost]
        public ActionResult ChangePassword([Bind (Include = "id,email,password,profile,role,status,newpassword,renewpassword")]User user)
        {
            int id = Convert.ToInt32(Session["id"]);
            byte[] EncDataByte = new byte[user.password.Length];
            EncDataByte = System.Text.Encoding.UTF8.GetBytes(user.password);
            String EncryptedData = Convert.ToBase64String(EncDataByte);
            user.password = EncryptedData;
            //newpass
            byte[] EncDataByte1 = new byte[user.newpassword.Length];
            EncDataByte1 = System.Text.Encoding.UTF8.GetBytes(user.newpassword);
            String EncryptedData1 = Convert.ToBase64String(EncDataByte1);
            user.renewpassword = EncryptedData1;
            //reenternewpass
            byte[] EncDataByt = new byte[user.renewpassword.Length];
            EncDataByt = System.Text.Encoding.UTF8.GetBytes(user.renewpassword);
            String EncryptedDat = Convert.ToBase64String(EncDataByt);
            user.renewpassword = EncryptedDat;

           
           
            
            using (firstEntities db = new firstEntities())
            {
              
                try
                {
                    if (ModelState.IsValid)
                    {
                        if (user.password == user.password)
                        {
                            if (user.newpassword == user.renewpassword)
                            {
                                user.password = user.newpassword;
                                db.Entry(user).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("UserProfile");
                            }
                            else
                                ViewBag.notmatch = "password not match";
                        }
                        else
                            ViewBag.notcorrect = "old password not correct";
                       
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.ex = ex.Message.ToString();
                }
                return RedirectToAction("UserProfile");
            }
        }
         public ActionResult UserProfile()
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            var mod = db.Users.Where(model => model.id == prof);
            foreach (var i in mod)
            {
                Profile = i.profile;
                ViewBag.profile = Profile;
                ViewBag.email = i.email;
                ViewBag.role = i.role;
               


            }
            if (Session["id"] == null)
            {
                return RedirectToAction("login");
            }
            return View();
        }
        public ActionResult Dashboard()
        {
        
            using (firstEntities db = new firstEntities())
            {
                
                TempData["note2"] = null;
             
                int count = 0;
                String Profile;
              int prof=  Convert.ToInt32(Session["id"]);
                var obj = db.Projects.Where(a => a.deadline.Equals(DateTime.Today)).ToList();
                var mod = db.Users.Where(model => model.id==prof);
                foreach (var i in obj)
                {
                    count++;
                }

                if (count > 0)
                {
                    TempData["note2"] = (count.ToString());
                }
                foreach(var i in mod)
                {
                   Profile = i.profile;
                    ViewBag.profile = Profile;
                    
                    
                }
                


            }



            if (Session["username"] == null || !Session["role"].ToString().Contains("advisor"))
            {
                return RedirectToAction("login");
            }
            else
                return View();
        }


        [HttpPost]
        public ActionResult Login(User user)
        {
            using (firstEntities db = new firstEntities())
            {
                if (user.email == null && user.password == null)
                {

                    TempData["login"] = ("please enter your username and password");
                    return View("Login");

                }
                else if (user.email == null)
                {
                    TempData["login"] = ("please enter your username");
                    return View("Login");
                }
                else if (user.password == null)
                {
                    TempData["login"] = ("please enter your password");
                    return View("Login");
                }
                else
                {
                    byte[] EncDataByte = new byte[user.password.Length];
                    EncDataByte = System.Text.Encoding.UTF8.GetBytes(user.password);
                    String EncryptedData = Convert.ToBase64String(EncDataByte);
                    user.password = EncryptedData;
                    TempData["login"] = "Incorrect Password or username";

                    var obj = db.Users.Where(a => a.email.Equals(user.email) && a.password.Equals(user.password)).FirstOrDefault();






                    if (obj != null)
                    {
                        String Role = obj.role.ToString();

                        if (Role.Contains("manager"))
                        {
                            if (obj.status.Equals(false))
                            {
                                TempData["login"] = "you are in pending state wait for manager to approve your account";
                                ViewBag.login = TempData["login"];
                                return View();
                            }
                            else
                            {



                                Session["id"] = obj.id.ToString();
                                Session["username"] = obj.email.ToString();
                                Session["role"] = Role.ToString();


                                return RedirectToAction("../home/index");
                            }
                        }
                        else
                              if (Role.Contains("advisor"))
                        {
                            if (obj.status.Equals(false))
                            {
                                TempData["login"] = "you are in pending state wait for manager to approve your account";
                                ViewBag.login = TempData["login"];
                                return View();
                            }
                            else
                            {
                                Session["id"] = obj.id.ToString();
                                Session["username"] = obj.email.ToString();
                                Session["role"] = Role.ToString();
                                Session["profile"] = obj.profile.ToString();
                                return RedirectToAction("dashboard");
                            }

                        }
                        else if (Role.Contains("developer"))
                        {
                            if (obj.status.Equals(false))
                            {
                                TempData["login"] = "you are in pending state wait for manager to approve your account";
                                ViewBag.login = TempData["login"];
                                return View();
                            }
                            else
                            {
                                Session["id"] = obj.id.ToString();
                                Session["username"] = obj.email.ToString();
                                Session["role"] = Role.ToString();



                            }
                            return RedirectToAction("../DEVELOPERs/dashboard");
                        }
                        else
                            TempData["login"].ToString();

                        return View();
                    }
                }
            }

            return View();
        }


        // GET: Users/Create
        public ActionResult Create()
        {

            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(User user)
        {

            using (firstEntities db = new firstEntities())
            {
                String fileName = Path.GetFileNameWithoutExtension(user.ImageFile.FileName);
                String extension = Path.GetExtension(user.ImageFile.FileName);

                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                var filename2 = fileName;
                String path = "C:/Users/Mehbuba.Abera/source/repos/WebApplication7/WebApplication7";
                var files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);


                user.profile = "~/Profile/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/Profile/"), fileName);
                foreach (var i in files)
                {
                    String fName = Path.GetFileName(i);
                    if (fName != filename2)
                    {
                        user.ImageFile.SaveAs(fileName);
                    }
                }

                byte[] EncDataByte = new byte[user.password.Length];
                EncDataByte = System.Text.Encoding.UTF8.GetBytes(user.password);
                String EncryptedData = Convert.ToBase64String(EncDataByte);
                user.password = EncryptedData;



                var obj = db.Users.Where(model => model.email.Equals(user.email)).FirstOrDefault();

                TempData["register"] = null;
                user.status = false;


                if (obj == null)
                {

                    db.Users.Add(user);
                    db.SaveChanges();
                    return RedirectToAction("../users/login");
                }
                else
                {
                    TempData["register"] = "this username has taken by another user";
                    ViewBag.register = TempData["register"];

                    return View(user);
                }




            }
        }


        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var mod = db.Users.Where(model => model.id == prof);
            User user = db.Users.Find(id);
            if (user == null)
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
                return RedirectToAction("login");
            }
            else
                return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,email,password,profile,role,status")] User user)
        {
            using (firstEntities db = new firstEntities())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        db.Entry(user).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index");
                    }

                }
                catch (Exception ex)
                {
                    ViewBag.ex = ex.Message.ToString();
                }
                return View(user);
            }
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            String Profile;
            int prof = Convert.ToInt32(Session["id"]);
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            var mod = db.Users.Where(model => model.id == prof);
            if (user == null)
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
                return RedirectToAction("login");
            }
            else
                return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            User user = db.Users.Find(id);
            db.Users.Remove(user);
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
