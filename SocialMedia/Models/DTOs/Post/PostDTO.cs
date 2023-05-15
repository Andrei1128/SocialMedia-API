using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models.DTOs
{
    public class PostDTO
    {
        [Required]
        public UserDTO Author { get; set; }
        public GroupDTO Group { get; set; }

        public ICollection<User> Likes { get; set; }
        public ICollection<Post> Comments { get; set; }

        public string ImageURL { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
