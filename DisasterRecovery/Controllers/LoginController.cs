using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DisasterRecovery.Models;
using Microsoft.Ajax.Utilities;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Web.Security;
using DisasterRecovery.Infrastructure;

namespace DisasterRecovery.Controllers
{
    
    public class LoginController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        
        // GET: Login
        public ActionResult Index()
        {
            if (Session["Message"] != null)
            {
                ViewBag.Message = Session["Message"];
                Session["Message"] = null;
            }
            var users = db.Users.Include(u => u.SubContractor);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "UserName,UserPassWord")] User user)
        {

            if (!String.IsNullOrEmpty(user.UserName))
            {
                string myPaswd = HashPaswd(user.UserPassWord);
                User myUser = db.Users.FirstOrDefault
                (u => u.UserName.Equals(user.UserName) && u.UserPassWord.Equals(myPaswd));
                
                if (myUser == null) {
                ViewBag.Message = "Incorrect username or password."; }
                else if (myUser.UserStatus == "0")
                {
                    ViewBag.Message = "Sorry this user is not Active, Please Contact the Administrator.";
                }
                else
                {
                    int id = myUser.IdUser;

                    Session["LogedUserID"] = myUser.IdUser.ToString();
                    Session["LogedUserName"] = myUser.FirstName + " " + myUser.LastName;
                    
                    if (myUser.IsAdm == 1)
                    {
                        Session["LogedUserRole"] = "Admin";
                        Session["Users"] = myUser.UserName;
                        if (TempPaswd(myUser.UserName, myUser.UserPassWord))
                        {
                            Session["AlertNew"] = 1;
                            return RedirectToAction("EditPass", "Users", new { id });
                        }
                    }
                    else
                    {
                        Session["LogedUserRole"] = "Contractor";
                        if (TempPaswd(myUser.UserName, myUser.UserPassWord)) 
                        {
                            Session["AlertNew"] = 1;
                            return RedirectToAction("EditPass", "Users", new { id });
                        }
                            
                        return RedirectToAction("Index", "TimeCards");

                    }
                    return RedirectToAction("IndexAdm", "TimeCards");
                }
            }
            return View();
        }
        public static bool TempPaswd(string UserName, string Pass)
        {
            
            if(Pass == HashPaswd(UserName + "1234"))
            {
                return true;
            }

            return false;

        }
        public static string HashPaswd(string Pass)
        {
            //create new instance of md5
            MD5 md5 = MD5.Create();

            //convert the input text to array of bytes
            byte[] hashData = md5.ComputeHash(Encoding.Default.GetBytes(Pass));

            //create new instance of StringBuilder to save hashed data
            StringBuilder returnValue = new StringBuilder();

            //loop for each byte and add it to StringBuilder
            for (int i = 0; i < hashData.Length; i++)
            {
                returnValue.Append(hashData[i].ToString());
            }

            // return hexadecimal string
            return returnValue.ToString();

        }

        public bool ValidateHashPass(string inputPass, string storedHashPass)
        {
            
            string getHashInputData = HashPaswd(inputPass);

            if (string.Compare(getHashInputData, storedHashPass) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult Logout()
        {
            Session["LogedUserID"] = null;
            Session["LogedUserName"] = null;
            Session["LogedUserRole"] = null;
            Session["Message"] = "Logged out successfully!";
            return RedirectToAction("Index", "Login");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
