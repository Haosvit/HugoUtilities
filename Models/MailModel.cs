using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WordScrambler.Models
{
    public class MailModel
    {
        public string FromAddress { get; set; }

        public string Password { get; set; }

        public string Subject { get; set; }

        [AllowHtml]
        public string Content { get; set; }

    }
}