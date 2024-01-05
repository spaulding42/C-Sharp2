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
        if(HttpContext.Session.GetInt32("UUID") != null)
        {
            return RedirectToAction("Dashboard", "Home");
        }
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
                HttpContext.Session.SetInt32("UUID", (int)newUser.RoleId);
                return RedirectToAction("Dashboard","Home");
            }

            // if we found a user with the email entered that means they are already in the DB and shouldn't be added again
            if(dbUser != null)
            {
                // Add an error to ModelState and return to View!z
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
            HttpContext.Session.SetInt32("UUID", (int)dbUser.RoleId);
            return RedirectToAction("Dashboard", "Home");
        }
        return View("Index");
    }

    [HttpGet("/profile")]
    public IActionResult UserProfile()
    {
        User? dbUser = db.Users.FirstOrDefault(u=>u.RoleId == HttpContext.Session.GetInt32("UUID"));
        if (dbUser == null){
            return RedirectToAction("Logout", "Users");
        }
        return View("Profile", dbUser);
    }
    
    [HttpGet("/users/edit")]
    public IActionResult EditProfile()
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        if(UUID != null)
        {
            User? dbUser = db.Users.FirstOrDefault(u=> u.RoleId == UUID);
            if(dbUser != null)
            {
                return View("EditProfile", dbUser);
            }

        }
        return RedirectToAction("Logout", "Users");
    }
    [HttpPost("/profile/update")]
    public IActionResult UpdateProfile(User updatedUser)
    {
        User? dbUser = db.Users.FirstOrDefault(u=>u.RoleId == HttpContext.Session.GetInt32("UUID"));
        if (dbUser == null){
            return RedirectToAction("Logout", "Users");
        }

        updatedUser.Password = dbUser.Password;
        updatedUser.ConfirmPassword = dbUser.Password;
        updatedUser.RoleId = dbUser.RoleId;
        updatedUser.CreatedAt = dbUser.CreatedAt;
        updatedUser.UpdatedAt = dbUser.UpdatedAt;

        if(ModelState.IsValid)
        {
            dbUser.FirstName = updatedUser.FirstName;
            dbUser.LastName = updatedUser.LastName;
            dbUser.Email = updatedUser.Email;
            dbUser.Role = updatedUser.Role;
            dbUser.UpdatedAt = DateTime.Now;
            db.Update(dbUser);
            db.SaveChanges();
            return RedirectToAction("UserProfile", "Users");
        }
        else
        {
            return View("EditProfile", updatedUser);
        }
    }

    [HttpGet("/users/changepassword")]
    public IActionResult ChangePassword()
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? dbUser = db.Users.FirstOrDefault(u=> u.RoleId == UUID);

        // if logged in and user is in db then proceed to changepassword form
        if(UUID != null && dbUser != null) return View("ChangePassword");
                
        return RedirectToAction("Logout", "Users");
    }

    [HttpPost("/users/updatepassword")]
    public IActionResult UpdatePassword(UpdatePassword UpdatedPassword)
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? dbUser = db.Users.FirstOrDefault(u=> u.RoleId == UUID);

        if(UUID == null || dbUser == null) return RedirectToAction("Logout", "Users");
        
        if(ModelState.IsValid)
        {
            //  // since email exists, we now need to  check if the passwords match
            var hasher = new PasswordHasher<UpdatePassword>();
                
            //     // verify provided password against hash stored in db
            var result = hasher.VerifyHashedPassword(UpdatedPassword, dbUser.Password, UpdatedPassword.CurrentPassword);

            if(result == 0)
            {
                ModelState.AddModelError("CurrentPassword", "incorrect current password");
                return View("ChangePassword", UpdatedPassword);
            }

            PasswordHasher<UpdatePassword> Hasher = new PasswordHasher<UpdatePassword>();
            UpdatedPassword.NewPassword = Hasher.HashPassword(UpdatedPassword, UpdatedPassword.NewPassword);
            dbUser.Password = UpdatedPassword.NewPassword;
            db.Update(dbUser);
            db.SaveChanges();
            return RedirectToAction("UserProfile", dbUser);

        }
        return View("ChangePassword");
    }

    [HttpGet("/users/forgotpassword")]
    public IActionResult ForgotPassword()
    {
        ForgotUser Forgot = new ForgotUser();
        Forgot.ForgotDoB = DateTime.Now;
        return View("ForgotPassword", Forgot);
    }
    [HttpPost("/users/recoverpassword")]
    public IActionResult RecoverPassword(ForgotUser forgotUser)
    {
        if(ModelState.IsValid)
        {
            User? dbUser = db.Users.FirstOrDefault(u => u.Email == forgotUser.ForgotEmail);
            
            if(dbUser == null)
            {
                ModelState.AddModelError("ForgotEmail", "not found");
                return View("ForgotPassword", forgotUser);
            }

            if(dbUser != null)
            {
                if(forgotUser.ForgotDoB.Date != dbUser.DoB.Date)
                {
                    ModelState.AddModelError("ForgotDoB", "doesn't match DoB for email listed");
                    return View("ForgotPassword", forgotUser);
                }
                UpdatePassword newPass = new UpdatePassword();
                newPass.CurrentPassword = dbUser.Password;
                HttpContext.Session.SetInt32("UUID", (int)dbUser.RoleId);
                return View("NewPassword", newPass);
            }
        }
        return View("ForgotPassword");
    }

    [HttpPost("/users/updatedrecoveredpassword")]
    public IActionResult UpdateRecoveredPassword(UpdatePassword UpdatedPassword)
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? dbUser = db.Users.FirstOrDefault(u=> u.RoleId == UUID);

        if(UUID == null || dbUser == null) return RedirectToAction("Logout", "Users");
        
        if(ModelState.IsValid)
        {
            PasswordHasher<UpdatePassword> Hasher = new PasswordHasher<UpdatePassword>();
            UpdatedPassword.NewPassword = Hasher.HashPassword(UpdatedPassword, UpdatedPassword.NewPassword);
            dbUser.Password = UpdatedPassword.NewPassword;
            db.Update(dbUser);
            db.SaveChanges();
            return RedirectToAction("UserProfile", dbUser);

        }
        return View("NewPassword", UpdatedPassword);
    }
    [HttpGet("/logout")]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Users");
    }

    [HttpGet("/users/delete/confirm")]
    public IActionResult DeleteForm()
    {
        User? dbUser = db.Users.FirstOrDefault(u=>u.RoleId == HttpContext.Session.GetInt32("UUID"));
        if (dbUser == null) return RedirectToAction("Dashboard", "Home");

        ViewBag.firstName = dbUser.FirstName;
        ViewBag.lastName = dbUser.LastName;
        return View("DeleteForm");
    }

    [HttpPost("/users/delete")]
    public IActionResult DeleteUser(DeleteConfirm confirm)
    {
        int? UUID = HttpContext.Session.GetInt32("UUID");
        User? dbUser = db.Users.FirstOrDefault(u=>u.RoleId == UUID);

        if(confirm.Confirmation == "delete")
        {
            if(dbUser != null)
            {
                if(dbUser.Role == "Salesman")
                {
                    List<Account>? assignedAccounts = db.Accounts.Where(a=>a.SalesId == UUID).ToList();
                    if (assignedAccounts != null)
                    {
                        foreach(Account account in assignedAccounts)
                        {
                            account.SalesId = -1;
                            db.Update(account);
                            db.SaveChanges();
                        }
                    }

                }
                if(dbUser.Role == "Technician")
                {
                    List<Account>? assignedAccounts = db.Accounts.Where(a=>a.TechId == UUID).ToList();
                    if (assignedAccounts != null)
                    {
                        foreach(Account account in assignedAccounts)
                        {
                            account.TechId = -1;
                            db.Update(account);
                            db.SaveChanges();
                        }
                    }

                }
                db.Remove(dbUser);
                db.SaveChanges();
                HttpContext.Session.Clear();
                return RedirectToAction("Index", "Users");
            }
        }
        else{
            if(dbUser == null) return RedirectToAction("Dashboard", "Home");

            ModelState.AddModelError("Confirmation", "must type 'delete' to confirm");
            ViewBag.firstName = dbUser.FirstName;
            ViewBag.lastName = dbUser.LastName;
            return View("DeleteForm");
        }
        return RedirectToAction("Index", "Users");

    }

    [HttpGet("/users/{userid}/view")]
    public IActionResult ViewUser(int userid)
    {
        User? UserById = db.Users.FirstOrDefault(u=>u.RoleId == userid);
        if(UserById == null) return RedirectToAction("Dashboard", "Home");

        return View("ViewUser", UserById);
        
    }

}