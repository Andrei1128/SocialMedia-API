using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models.DTOs
{
    public class PostCreatedDTO
    {
        public int GroupId { get; set; }
        public string ImageURL { get; set; }
        [Required]
        public string Content { get; set; }

    }
}
