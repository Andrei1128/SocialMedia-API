namespace SocialMedia.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }
        public ICollection<UserRequest> Requests { get; set; }
        public ICollection<UserDTO> Friends { get; set; }
        public ICollection<PostDTO> Posts { get; set; }
        public ICollection<GroupDTO> Groups { get; set; }

        public string Name { get; set; }
        public string ImageURL { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
