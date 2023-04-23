namespace Practical.Models
{
    public class PostListViewModel
    {
        public List<PostUpdated> UpdatedPosts { get; set; }        
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Search { get; set; }
        public string Phrase { get; set; }

    }
}
