using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models.DTOs
{
    public class GroupDTO
    {
        public int Id { get; set; }
        [Required]
        public ICollection<User> Participants { get; set; }
        public ICollection<Post> Posts { get; set; }
        [Required]
        public string Name { get; set; }
        public string About { get; set; }
        public string ImageURL { get; set; }
    }
}
