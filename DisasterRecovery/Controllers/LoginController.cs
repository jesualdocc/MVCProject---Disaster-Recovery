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

namespace DisasterRecovery.Controllers
{
    public class LoginController : Controller
    {
        private DisasterRecoveryEntities db = new DisasterRecoveryEntities();

        // GET: Login
        public ActionResult Index()
        {
            var users = db.Users.Include(u => u.SubContractor);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index([Bind(Include = "UserName,UserPassWord")] User user, bool? remember)
        {

            if (!String.IsNullOrEmpty(user.UserName))
            {
                string myPaswd = HashPaswd(user.UserPassWord);
                User myUser = db.Users.FirstOrDefault
                (u => u.UserName.Equals(user.UserName) && u.UserPassWord.Equals(myPaswd));

                if (myUser == null) {
                ViewBag.Message = "Incorrect username or password."; }

                else
                {
                    Session["LogedUserID"] = myUser.IdUser.ToString();
                    Session["LogedUserName"] = myUser.FirstName + " " + myUser.LastName;

                    if (myUser.IsAdm == 1)
                        Session["LogedUserRole"] = "Admin";
                    else
                        Session["LogedUserRole"] = "Contractor";

                    return RedirectToAction("Index", "Home");
                }
            }
            return View();
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
            ViewBag.Message = "Logged Out successfully!";
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
