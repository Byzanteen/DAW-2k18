using Microsoft.AspNet.Identity;
using NoBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NoBook.Controllers
{
    public class MessageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Create(int id)
        {
            ViewBag.id = id;
            return View();
        }

        // Create message
        [HttpPost]
        public ActionResult Create(Message message)
        {
            var MyId = User.Identity.GetUserId();
            var query = from pf in db.Profile
                        where pf.UserId == MyId
                        select pf;

            int myProfileId = query.FirstOrDefault<Profile>().ProfileId;

            if(message.Text == null)
            {
                ViewBag.Error = "Enter text";
                ViewBag.id = message.Receiver;
                return View();
            }

            message.Sender = myProfileId;
            message.Sent = DateTime.Now;
            int aux = message.Receiver;
            db.Messages.Add(message);
            db.SaveChanges();
            return RedirectToAction("Index","Message",new { id = aux });
        }

        //Get messages
        public ActionResult Index(int id)
        {
            var MyId = User.Identity.GetUserId();
            var query = from pf in db.Profile
                        where pf.UserId == MyId
                        select pf;

            int myProfileId = query.FirstOrDefault<Profile>().ProfileId;

            var messageQuery = from ms in db.Messages
                               where (ms.Sender.Equals(myProfileId) &&
                               ms.Receiver.Equals(id)) ||
                               (ms.Sender.Equals(id) && ms.Receiver.Equals(myProfileId))
                               orderby ms.Sent ascending
                               select ms;

            ViewBag.id = id;
            ViewBag.messages = messageQuery;
            return View();
        }


    }
}