using System.ComponentModel.DataAnnotations.Schema;

namespace WebApp.Models;

public class Comment
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedDateTime { get; set; } = DateTime.UtcNow;


    [ForeignKey("Post")]
    public int PostId { get; set; }
    
    public virtual Post Post { get; set; }
}
