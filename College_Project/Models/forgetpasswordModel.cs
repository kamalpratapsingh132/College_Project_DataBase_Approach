using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace College_Project.Models
{
    public class forgetpasswordModel
    {
        [Required,EmailAddress,Display(Name="Register Email address")]
        public string Email { get; set; }
        public bool Emailsent { get; set; }
    }
}