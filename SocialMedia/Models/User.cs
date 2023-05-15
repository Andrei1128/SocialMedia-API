namespace SocialMedia.Models
{
    public class User
    {
        public int Id { get; set; }

        public ICollection<UserRequest> Requests { get; set; }
        public ICollection<User> Friends { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Group> Groups { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ImageURL { get; set; }
        public DateTime BirthDate { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}
