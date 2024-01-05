// Disabled because we know the framework will assign non-null values when it
// constructs this class for us.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
using System.ComponentModel.DataAnnotations;
namespace Vanlife.Models;

public class Post
{
    
    [Key] // Primary Key
    public int PostId { get; set; }

    [Required(ErrorMessage = "is required")]
    [MinLength(2, ErrorMessage = "must be more than 2 characters.")]
    public string Message { get; set; }

    [Display(Name = "Image Url")]
    public string? ImgUrl { get; set; }

    [Display(Name =  "Website Url")]
    public string? WebUrl {get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
    /**********************************************************************
    Relationship properties below

    Foreign Keys: id of a different (foreign) model.

    Navigation props:
        data type is a related model
        MUST use .Include for the nav prop data to be included via a SQL JOIN.
    **********************************************************************/
    public int UserId { get; set; } // this foreign key MUST match primary property name
    public User? Author { get; set; } // the ONE user related to each post

    // One to Many - 1 post can be liked by many users
    public List<Comment> PostsComments { get; set; } = new List<Comment>();

}