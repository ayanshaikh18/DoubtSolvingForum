using DoubtSolvingForum.ViewModels;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DoubtSolvingForum.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        public AccountController(UserManager<IdentityUser> userManager,
                                 SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser()
                {
                    UserName = model.Username,
                    Email = model.Email,
                };
                var result = await userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("list","question");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Username, model.Password,isPersistent:model.RememberMe,false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(model.Username);
                    await signInManager.SignInAsync(user, isPersistent: false);
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("list", "question");
                }
                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await signInManager.SignOutAsync();
            return RedirectToAction("login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var resetPasswordLink = Url.Action("ResetPassword", "Account",
                                         new { email = model.Email, token = token }, Request.Scheme);

                    var msg = new MimeMessage();
                    msg.From.Add(new MailboxAddress("Admin", "mahammadayan18@gmail.com"));
                    msg.To.Add(new MailboxAddress(user.UserName, model.Email));
                    msg.Subject = "Reset Password of your Student Portal Account";
                    msg.Body = new TextPart("plain")
                    {
                        Text = "To reset your password, use the provided link\n\n " + resetPasswordLink
                    };
                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 587, false);
                        client.Authenticate("mahammadayan18@gmail.com", "Ayan786@");
                        client.Send(msg);
                        client.Disconnect(true);
                    }
                }
                return View("ConfirmForgotPassword", "account");
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token == null || email == null)
                ModelState.AddModelError("", "Invalid Attempt");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.email);
                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.token, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("login", "account", new { msg = "reset" });
                    }
                    foreach (var error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
                else
                    ModelState.AddModelError("", "Something Went Wrong");
            }
            return View(model);
        }
    }
}
