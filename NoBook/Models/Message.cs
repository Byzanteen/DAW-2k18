using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoBook.Models
{
    public class Message
    {
        [Key]
        public int MessageId { get; set; }
        public int Sender { get; set; }
        public int Receiver { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime Sent { get; set; }
    }
}