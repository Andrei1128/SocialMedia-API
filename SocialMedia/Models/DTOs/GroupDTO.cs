namespace FirstProject_API.Models
{
    public class GroupDTO
    {
        public int Id { get; set; }

        public ICollection<User> Participants { get; set; }
        public ICollection<Post> Posts { get; set; }

        public string Name { get; set; }
        public string About { get; set; }
        public string ImageURL { get; set; }
    }
}
