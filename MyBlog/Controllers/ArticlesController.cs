﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [BindProperty]
        public Comment newComment { get; set; }

        public int pageSize { get; }

        [BindProperty]
        public ArticleCategory artCategory { get; set; }

        //private int pageCount { get; set; }
        public ArticlesController(IDbRepository repository, IWebHostEnvironment hostEnvironment)
        {
            pageSize = 10;
            this.repository = repository;
            this.webHostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            
            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> PublishedArticles()
        {
            var totalArticles = await repository.GetArticlesCountAsync(true);
            ViewBag.totalPages = totalArticles / pageSize;
            return View();
        }

        public async Task<IActionResult> Detail(int Id) {
            
            ArticleView = new ArticleView();
            ArticleView.Article = await repository.GetArticleAsync((int)Id);
            DateTime publishedDate = (DateTime)ArticleView.Article.Published_Date;

            if (publishedDate != null)
            {
                DateTime now = DateTime.Now;
                DateTime aMonthAgo = now.AddMonths(-1);

                if (publishedDate > aMonthAgo)
                {
                    long dayDiff = (now - (DateTime)ArticleView.Article.Published_Date).Days;
                    ViewData["publishedDate"] = ((dayDiff==0)?"today":(dayDiff == 1? "a day ago":dayDiff+" days ago"));
                }
                else
                {
                    ViewData["publishedDate"] = publishedDate.Day+"/"+ publishedDate.Month+"/"+publishedDate.Year;
                }
            }
          
            return View(ArticleView);
        }

        public async Task<IActionResult> Upsert(int? Id) {
            IEnumerable<ArticleCategory> artCats= await repository.getArtCategoriesAsync();
            ViewBag.ArtCats=new SelectList(artCats, "Id", "Name");
            
            ArticleView = new ArticleView();
            
            if (Id != null)
            {
                ArticleView.Article = (repository.GetArticleAsync((int)Id)).Result;

                if (ArticleView.Article == null)
                    NotFound();

                ArticleView.Published = (ArticleView.Article.Published_Date != null ? true : false);
                ArticleView.CatId = (ArticleView.Article.ArticleCategory != null)? ArticleView.Article.ArticleCategory.Id:0;
            }
            else {
                ArticleView.Article = new Article();
            }

            return View(ArticleView);
        }

        [HttpGet]
        public async Task<IActionResult> commentList(int articleId)
        {
            return  Json(new { data = await repository.GetCommentsAsync(articleId) });
        }

        [HttpPost]
        public async Task<IActionResult> SendComment(string commenter,string detail,int articleId) {

            newComment = new Comment();
            newComment.Commenter = commenter;
            newComment.Detail = detail;
            
            /*if (articleId != null)
                newComment.Article=await repository.GetArticleAsync(articleId);*/
            
            newComment.ArticleId = articleId;
            newComment.Date = DateTime.Now;
            await repository.CreateCommentAsync(newComment);
            
            return Json(new { success=true});
        }

        [HttpPut]
        public async Task<IActionResult> GiveHeart(int Id) {

            Comment comment=await repository.GetCommentAsync(Id);
            comment.HeartVoteNumber++;
            await repository.UpdateCommentAsync(comment);
            return Json(new { success=true });
        }

        [HttpPut]
        public async Task<IActionResult> GiveBrokenHeart(int Id)
        {

            Comment comment = await repository.GetCommentAsync(Id);
            comment.BrokenHeartVoteNumber++;
            await repository.UpdateCommentAsync(comment);
            return Json(new { success = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert()
        {
            if (ArticleView.CatId != 0){
                    ArticleCategory cat = await repository.getArtCategoryAsync(ArticleView.CatId);
                    ArticleView.Article.ArticleCategory = cat;
             }
            

            if (!ArticleView.Article.Published_Date.HasValue && ArticleView.Published)
                ArticleView.Article.Published_Date = DateTimeOffset.UtcNow.DateTime;
            else
            {
                if (ArticleView.Article.Published_Date.HasValue && !ArticleView.Published)
                    ArticleView.Article.Published_Date = null;
            }

            //For image name creation
            if (ArticleView.Image != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "img");
                string filePath;

                if (ArticleView.Article.Image != null)
                {

                    filePath = Path.Combine(uploadsFolder, ArticleView.Article.Image);

                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);

                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + ArticleView.Image.FileName;
                ArticleView.Article.Image = uniqueFileName;

                filePath = Path.Combine(uploadsFolder, ArticleView.Article.Image);
                var fileStream = new FileStream(filePath, FileMode.Create);
                ArticleView.Image.CopyTo(fileStream);
            }

                if (ArticleView.Article.Id != 0)
                {
                    await repository.UpdateArticleAsync(ArticleView.Article);   
                }
                else
                {
                    ArticleView.Article.Created_Date=DateTime.Now;
                   await repository.CreateArticleAsync(ArticleView.Article);
                }
                return RedirectToAction("Index");
          
            return View(ArticleView);
        }

        [HttpGet]
        public async Task<IActionResult> getAll()
        {
       
            return Json(new { data = await repository.GetArticlesAsync(null,0,0)});
        }

        [HttpGet]
        public async Task<IActionResult> moreArticles(int pageCount) {
            
            return Json(new { data= await repository.GetArticlesAsync(true, pageCount,pageSize) });
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteComment(int id) {

            Comment comment = await repository.GetCommentAsync(id);
            
            if (comment == null)
            {
                return Json(new { success = false  })  ;
            }
            int articleId = comment.ArticleId;
            await repository.DeleteCommentAsync(comment);

            return Json(new { success=true });
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int Id) {

            Article article=await repository.GetArticleAsync(Id);

            if (article == null)
            {
                return Json(new { success=false, message="Not successful"});
            }

            await repository.DeleteArticleAsync(article);

            return Json(new { success=true, message="Successfull!!"});

        }


        public async Task<IActionResult> ArtCategories() {
            var artCategories = await repository.getArtCategoriesAsync();
            return View(artCategories);
        }

        public async Task<IActionResult> UpsertArtCat(int id) {

            if (id != 0)
            {
                artCategory = await repository.getArtCategoryAsync(id);

                if (artCategory == null)
                    NotFound();
            }
            else
            {
                artCategory = new ArticleCategory();
            }               

            return View(artCategory);
        }

        [HttpPost]
        public async Task<IActionResult> UpsertArtCat() {

            if (artCategory.Id == 0)
               await repository.CreateArtCatAsync(artCategory);
            else
                await repository.UpdateArtCatAsync(artCategory);

            return RedirectToAction("ArtCategories");
        } 
        [HttpPost]
        public async Task<IActionResult> DeleteArtCat(int Id) {

            ArticleCategory cat=await repository.getArtCategoryAsync(Id);

            if (cat == null)
                return NotFound();

            await repository.DeleteArtCatAsync(cat);

            return RedirectToAction("ArtCategories");
        }
    }
}
