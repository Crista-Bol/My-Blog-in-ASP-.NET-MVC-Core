using MyBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Repositories
{
    public interface IDbRepository
    {
        Task<IEnumerable<Article>> GetArticlesAsync(bool? published);

        Task<Article> GetArticleAsync(int id);

        Task CreateArticleAsync(Article article);

        Task UpdateArticleAsync(Article article);

        Task DeleteArticleAsync(Article article);
    }
}