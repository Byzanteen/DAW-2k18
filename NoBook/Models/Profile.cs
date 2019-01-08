using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoBook.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserId { get; set; }
        public bool IsPublic { get; set; }
        public List<Profile> Friends = new List<Profile>();
        public List<FriendRequestModel> FriendRequests = new List<FriendRequestModel>();

        public virtual ApplicationUser User { get; set; }
    }
}