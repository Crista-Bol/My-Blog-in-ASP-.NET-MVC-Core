using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using MyBlog.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Helpers;

namespace MyBlog.Controllers
{
    
    public class ArticlesController : Controller
    {

        private readonly IDbRepository repository;
        
        [BindProperty]
        public ArticleView ArticleView { get; set; }
        
        
        private readonly IWebHostEnvironment webHostEnvironment;

        [BindProperty]
        public IFormFile ArticleImage { get; set; }


        public ArticlesController(IDbRepository repository, IWebHostEnvironment hostEnvironment)
        {
            this.repository = repository;
            this.webHostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> PublishedArticles()
        {
            var articles=await repository.GetArticlesAsync(published: true);
            return View(articles);
        }

        public IActionResult Upsert(int? Id) {

            ArticleView = new ArticleView();

            if (Id != null)
            {
                ArticleView.Article = (repository.GetArticleAsync((int)Id)).Result;

                if (ArticleView.Article == null)
                    NotFound();

                ViewData["isPublished"] = (ArticleView.Article.Published_Date.HasValue) ? true : false;
            }

            return View(ArticleView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Boolean published)
        {
            if (ModelState.IsValid) {

                if (!ArticleView.Article.Published_Date.HasValue && published)
                    ArticleView.Article.Published_Date = DateTimeOffset.UtcNow.DateTime;
                else
                {
                    if (ArticleView.Article.Published_Date.HasValue && !published)
                        ArticleView.Article.Published_Date = null;
                }

                //For image name creation
                if (ArticleView.Image != null)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + ArticleView.Image.FileName;
                    ArticleView.Article.Image = uniqueFileName;

                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                    string filePath = Path.Combine(uploadsFolder, ArticleView.Article.Image);
                    var fileStream = new FileStream(filePath, FileMode.Create);
                        
                        ArticleView.Image.CopyTo(fileStream);
                    
                    
                }

                if (ArticleView.Article.Id != 0)
                {
                    await repository.UpdateArticleAsync(ArticleView.Article);   
                }
                else
                {
                   await repository.CreateArticleAsync(ArticleView.Article);
                }
                return RedirectToAction("Index");
            }

            return View(ArticleView);
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            return Json(new { data = await repository.GetArticlesAsync(null)});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {

            Article article=await repository.GetArticleAsync(id);

            if (article == null)
            {
                return Json(new { success=false, message="Not successful"});
            }

            await repository.DeleteArticleAsync(article);

            return Json(new { success=true, message="Successfull!!"});

        }
    }
}
