using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        [Required]
        public UserDTO Author { get; set; }
        public GroupDTO Group { get; set; }

        public ICollection<UserDTO> Likes { get; set; }
        public ICollection<PostDTO> Comments { get; set; }

        public string ImageURL { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
