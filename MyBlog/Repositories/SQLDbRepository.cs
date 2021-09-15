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

            return (published != null && published == true) ?
                await _db.Articles.Where(a => a.Published_Date != null).OrderByDescending(a => a.Created_Date).ToListAsync() :
                await _db.Articles.OrderByDescending(a => a.Created_Date).ToListAsync();
        }
        public async Task<IEnumerable<Comment>> GetCommentsAsync(int articleId)
        {
            return await _db.Comments.Where(cm=>cm.ArticleId== articleId).OrderBy(cm=>cm.Date).ToListAsync();
        }

        public async Task<Article> GetArticleAsync(int id) {

            return await _db.Articles.FirstOrDefaultAsync(article=>article.Id==id);
        }


        public async Task<Comment> GetCommentAsync(int id)
        {

            return await _db.Comments.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task CreateArticleAsync(Article article)
        {
           await _db.Articles.AddAsync(article);
            await _db.SaveChangesAsync();
        }
        public async Task CreateCommentAsync(Comment comment)
        {
            await _db.Comments.AddAsync(comment);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateArticleAsync(Article article)
        {
            _db.Articles.Update(article);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            _db.Comments.Update(comment);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteArticleAsync(Article article)
        {

             _db.Articles.Remove(article);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteCommentAsync(Comment comment)
        {

            _db.Comments.Remove(comment);
            await _db.SaveChangesAsync();
        }
    }
}
