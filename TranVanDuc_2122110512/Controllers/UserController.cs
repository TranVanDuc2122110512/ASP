using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using TranVanDuc_2122110512.Context;

namespace TranVanDuc_2122110512.Controllers
{
    public class UserController : Controller
    {
        OnlineShopEntities4 objModel = new OnlineShopEntities4();

        // GET: Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Register _user)
        {
            if (ModelState.IsValid)
            {
                var check = objModel.Registers.FirstOrDefault(s => s.Email == _user.Email);
                if (check == null)
                {
                    _user.Password = GetMD5(_user.Password);
                    objModel.Configuration.ValidateOnSaveEnabled = false;
                    objModel.Registers.Add(_user);
                    objModel.SaveChanges();
                    return RedirectToAction("Login", "User");
                }
                else
                {
                    ViewBag.error = "Email already exists";
                    return View();
                }
            }
            return View();
        }

        // Create a string MD5
        public static string GetMD5(string str)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] fromData = Encoding.UTF8.GetBytes(str);
                byte[] targetData = md5.ComputeHash(fromData);
                return BitConverter.ToString(targetData).Replace("-", "").ToLower();
            }
        }

        // GET: Login
        public ActionResult Login()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var f_password = GetMD5(password);
                var user = objModel.Registers
                    .FirstOrDefault(s => s.Email == email && s.Password == f_password);

                if (user != null)
                {
                    // Add session
                    Session["FullName"] = user.FirstName + " " + user.LastName;
                    Session["Email"] = user.Email;
                    Session["idUser"] = user.Id;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.error = "Login failed";
                }
            }
            return View();
        }

        // GET: Logout
        public ActionResult Logout()
        {
            Session.Clear(); // Remove session
            return RedirectToAction("Index", "Home");
        }
    }
}
