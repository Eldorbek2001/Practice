namespace Practical.Models
{
    public class PostListViewModel
    {
        public List<Post> Posts { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; }
    }
}
