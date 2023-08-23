using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SecurityApp.Models;
using Microsoft.AspNetCore.Identity;

namespace SecurityApp.Controllers;

public class UsersController : Controller
{
    private MyContext db;
    public UsersController (MyContext DB)
    {
        db = DB;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost("/register")]
    public IActionResult Register(User newUser)
    {
        if(ModelState.IsValid)
        {
            // if they entered values that passed the validations we now need to see if it isnt already in the DB
            User? dbUser = db.Users.FirstOrDefault(u=>u.Email == newUser.Email);
            if (dbUser == null)  // meaning it couldn't find a user with that email address in the DB
            {
                // need to hash the password before adding to DB
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                newUser.CreatedAt = DateTime.Now;
                newUser.UpdatedAt = DateTime.Now;

                //generate a random tech/sales ID and keep looping throgh until an ID with the given number is not in the DB
                Random rand = new Random();
                int newID = rand.Next(1000,10000);
                User? newRoleID = db.Users.FirstOrDefault(id=>id.RoleId == newID);
                while (newRoleID != null)
                {
                    newID = rand.Next(1000,10000);
                    newRoleID = db.Users.FirstOrDefault(id=>id.RoleId == newID);
                } 
                newUser.RoleId = newID;

                // now we can add it into the DB
                db.Users.Add(newUser);
                db.SaveChanges();
                HttpContext.Session.SetInt32("UUID", newUser.UserId);
                return RedirectToAction("Dashboard","Home");
            }

            // if we found a user with the email entered that means they are already in the DB and shouldn't be added again
            if(dbUser != null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Email", "already in use");
            }
        }
        return View("Index");
    }
    
    [HttpPost("/login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if(ModelState.IsValid)
        {
            User? dbUser = db.Users.FirstOrDefault(u=>u.Email == loginUser.LoginEmail);
            if (dbUser == null) //meaning it looked in he DB and no use with that email was found
            {
                ModelState.AddModelError("LoginEmail", "invalid Email/Password");
                return View("Index");
            }

            // since email exists, we now need to  check if the passwords match
            var hasher = new PasswordHasher<LoginUser>();
            
            // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);
                
            // result can be compared to 0 for failure
            if(result == 0)
            {
                // handle failure (this should be similar to how "existing email" is handled)
                ModelState.AddModelError("LoginEmail", "invalid Email/Password");
                return View("Index");
            }
            HttpContext.Session.SetInt32("UUID", dbUser.UserId);
            return RedirectToAction("Dashboard", "Home");
        }
        return View("Index");
    }

    [HttpPost("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}