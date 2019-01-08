using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace NoBook.Models
{
    public class FriendRequestModel
    {
            [Key]
            public int RequestId { get; set; }
            //The one that sends the request
            public virtual Profile Requester { get; set; }
            //The one that receives the request
            public virtual Profile Requestee { get; set; }
            public bool Accepted { get; set; }
    }

}