using GreenPastures.BlogBurster.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace GreenPastures.BlogBurster.Services
{
    public interface IWordPressBlogService
    {
        Task<List<BlogPost>> GetBlogPostsAsync(int maxPosts = 50, int page = 1);
        Task<BlogPost?> GetBlogPostByIdAsync(int postId);
    }

    public class WordPressBlogService : IWordPressBlogService, IBlogService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseApiUrl;
        private readonly JsonSerializerOptions _jsonOptions;

        public WordPressBlogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            // WordPress.com site API endpoint - you can also use self-hosted WordPress REST API
            _baseApiUrl = "https://public-api.wordpress.com/wp/v2/sites/bloggingduringlunchcom.wordpress.com";
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        public async Task<List<BlogPost>> GetBlogPostsAsync(int maxPosts = 50, int page = 1)
        {
            try
            {
                // WordPress REST API supports up to 100 posts per request
                var perPage = Math.Min(maxPosts, 100);
                var url = $"{_baseApiUrl}/posts?per_page={perPage}&page={page}&_embed=true&status=publish&orderby=date&order=desc";

                Console.WriteLine($"Fetching WordPress posts from: {url}");

                var response = await _httpClient.GetStringAsync(url);
                var wordPressPosts = JsonSerializer.Deserialize<List<WordPressPost>>(response, _jsonOptions);

                if (wordPressPosts == null)
                {
                    Console.WriteLine("No posts returned from WordPress API");
                    return new List<BlogPost>();
                }

                Console.WriteLine($"Retrieved {wordPressPosts.Count} posts from WordPress API");

                var blogPosts = wordPressPosts.Select(ConvertToBlogPost).ToList();
                return blogPosts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching WordPress posts: {ex.Message}");
                return new List<BlogPost>();
            }
        }

        public async Task<BlogPost?> GetBlogPostByIdAsync(int postId)
        {
            try
            {
                var url = $"{_baseApiUrl}/posts/{postId}?_embed=true";
                var response = await _httpClient.GetStringAsync(url);
                var wordPressPost = JsonSerializer.Deserialize<WordPressPost>(response, _jsonOptions);

                return wordPressPost != null ? ConvertToBlogPost(wordPressPost) : null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching WordPress post {postId}: {ex.Message}");
                return null;
            }
        }

        private BlogPost ConvertToBlogPost(WordPressPost wpPost)
        {
            var blogPost = new BlogPost
            {
                Title = StripHtml(wpPost.Title.Rendered),
                Content = wpPost.Content.Rendered,
                Summary = GetSummary(wpPost),
                PublishDate = wpPost.Date,
                Url = wpPost.Link,
                Author = GetAuthor(wpPost),
                Categories = GetCategories(wpPost)
            };

            return blogPost;
        }

        private string GetSummary(WordPressPost wpPost)
        {
            // Try excerpt first, then content
            var summary = !string.IsNullOrEmpty(wpPost.Excerpt.Rendered) 
                ? wpPost.Excerpt.Rendered 
                : wpPost.Content.Rendered;

            return StripHtml(summary);
        }

        private string GetAuthor(WordPressPost wpPost)
        {
            // Try to get author from embedded data first
            var author = wpPost.Embedded?.Authors?.FirstOrDefault()?.Name;
            
            if (!string.IsNullOrEmpty(author))
                return author;

            return "Unknown Author";
        }

        private List<string> GetCategories(WordPressPost wpPost)
        {
            var categories = new List<string>();

            // Get categories and tags from embedded terms
            if (wpPost.Embedded?.Terms != null)
            {
                foreach (var termGroup in wpPost.Embedded.Terms)
                {
                    if (termGroup != null)
                    {
                        var categoryNames = termGroup
                            .Where(t => t.Taxonomy == "category" || t.Taxonomy == "post_tag")
                            .Select(t => t.Name)
                            .ToList();
                        
                        categories.AddRange(categoryNames);
                    }
                }
            }

            return categories.Distinct().ToList();
        }

        private string StripHtml(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            // Remove HTML tags
            var result = Regex.Replace(input, "<.*?>", string.Empty);
            
            // Decode HTML entities
            result = System.Net.WebUtility.HtmlDecode(result);
            
            // Clean up extra whitespace
            result = Regex.Replace(result, @"\s+", " ").Trim();
            
            return result;
        }
    }
}