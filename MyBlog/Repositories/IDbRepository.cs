using MyBlog.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyBlog.Repositories
{
    public interface IDbRepository
    {
        Task<IEnumerable<Article>> GetArticles();

        Task<Article> GetArticle(int id);

        Task DeleteArticle(Article article);
    }
}