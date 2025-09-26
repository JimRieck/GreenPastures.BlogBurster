namespace GreenPastures.BlogBurster.Models
{
    public class BlogPost
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Summary { get; set; } = string.Empty;
        public DateTime PublishDate { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public List<string> Categories { get; set; } = new List<string>();
    }
}