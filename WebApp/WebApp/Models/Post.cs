using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public class Post
{
    public Post()
    {
        Categories = new List<Category>();
        Comments = new List<Comment>();
    }

    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;

    [ForeignKey("Author")]
    public int AuthorId { get; set; }
    
    public virtual User Author { get; set; }


    public virtual ICollection<Category> Categories { get; set; }
    public virtual ICollection<Comment> Comments { get; set; }

}
