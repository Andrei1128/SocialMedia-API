namespace SocialMedia.Models.DTOs
{
    public class UserDTO
    {
        public int Id { get; set; }

        public ICollection<UserRequest> Requests { get; set; }
        public ICollection<UserFriend> Friends { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Group> Groups { get; set; }

        public string Name { get; set; }
        public string ImageURL { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
