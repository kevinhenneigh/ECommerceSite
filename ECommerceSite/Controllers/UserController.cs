using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceSite.Data;
using ECommerceSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceSite.Controllers
{
    public class UserController : Controller
    {
        private readonly ProductContext _context;

        public UserController(ProductContext context)
        {
            _context = context;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel register)
        {
            if (ModelState.IsValid)
            {
                // Check if username/email is taken
                bool isEmailTaken = await (from acc in _context.UserAccounts
                                     where acc.Email == register.Email
                                     select acc).AnyAsync();
                // if yes, add a custom error and send back to view
                if (isEmailTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Email), "That Email is already in use");       
                }

                bool isUsernameTaken = await (from acc in _context.UserAccounts
                                           where acc.Username == register.Username
                                           select acc).AnyAsync();
                // if yes, add a custom error and send back to view
                if (isUsernameTaken)
                {
                    ModelState.AddModelError(nameof(RegisterViewModel.Username), "That Username is already in use");  
                }
                if (isEmailTaken || isUsernameTaken)
                {
                    return View(register);
                }
                {

                }

                // Map data to user account instance
                UserAccount account = new UserAccount()
                {
                    DateOfBirth = register.DateOfBirth,
                    Email = register.Email,
                    Password = register.Password,
                    Username = register.Username
                };

                // Add to database
                _context.UserAccounts.Add(account);
                await _context.SaveChangesAsync();

                // Log user in immediately after registration
                LogUserIn(account.UserId);

                // redirect to homepage
                return RedirectToAction("Index", "Home");
            }
            return View(register);
        }
        public IActionResult Login()
        {
            // Check if user already logged in
            if (HttpContext.Session.GetInt32("UserId").HasValue)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Query syntax
            //UserAccount account = await (from u in _context.UserAccounts
            //                             where (u.Username == model.UsernameOrEmail
            //                               || u.Email == model.UsernameOrEmail)
            //                               && u.Password == model.Password
            //                             select u).SingleOrDefaultAsync();

            //Same as above with method syntax
            UserAccount account = await _context.UserAccounts.
                Where(userAcc => (userAcc.Username == model.UsernameOrEmail ||
                                   userAcc.Email == model.UsernameOrEmail) &&
                                   userAcc.Password == model.Password)
                .SingleOrDefaultAsync();

            if (account == null)
            {
                // Credentials did not match

                // Custom error message
                ModelState.AddModelError(string.Empty, "Credentials do not match our records");

                return View(model);
            }

            LogUserIn(account.UserId);

            return RedirectToAction("Index", "Home");
        }

        private void LogUserIn(int accountId)
        {
            // Log user into website
            HttpContext.Session.SetInt32("UserId", accountId);
        }

        public IActionResult Logout()
        {
            // Remove all current session data
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home");
        }
    }
}
