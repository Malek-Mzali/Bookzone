using System.Security.Claims;
using System.Threading.Tasks;
using Bookzone.Extensions;
using Bookzone.Models.BLL;
using Bookzone.Models.DAL;
using Bookzone.Models.Entity.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;

namespace Bookzone.Controllers
{
    public class AccountController : Controller
    {
        private readonly IToastNotification _notification;
        public AccountController(IToastNotification notification)
        {
            _notification = notification;
        }
        public IActionResult Index()
        {
            return RedirectToAction("Login");
        }

        #region SignUp
            public IActionResult Signup()
            {
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
                else
                    return View("Signup/Index");
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult Signup(Users usr)
            {
                if (!ModelState.IsValid) return View("Signup/Index");
                var operation = BllAccount.SignupApi(usr, _notification);
                return View(operation.Success ? "Login/index" : "Signup/index");
            }
            public  IActionResult ActivateUser(string code)
            {
                if (string.IsNullOrEmpty(code)) return RedirectToAction("Index", "Home");
                var operation = BllAccount.ActivateApi(code,_notification);
                return operation == null ?  RedirectToAction("Index", "Home") : View("Login/Index");
            }
        #endregion

        #region Login
            public IActionResult Login()
            {
                if (!User.Identity.IsAuthenticated) return View("Login/index");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var userRole = User.FindFirstValue(ClaimTypes.Role);
                return userRole switch
                {
                    "Individual" => DalAccount.CheckFieldUnicityIndv("Id", userId)
                        ?  RedirectToAction("Index", "Home")
                        : View("Signup/profile"),
                    _ => RedirectToAction("Index", "Home")
                };
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public async Task<IActionResult> Login(Users u)
            {
                var user = BllAccount.LoginApi(u,_notification);
                if (user == null) return View("Login/Index");
                var identity = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, user.Email), new Claim(ClaimTypes.Role, user.Role), new Claim(ClaimTypes.Surname, user.Photo), new Claim(ClaimTypes.Country, user.Country)
                }, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                return RedirectToAction("Index");
                
            }
            public IActionResult Reset()
            {
                if (User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Home");
                return View("Login/Reset");
            }
            [HttpPost]
            public IActionResult Reset(string email)
            {
                var operation = BllAccount.ResetApi(email,_notification);
                return operation.Success == false ?  View("Login/Reset") : View("Login/Index");
            }
            public  IActionResult ResetPassword(string code)
            {
                if (string.IsNullOrEmpty(code)) return RedirectToAction("Index", "Home");
                var operation = BllAccount.ResetPasswordApi(code,_notification).Success;
                return operation == false ?  View("Login/Index") : View("Login/ResetForm", code);
            }
            [HttpPost]
            public  IActionResult ResetForm(string code, string NewPassword)
            {
                if (string.IsNullOrEmpty(code)) return RedirectToAction("Index", "Home");
                if (string.IsNullOrEmpty(NewPassword)) return RedirectToAction("Index", "Home");
                BllAccount.ResetUserPassword(code,NewPassword);
                return RedirectToAction("Login", "Account");
            }
        #endregion
        
        #region Profile
            public IActionResult ProfileGenerator()
            {
                return RedirectToAction("Index", "Home");
            }
            [HttpPost]
            [ValidateAntiForgeryToken]
            public IActionResult ProfileGenerator(Individual indvusr)
            {
                if (!ModelState.IsValid) return View("Signup/profile");
                BllAccount.ProfileApi(indvusr, User.FindFirstValue(ClaimTypes.NameIdentifier), _notification);
                return RedirectToAction("Index", "Home");
            }
        #endregion

        #region LogOut
            public async Task<IActionResult> Logout()  
            {  
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);  
                SessionHelper.SetObjectAsJson(HttpContext.Session, "organizationId", null);
                SessionHelper.SetObjectAsJson(HttpContext.Session, "organizationName", null);
                return RedirectToAction("Index", "Home");  
            }
        #endregion
    }
}