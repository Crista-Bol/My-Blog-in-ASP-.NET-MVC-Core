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

        public async Task<IEnumerable<Article>> GetArticlesAsync(bool? published)
        {
           return (published == null)? await _db.Articles.OrderByDescending(a => a.Created_Date).ToListAsync():
            await _db.Articles.Where(a => a.published == published).OrderByDescending(a => a.Created_Date).ToListAsync();
        }

        public async Task<Article> GetArticleAsync(int id) {

            return await _db.Articles.FirstOrDefaultAsync(article=>article.Id==id);
        }

        public async Task CreateArticleAsync(Article article)
        {
           await _db.Articles.AddAsync(article);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _db.Articles.Update(article);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(Article article)
        {

             _db.Articles.Remove(article);
            await _db.SaveChangesAsync();
        }
    }
}
