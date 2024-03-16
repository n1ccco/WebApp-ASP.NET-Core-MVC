using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
    public class PostCreateViewModel
    {
        public Post Post { get; set; }
        public int[] SelectedCategories { get; set; }
    }
}

