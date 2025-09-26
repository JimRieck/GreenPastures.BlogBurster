using GreenPastures.BlogBurster.Models;

namespace GreenPastures.BlogBurster.Services
{
    /// <summary>
    /// Basic blog service interface for standard operations
    /// </summary>
    public interface IBlogService
    {
        Task<List<BlogPost>> GetBlogPostsAsync(int maxPosts = 50, int page = 1);
        Task<BlogPost?> GetBlogPostByIdAsync(int postId);
    }
}