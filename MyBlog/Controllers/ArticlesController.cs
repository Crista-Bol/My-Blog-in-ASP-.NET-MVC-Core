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

       
        public ArticlesController(IDbRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
            return Json(new { data = await repository.GetArticles()});
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id) {

            Article article=await repository.GetArticle(id);

            if (article == null)
            {
                return Json(new { success=false, message="Not successful"});
            }

            await repository.DeleteArticle(article);

            return Json(new { success=true, message="Successfull!!"});

        }
    }
}
