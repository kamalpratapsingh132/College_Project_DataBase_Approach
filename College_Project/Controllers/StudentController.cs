using College_Project.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace College_Project.Controllers
{
    public class StudentController : Controller
    {
        FriendsEntities dbj = new FriendsEntities();
        ObjectParameter objectparam = new ObjectParameter("accid", typeof(decimal));

        // GET: Student

        public ActionResult Create(int? id)
        {
            Student tbl = new Student();

            if (id != null)
            {      
                var res = dbj.Students.ToList();
                ViewBag.clientlist = res;
                //var data = dbj.Students.SingleOrDefault(e => e.ID == ID);
                var data = dbj.Students.Where(x => x.ID == id).FirstOrDefault();
                  ViewBag.StateList = dbj.State_tbl.Distinct().OrderBy(i => i.StateId).Select(i => new SelectListItem() { Text = i.State, Value = i.StateId.ToString() }).ToList();


                tbl.ID = data.ID;
                tbl.First_Name = data.First_Name;
                tbl.Last_Name = data.Last_Name;
                tbl.Mob_No = data.Mob_No;
                tbl.Email = data.Email;             
                tbl.State = data.State;
                tbl.city = data.city;
                tbl.Address = data.Address;

                return View(tbl);               
            }
            else
            {
                ViewBag.StateList = dbj.State_tbl.Distinct().OrderBy(i => i.StateId).Select(i => new SelectListItem() { Text = i.State, Value = i.StateId.ToString() }).ToList();
                var res = dbj.Students.ToList();
                ViewBag.clientlist = res;
                return View(tbl);
            }

        }

        [HttpPost]
        public ActionResult Create(Student Model)
        {
            ViewBag.StateList = dbj.State_tbl.Distinct().OrderBy(i => i.StateId).Select(i => new SelectListItem() { Text = i.State, Value = i.StateId.ToString() }).ToList();
            var res = dbj.Students.ToList();

            ViewBag.clientlist = res;

            Random rand = new Random();
            string password = Convert.ToString(rand.Next(100000, 999999));

            if (Model.ID == 0)
            {               
                var X = dbj.spstudentinsert( Model.First_Name, Model.Last_Name, Model.Mob_No, Model.Email, password, Model.State, Model.city, Model.Address, objectparam);
                var chk = objectparam.Value;

                if (Convert.ToInt32(chk) == 0)
                {
                    ViewBag.Message = "STUDENT DATA ALREADY EXIST.";
                }
                else
                {
                    ViewBag.Message = "STUDENT DATA SAVE SUCCESSFULLY.";
                }
                ModelState.Clear();

            }
            else
            {  //Update Data

                var x = dbj.Sp_Update_student(Model.ID, Model.First_Name, Model.Last_Name, Model.Mob_No, Model.Email, password, Model.State, Model.city, Model.Address, objectparam);
                var chk = objectparam.Value;
                // var chk = Convert.ToInt32(accid.Value);
                if (Convert.ToInt32(chk) == 1)
                {
                    TempData["msg"] = " UPDATED SUCCESSFULLY !!!";
                }
                else
                {
                    TempData["msg"] = " NOT UPDATED SUCCESSFULLY !!!";
                }
                ModelState.Clear();
            }
            
           return RedirectToAction("Create");
        }

        public ActionResult City_Bind(int State)
        {
            List<SelectListItem> City = new List<SelectListItem>();
            City = dbj.City_tbl.Where(i => i.StateID == State).OrderBy(i => i.StateID).Select(i => new SelectListItem() { Text = i.City, Value = i.CityID.ToString() }).Distinct().ToList();
            return Json(City, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Studentdelete(int id)
        {
            if (id != 0)
            {
                var dt_Delete = dbj.Students.Where(x => x.ID == id).FirstOrDefault();
                var dt = dbj.Sp_Delete_student(id, objectparam);
                var chk = objectparam.Value;
                //var chk = Convert.ToInt32(accid.Value);
                if (Convert.ToInt32(chk) == 1)
                {
                    TempData["mg"] = "DELETED SUCCESSFULLY !";
                }
                else
                {
                    TempData["mg"] = "SOMETHING WENT WRONG !";
                }
                return RedirectToAction("Create");
            }
            else
            {
                return RedirectToAction("Create");
            }
        }


        public ActionResult StudentLogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StudentLogin(Student usermodel)
        {
            var userDetails = dbj.Students.Where(model => model.Email == usermodel.Email && model.Mob_No == usermodel.Mob_No).FirstOrDefault();
            if (userDetails == null)
            {
                ViewBag.Tempmessage = "INVALID USERNAME OR PASSWORD";
                return View("StudentLogin", usermodel);
            }
            else
            {
               
                return RedirectToAction("Create", "Student");
            }
        }

        [AllowAnonymous, HttpGet()]
        public ActionResult Forgetpwd()
        {
            return View();
        }

        [AllowAnonymous, HttpPost()]
    
        public ActionResult Forgetpwd(resetpasswordmodel usermodel)
        {
            var userDetails = dbj.Students.Where(model => model.Email == usermodel.Email).FirstOrDefault();
            if (usermodel == null)
            {
                ViewBag.Tempmessage = "Email Id Not Register";
                return View("forgetpwd", usermodel);
            }
            else
            {
                string useremail = usermodel.Email;
                Random rn = new Random();
                int otp = rn.Next(1000, 9999);
                string body = otp.ToString();
                mail(useremail, body);
                return RedirectToAction("Admin_reset_pwd");

                //return RedirectToAction("adsendotp","Student");
            }
        }

        //[HttpGet]

        //public ActionResult adsendotp(resetpasswordmodel model)
        //{

        //    // string useremail = dbj.Students.Where(model => model.Email == user.Email).FirstOrDefault();
        //    //string useremail = email.ToString();
        //    string useremail = model.Email;
        //    //string useremail = "pkiit4922@gmail.com";
        //    //string sub = "Otp For Forget Password";
        //    Random rn = new Random();
        //    int otp = rn.Next(1000, 9999);
        //    string body = otp.ToString();
        //    mail(useremail, body);
        //    return View("Admin_reset_pwd");

        //}

        public void mail( string to, string body)
        {
           
                MailMessage mail = new MailMessage();
                mail.To.Add(to);
                mail.From = new MailAddress(to);
                mail.Subject = "Verify Your Account";
                string Body = $"Your OTP is <b> {body}</b>  <br/>thanks for choosing us.";
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
                // My Id is => forprojectemailid@gmail.com : password = "Project@123" : ConnectWith = 7470552359 
                // Login Id => go to Manage Yor Google Account => go to Security => Signing in to Google
                // => on 2Step Verification and go to App Password => Login => SMTP.Enablessl
                smtp.Credentials = new System.Net.NetworkCredential("kamalpratapsingh132@gmail.com", "KPsingh123@");
                // Enter seders User name and password  
                smtp.EnableSsl = true;
                smtp.Send(mail);

            

        }

        public ActionResult Admin_reset_pwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Admin_reset_pwd(resetpasswordmodel model)
        {
            

                var oldemail = model.Email;
                var newpass = model.Newpassword;
                var tbl_pass = dbj.Students.Where(x => x.Email == oldemail).FirstOrDefault();
                if (newpass != null)
                {

                   //
                }
                else
                {
                    ViewBag.Message = "Password does not Exist !";

                }
                ModelState.Clear();
                return View("StudentLogin");

            
           
        }
    }
}