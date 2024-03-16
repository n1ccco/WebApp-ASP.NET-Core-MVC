using System.ComponentModel.DataAnnotations;

namespace WebApp.Models;

public class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public virtual ICollection<Post> Posts { get; set; }
}

