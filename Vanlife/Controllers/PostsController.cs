
using Microsoft.AspNetCore.Mvc;
using Vanlife.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Vanlife.Controllers;

public class PostsController : Controller
{
    private VanlifeContext db;

    public PostsController (VanlifeContext DB)
    {
        db = DB;
    }

    [HttpGet("/posts")]
    public IActionResult AllPosts()
    {
        int? uid = HttpContext.Session.GetInt32("UUID");
        if (uid == null) return RedirectToAction("Index", "Users");
        List<Post> allPosts = db.Posts.Include(p=>p.Author).Include(p=>p.PostsComments).ToList();

        return View("AllPosts", allPosts);
    }

    [HttpPost("/posts/new")]
    public IActionResult Create(Post newPost)
    {
        int? uid = HttpContext.Session.GetInt32("UUID");
        if (uid == null) return RedirectToAction("Index", "Users");

        if(ModelState.IsValid)
        {
            newPost.UserId = (int)uid;
            db.Add(newPost);
            db.SaveChanges();
            return RedirectToAction("AllPosts");
        }
        return AllPosts();
    }

    [HttpGet("/posts/{postId}")]
    public IActionResult ViewOne(int postId)
    {
        int? uid = HttpContext.Session.GetInt32("UUID");
        if (uid == null) return RedirectToAction("Index", "Users");

        Post? dbPost = db.Posts
            .Include(p=>p.Author)
            .Include(p=>p.PostsComments)
                .ThenInclude(p=>p.Author)
            .FirstOrDefault(p=>p.PostId == postId);
        
        if(dbPost != null)
        {
            return View("ViewOne", dbPost);
        }
        return AllPosts();
    }

    [HttpPost("/comments/new")]
    public IActionResult CreateComment(Comment newComment)
    {
        int? uid = HttpContext.Session.GetInt32("UUID");
        if (uid == null) return RedirectToAction("Index", "Users");
        
        Post? dbPost = db.Posts
            .Include(p=>p.Author)
            .Include(p=>p.PostsComments)
                .ThenInclude(p=>p.Author)
            .FirstOrDefault(p=>p.PostId == newComment.PostId);
        
        if(ModelState.IsValid)
        {
            Console.WriteLine("Valid Submission!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
            Comment dbComment = new Comment();
            dbComment.MessageComment = newComment.MessageComment;
            dbComment.ImgUrl = newComment.ImgUrl;
            dbComment.WebUrl = newComment.WebUrl;

            dbComment.UserId = (int)uid;
            dbComment.PostId = newComment.PostId;
            Console.WriteLine($"Comment: {dbComment.MessageComment}\n ImgUrl: {dbComment.ImgUrl}\n WebUrl: {dbComment.WebUrl}\n Created at: {dbComment.CreatedAt}\n Updated at: {dbComment.UpdatedAt}\n UserId: {dbComment.UserId}\n PostId: {dbComment.PostId}");
            db.Add(dbComment);
            db.SaveChanges();
        }
        return RedirectToAction("ViewOne", dbPost);
    }

    [HttpGet("/posts/{postId}/delete")]
    public IActionResult Delete(int postId)
    {
        int? uid = HttpContext.Session.GetInt32("UUID");
        if (uid == null) return RedirectToAction("Index", "Users");
        Console.WriteLine("made it here!");

        Post? dbPost = db.Posts.Include(p=>p.PostsComments).FirstOrDefault(p=>p.PostId == postId);
        if(dbPost != null && uid == dbPost.UserId)
        {
            foreach (Comment cmt in dbPost.PostsComments)
            {
                db.Remove(cmt);
            }
            db.Remove(dbPost);
            db.SaveChanges();
        }
        return RedirectToAction("AllPosts");
    }

    [HttpPost("/comments/{commentId}/delete")]
    public IActionResult DeleteComment(int commentId)
    {
        return ViewOne(1);
    }
}