using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace College_Project.Models
{
    public class resetpasswordmodel
    {
        [Display(Name = "EMAIL")]
        [Required(ErrorMessage = "EMAIL is Required.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }


        [Display(Name = "New Password")]
        [Required(ErrorMessage = "New Password is Required.")]
        public string Newpassword { get; set; }

        [Display(Name = "Confirm New Password")]
        [Required(ErrorMessage = "Confirm New Password is Required.")]
        [Compare(otherProperty: "NewPassword", ErrorMessage = "New Password Does't Match")]
        [DataType(DataType.Password)]
        public string Confirmnewpassword { get; set; }


        [Display(Name = "Enter OTP")]
        public int Otp { get; set; }
    }
}