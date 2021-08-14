using Microsoft.AspNetCore.Mvc;
using MyBlog.Models;
using MyBlog.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Controllers
{
    
    public class ArticlesController : Controller
    {

        private readonly IDbRepository repository;
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public Article Article { get; set; }
       
        public ArticlesController(IDbRepository repository,ApplicationDbContext db)
        {
            _db = db;
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Upsert(int? Id) {

            Article = new Article();
            

            if (Id != null)
            {
                Article = (repository.GetArticleAsync((int)Id)).Result;

                if (Article == null)
                    NotFound();
            }

            return View(Article);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {
            if (ModelState.IsValid) {

                if (Article.Id != 0)
                {
                    await repository.UpdateArticleAsync(Article);
                   
                }
                else
                {
                   await repository.CreateArticleAsync(Article);
                   
                }
                return RedirectToAction("Index");
            }

            return View(Article);
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            return Json(new { data = await repository.GetArticlesAsync()});
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
