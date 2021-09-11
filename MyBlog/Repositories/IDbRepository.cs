using MyBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Repositories
{
    public interface IDbRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync(bool? published);
        Task<IEnumerable<Comment>> GetCommentsAsync();

        Task<Article> GetArticleAsync(int id);

        Task UpdateCommentAsync(Comment comment);
        Task<Comment> GetCommentAsync(int id);

        Task CreateArticleAsync(Article article);
        Task CreateCommentAsync(Comment comment);

        Task UpdateArticleAsync(Article article);

        Task DeleteArticleAsync(Article article);
    }
}