using InventoryManagement.Data;
using InventoryManagement.Models.Account;
using InventoryManagement.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InventoryManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationContext context;

        public AccountController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        /// <summary>
        /// This method is used to register the user before login to the application
        /// </summary>
        /// <param name="model"></param>
        /// <returns>It returns the registered user</returns>
        [HttpPost]
        public IActionResult SignUp(SignUpUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = new User()
                {
                Username=model.Username,
                Email=model.Email,
                Password=model.Password,
                Mobile=model.Mobile,
                IsActive=model.IsActive
                };
                context.Users.Add(data);
                context.SaveChanges();
                TempData["successMessage"] = "Registered Successfully";
                return RedirectToAction("Login");
                
            }
            else
            {
                
                TempData["errorMessage"] = "Empty form can't be submitted";
                return View();
            }
            
        }
        /// <summary>
        /// This method is used to check if the user is already exist or not
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>if usernamre exist then it will return the error message or else it will allow the user for register</returns>
        [AcceptVerbs("Post","Get")]
        public IActionResult UserNameIsExist(string userName)
        {
            var data = context.Users.Where(e => e.Username == userName).FirstOrDefault();
            if (data != null)
            {
                return Json($"Username {userName} already exist");
            }
            else
            {
                return Json(true);
            }
        }
        //login

        public IActionResult Login()
        {

            return View();
        }
        /// <summary>
        /// This method is used for login the user into the application
        /// </summary>
        /// <param name="model"></param>
        /// <returns>it validates the user and returns the status</returns>
        [HttpPost]
        public IActionResult Login(LoginSignUpViewModel model)
        {
            if (ModelState.IsValid)
            {
                var data = context.Users.Where(e => e.Username == model.Username).FirstOrDefault();
                if (data != null)
                {
                    bool isValid = (data.Username == model.Username && data.Password == model.Password);
                    if (isValid)
                    {
                        //stored user info
                        var identity = new ClaimsIdentity(new[] {new  Claim(ClaimTypes.Name, model.Username)},
                        CookieAuthenticationDefaults.AuthenticationScheme);
                        //pass the user info
                        var principal = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                        HttpContext.Session.SetString("Username", model.Username);
                        return RedirectToAction("Index", "Unit");
                    }
                    else
                    {
                        TempData["errorMessage"] = "invalid password";
                        return View(model);
                    }
                }
                else
                {
                    TempData["errorMessage"] = "Username not found";
                    return View();
                }
            }
            else
            {
                return View();
            }
            
        }

        /// <summary>
        /// This method is used to logout the user from the application and removes the cookie
        /// </summary>
        /// <returns>It logout the user and redirects to Login page</returns>
        public IActionResult LogOut()
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var storedCookies = Request.Cookies.Keys;
            foreach(var cookies in storedCookies)
            {
                Response.Cookies.Delete(cookies);
            }
            return RedirectToAction("Login", "Account");
        }
    }
}
