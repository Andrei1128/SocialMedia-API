using System.ComponentModel.DataAnnotations;

namespace FirstProject_API.Models.DTOs
{
    public class PostDTO
    {
        public int Id { get; set; }
        [Required]
        public int AuthorId { get; set; }
        public User Author { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public ICollection<User> Likes { get; set; }
        public ICollection<Post> Comments { get; set; }

        public ICollection<string> ImageURLs { get; set; }
        [Required]
        public string Content { get; set; }
    }
}
