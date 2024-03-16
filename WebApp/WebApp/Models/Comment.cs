using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public class Comment
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;
    

    [ForeignKey("Author")]
    public int AuthorId { get; set; }
    
    public virtual User Author { get; set; }


    [ForeignKey("Post")]
    public int PostId { get; set; }
    
    public virtual Post Post { get; set; }
}
