
using Microsoft.AspNetCore.Mvc;
using Vanlife.Models;
using Microsoft.AspNetCore.Identity;

namespace Vanlife.Controllers;

public class UsersController : Controller
{
    private VanlifeContext db;

    public UsersController (VanlifeContext DB)
    {
        db = DB;
    }

    [HttpGet("/")]
    public IActionResult Index()
    {
        if(HttpContext.Session.GetInt32("UUID") != null)
        {
            // if a something is in session, check to see if that corresponds to a valid user. if so go straight to Posts page
            User? loggedUser = db.Users.FirstOrDefault(u=>u.UserId == HttpContext.Session.GetInt32("UUID"));
            if (loggedUser != null)
            {
                return RedirectToAction("AllPosts", "Posts");
            }
        }
        return View("Index");
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
                // now we can add it into the DB
                db.Users.Add(newUser);
                db.SaveChanges();
                HttpContext.Session.SetInt32("UUID", newUser.UserId);
                return RedirectToAction("AllPosts","Posts");
            }

            // if we found a user with the email entered that means they are already in the DB and shouldn't be added again
            if(dbUser != null)
            {
                // Add an error to ModelState and return to View!
                ModelState.AddModelError("Email", "already in use");
            }
        }
        return Index();
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
                return Index();
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
                return Index();
            }
            HttpContext.Session.SetInt32("UUID", dbUser.UserId);
            return RedirectToAction("AllPosts", "Posts");
        }
        return Index();
    }

    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        Console.WriteLine("Logging out");
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Users");
    }
}