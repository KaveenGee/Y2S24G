using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ITP.Models
{
    public class Email
    {

        public IFormFile Imagename { get; set; }

        public string Email_title { get; set; }

        public string Email_body { get; set; }

    }
}
