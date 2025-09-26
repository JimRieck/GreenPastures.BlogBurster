using GreenPastures.BlogBurster.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GreenPastures.BlogBurster.Services
{
    // WordPress API Response Models
    public class WordPressPost
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("date")]
        public DateTime Date { get; set; }

        [JsonPropertyName("modified")]
        public DateTime Modified { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("link")]
        public string Link { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public WordPressTitle Title { get; set; } = new();

        [JsonPropertyName("content")]
        public WordPressContent Content { get; set; } = new();

        [JsonPropertyName("excerpt")]
        public WordPressExcerpt Excerpt { get; set; } = new();

        [JsonPropertyName("author")]
        public int AuthorId { get; set; }

        [JsonPropertyName("featured_media")]
        public int FeaturedMediaId { get; set; }

        [JsonPropertyName("categories")]
        public List<int> CategoryIds { get; set; } = new();

        [JsonPropertyName("tags")]
        public List<int> TagIds { get; set; } = new();

        [JsonPropertyName("_embedded")]
        public WordPressEmbedded? Embedded { get; set; }
    }

    public class WordPressTitle
    {
        [JsonPropertyName("rendered")]
        public string Rendered { get; set; } = string.Empty;
    }

    public class WordPressContent
    {
        [JsonPropertyName("rendered")]
        public string Rendered { get; set; } = string.Empty;

        [JsonPropertyName("protected")]
        public bool Protected { get; set; }
    }

    public class WordPressExcerpt
    {
        [JsonPropertyName("rendered")]
        public string Rendered { get; set; } = string.Empty;

        [JsonPropertyName("protected")]
        public bool Protected { get; set; }
    }

    public class WordPressEmbedded
    {
        [JsonPropertyName("author")]
        public List<WordPressAuthor>? Authors { get; set; }

        [JsonPropertyName("wp:term")]
        public List<List<WordPressTerm>>? Terms { get; set; }

        [JsonPropertyName("wp:featuredmedia")]
        public List<WordPressMedia>? FeaturedMedia { get; set; }
    }

    public class WordPressAuthor
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("avatar_urls")]
        public Dictionary<string, string> AvatarUrls { get; set; } = new();
    }

    public class WordPressTerm
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("taxonomy")]
        public string Taxonomy { get; set; } = string.Empty;
    }

    public class WordPressMedia
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("source_url")]
        public string SourceUrl { get; set; } = string.Empty;

        [JsonPropertyName("alt_text")]
        public string AltText { get; set; } = string.Empty;
    }
}