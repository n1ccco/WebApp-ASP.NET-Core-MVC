using System.ComponentModel.DataAnnotations;
using WebApp.Models;

namespace WebApp.ViewModels
{
	public class PostDetailsViewModel
	{
		public Post Post { get; set; }
		public Comment? Comment { get; set; }
	}
}
