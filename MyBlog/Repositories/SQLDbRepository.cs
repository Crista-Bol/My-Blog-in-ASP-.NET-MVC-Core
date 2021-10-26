using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyBlog.Areas.Identity.Data;
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

        public async Task<int> GetArticlesCountAsync(bool? published)
        {
            return (published != null && published == true) ?
                    await _db.Articles.Where(a => a.Published_Date != null).CountAsync() :
                    await _db.Articles.CountAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesAsync(bool? published, int pageIndex, int pageSize)
        {
            return (pageSize!=0)?((published != null && published == true) ?
                    await _db.Articles.Where(a => a.Published_Date != null)
                                      .OrderByDescending(a => a.Created_Date)
                                      .Skip((pageIndex-1)*pageSize)
                                      .Take(pageSize)
                                      .ToListAsync():
                    await _db.Articles.OrderByDescending(a => a.Created_Date)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .ToListAsync())
             : (await _db.Articles.OrderByDescending(a => a.Created_Date)
                                 .ToListAsync());
        }

        public async Task<IEnumerable<PubArtView>> GetPubArtsViewAsync(bool? published, int pageIndex, int pageSize,string searchVal)
        {
            return (pageSize != 0) ? ((published != null && published == true) ?
                    await _db.Articles.Where(a => a.Published_Date != null && (searchVal!=null && (a.Header.Contains(searchVal) || a.Body.Contains(searchVal))))
                                      .OrderByDescending(a => a.Created_Date)
                                      .Include(a=>a.Comments)
                                      .Skip((pageIndex - 1) * pageSize)
                                      .Take(pageSize)
                                      .Select(a => new PubArtView{ Id=a.Id, Image=a.Image, Header=a.Header, PubDate=a.Published_Date, Body=a.Body, ComCount=a.Comments.Count})
                                      .ToListAsync():
                    await _db.Articles.Where(a=> searchVal != null && (a.Header.Contains(searchVal) || a.Body.Contains(searchVal)))
                                  .OrderByDescending(a => a.Created_Date)
                                  .Include(a => a.Comments)
                                  .Skip((pageIndex - 1) * pageSize)
                                  .Take(pageSize)
                                  .Select(a => new PubArtView { Id = a.Id, Image = a.Image, Header = a.Header, PubDate = a.Published_Date, Body = a.Body, ComCount = a.Comments.Count })
                                  .ToListAsync())
             : (await _db.Articles.Where(a => searchVal != null && (a.Header.Contains(searchVal) || a.Body.Contains(searchVal)))
                                  .OrderByDescending(a => a.Created_Date)
                                  .Include(a => a.Comments)
                                  .Select(a => new PubArtView { Id = a.Id, Image = a.Image, Header = a.Header, PubDate = a.Published_Date, Body = a.Body, ComCount = a.Comments.Count })
                                  .ToListAsync());
        }
        public async Task<IEnumerable<Comment>> GetCommentsAsync(int articleId)
        {
            return await _db.Comments.Where(cm=>cm.ArticleId== articleId).OrderBy(cm=>cm.Date).ToListAsync();
        }

        public async Task<IEnumerable<MyBlogUser>> GetUsersAsync()
        {
            return await _db.users.OrderBy(u => u.FirstName).ToListAsync();
        }

        public async Task<Article> GetArticleAsync(int id) {

            return await _db.Articles.Include(a=>a.ArticleCategory).FirstOrDefaultAsync(article=>article.Id==id);
        }

        public async Task<IEnumerable<Article>> GetArtsWithinTimeByCatId(int Id, int catId, DateTime dt) {
            DateTime now = DateTime.Now;
            return _db.Articles
                .Include(a => a.ArticleCategory)
                .Where(a => a.Id!=Id && a.ArticleCategory.Id == catId && a.Published_Date <= now && a.Published_Date >= dt)
                .ToList();
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

        /*Start-Article Categories*/
        public async Task<IEnumerable<ArticleCategory>> getArtCategoriesAsync() {
            return await _db.ArticleCategories.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<ArticleCategory> getArtCategoryAsync(int id)
        {
            return await _db.ArticleCategories.FirstOrDefaultAsync(c => c.Id == id);
        }

       
        public async Task CreateArtCatAsync(ArticleCategory cat)
        {
            await _db.ArticleCategories.AddAsync(cat);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateArtCatAsync(ArticleCategory cat)
        {
             _db.ArticleCategories.Update(cat);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteArtCatAsync(ArticleCategory cat)
        {
            _db.ArticleCategories.Remove(cat);
            await _db.SaveChangesAsync();
        }
        /*End-Article Categories*/

    }
}
