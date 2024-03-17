using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
	public class PostDetailsViewModel
	{
		[Required]
		public Post Post { get; set; }
		[Required]
		public Comment? Comment { get; set; }
	}
}
