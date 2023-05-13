using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        [Required]
        public UserDTO Author { get; set; }
        public Group GroupDTO { get; set; }

        public ICollection<User> Likes { get; set; }
        public ICollection<Post> Comments { get; set; }

        public ICollection<string> ImageURLs { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
