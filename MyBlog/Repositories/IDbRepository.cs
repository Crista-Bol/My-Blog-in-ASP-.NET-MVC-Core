using MyBlog.Areas.Identity.Data;
using MyBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Repositories
{
    public interface IDbRepository
    {
        Task<int> GetArticlesCountAsync(bool? published);

        Task<IEnumerable<Article>> GetArticlesAsync(bool? published, int pageIndex, int pageSize);
        Task<IEnumerable<Comment>> GetCommentsAsync(int articleId);

        Task<Article> GetArticleAsync(int id);

        Task UpdateCommentAsync(Comment comment);
        Task<Comment> GetCommentAsync(int id);

        Task CreateArticleAsync(Article article);
        Task CreateCommentAsync(Comment comment);

        Task UpdateArticleAsync(Article article);

        Task DeleteArticleAsync(Article article);
        Task DeleteCommentAsync(Comment comment);

        Task<IEnumerable<MyBlogUser>> GetUsersAsync();

        /*Start-Art categories*/
        Task<IEnumerable<ArticleCategory>> getArtCategoriesAsync();
        Task<ArticleCategory> getArtCategoryAsync(int id);

        Task CreateArtCatAsync(ArticleCategory cat);
        Task UpdateArtCatAsync(ArticleCategory cat);
        Task DeleteArtCatAsync(ArticleCategory cat);
        /*End-Art Categories*/
    }
}