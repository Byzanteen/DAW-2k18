using NoBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace NoBook.Controllers
{
    public class FriendRequestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Create Friend Request
        public ActionResult Create(int id)
        {
            var MyId = User.Identity.GetUserId();
            var query = from pf in db.Profile
                        where pf.UserId == MyId
                        select pf;

            Profile profile = query.FirstOrDefault<Profile>();

            Profile friendProfile = db.Profile.Find(id);

            var query1 = from fr in db.FriendRequests
                         where fr.Requester.ProfileId.Equals(profile.ProfileId) && 
                         fr.Requestee.ProfileId.Equals(friendProfile.ProfileId)
                         select fr;

            FriendRequestModel friendRequest = query1.FirstOrDefault<FriendRequestModel>();

            if (friendRequest == null || friendRequest.Equals(default(Profile)))
            {
                var NewFR = new FriendRequestModel();
                NewFR.Requester = profile;
                NewFR.Requestee = friendProfile;
                NewFR.Accepted = false;
                db.FriendRequests.Add(NewFR);
                db.SaveChanges();
                if (TryUpdateModel(profile))
                {
                    profile.FriendRequests.Insert(0, NewFR);
                    db.SaveChanges();
                }
                if (TryUpdateModel(friendProfile))
                {
                    friendProfile.FriendRequests.Insert(0, NewFR);
                    db.SaveChanges();
                }
                db.SaveChanges();
            }   
            return RedirectToAction("Show","Profile",id);
        }

        //Accept Friend Request
        public ActionResult Accept(int id)
        {
            FriendRequestModel request = db.FriendRequests.Find(id);

            Profile requester = db.Profile.Find(request.Requester.ProfileId);
            Profile requestee = db.Profile.Find(request.Requestee.ProfileId);
            
            if(requester != null && requestee != null)
            {
                if (TryUpdateModel(requester) && TryUpdateModel(requestee))
                {
                    requester.FriendRequests.Remove(request);
                    requester.Friends.Add(requestee);
                    requestee.FriendRequests.Remove(request);
                    requestee.Friends.Add(requester);
                    db.SaveChanges();

                }
            }
            return this.Delete(request);

        }

        //Delete Friend Request
        public ActionResult Delete(FriendRequestModel fr)
        {
            db.FriendRequests.Remove(fr);
            db.SaveChanges();
            return RedirectToAction("Index","FriendRequest");
        }

        // GET: FriendRequest
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var query = from pf in db.Profile
                        where pf.UserId == userid
                        select pf;

            Profile profile = query.FirstOrDefault<Profile>();

            if (profile == null || profile.Equals(default(Profile)))
            {
                ViewBag.Message = "Profile Not Found";
                return View();
            } else {
                var query1 = from fr1 in db.FriendRequests
                        where fr1.Requester.ProfileId.Equals(profile.ProfileId)
                        select fr1;

                var query2 = from fr in db.FriendRequests
                             where fr.Requestee.ProfileId.Equals(profile.ProfileId)
                             select fr;

                ViewBag.ByMe = query1.AsEnumerable<FriendRequestModel>();
                ViewBag.ToMe = query2.AsEnumerable<FriendRequestModel>();

                if (query1.Count<FriendRequestModel>() == 0)
                    ViewBag.ByMeMessage = "No friend requests by you.";

                if (query2.Count() == 0)
                    ViewBag.ToMeMessage = "You have no friends requests.";
                return View();
            }
        }
    }
}