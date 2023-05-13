using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models.DTOs
{
    public class GroupCreatedDTO
    {
        [Required]
        public ICollection<User> Participants { get; set; }
        public ICollection<Post> Posts { get; set; }

        [Required]
        public string Name { get; set; }
        public string About { get; set; }
        public string ImageURL { get; set; }
    }
}
