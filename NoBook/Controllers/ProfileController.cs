using Microsoft.AspNet.Identity;
using NoBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoBook.Controllers
{
    public class ProfileController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //GET: Create profile
        public ActionResult Create ()
        {
            return View();
        }

        //POST: Create profile
        [HttpPost]
        public ActionResult Create (Profile profile)
        {
            profile.UserId = User.Identity.GetUserId();
            try
            {
                db.Profile.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Show","Profile",profile.ProfileId);
            } catch (Exception e) {
                return View();
            }
        }

        // GET: Profile
        public ActionResult Show(int id)
        {
            Profile profile = db.Profile.Find(id);
            return View(profile);
        }



    }
}