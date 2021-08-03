using Microsoft.EntityFrameworkCore;
using MyBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Repositories
{
    public class SQLDbRepository : IDbRepository
    {
        private readonly ApplicationDbContext _db;

        public SQLDbRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Article>> GetArticles()
        {
            return await _db.Articles.ToListAsync();
        }

        public async Task<Article> GetArticle(int id) {

            return await _db.Articles.FirstOrDefaultAsync(article=>article.Id==id);
        }

        public async Task DeleteArticle(Article article)
        {

             _db.Articles.Remove(article);
            await _db.SaveChangesAsync();
        }
    }
}
