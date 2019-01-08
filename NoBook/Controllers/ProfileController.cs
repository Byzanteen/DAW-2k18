using Microsoft.AspNet.Identity;
using NoBook.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            var userid = User.Identity.GetUserId();
            var query = from pf in db.Profile
                        where pf.UserId == userid
                        select pf;

            Profile profile = query.FirstOrDefault<Profile>();

            if (profile == null || profile.Equals(default(Profile)))
                return View();
            else
                return RedirectToAction("Show", "Profile", profile.ProfileId);
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
        public ActionResult Show(int? id)
        {
            ViewBag.SameUser = 1;
            ViewBag.Friend = 0;
            ViewBag.RequestedByMe = 0;
            ViewBag.RequestedByThem = 0;

            if (id.HasValue)
            {
                Profile profile = db.Profile.Find(id);

                var userid = User.Identity.GetUserId();
                var query = from pf in db.Profile
                            where pf.UserId == userid
                            select pf;

                Profile myProfile = query.FirstOrDefault<Profile>();

                if (myProfile != null && !myProfile.Equals(default(Profile)))
                {

                    if (User.Identity.GetUserId() != profile.UserId)
                    {
                        ViewBag.SameUser = 0;
                    }

                    if (profile.Friends.Contains(myProfile))
                    {
                        ViewBag.Friend = 1;
                    }


                    foreach(FriendRequestModel freq in profile.FriendRequests)
                    {
                        if(freq.Requestee.ProfileId == myProfile.ProfileId)
                        {
                            ViewBag.RequestedByThem = 1;
                            break;
                        }
                        if(freq.Requester.ProfileId == myProfile.ProfileId)
                        {
                            ViewBag.RequestedByMe = 1;
                            break;
                        }
                    }

                }
                return View(profile);
            }
            else
            { 
            var userid = User.Identity.GetUserId();
            var query = from pf in db.Profile
                        where pf.UserId == userid
                        select pf;

            Profile profile = query.FirstOrDefault<Profile>();
            if (profile == null || profile.Equals(default(Profile)))
                return RedirectToAction("Create", "Profile");
            else
                return View(profile);
            }
        }

        //GET: Edit profile
        public ActionResult Edit(int? id)
        {

            if (id.HasValue)
            {
                Profile profile = db.Profile.Find(id);
                return View(profile);
            }
            else
            {
                var userid = User.Identity.GetUserId();
                var query = from pf in db.Profile
                            where pf.UserId == userid
                            select pf;

                Profile profile = query.FirstOrDefault<Profile>();

                if (profile == null || profile.Equals(default(Profile)))
                    return RedirectToAction("Create", "Profile", profile.ProfileId);
                else
                    return View(profile);
            }
        }

        //POST: Edit profile
        [HttpPut]
        public ActionResult Edit(int id, Profile profile)
        {
            try
            {
                Profile aux = db.Profile.Find(id);
                if (TryUpdateModel(aux))
                {
                    aux.FirstName = profile.FirstName;
                    aux.LastName = profile.LastName;
                    db.SaveChanges();
                }
                return RedirectToAction("Show");
            } catch (Exception e) {
                return View();
            }
        }

        //GET: Index
        public ActionResult Index(string searchTerm)
        {
            var profiles = from pf in db.Profile
                        where pf.FirstName.ToLower().Contains(searchTerm.ToLower()) ||
                        pf.LastName.ToLower().Contains(searchTerm.ToLower())
                        select pf;

            ViewBag.Profiles = profiles;
            ViewBag.count = profiles.Count<Profile>();
            return View();
        }


    }
}