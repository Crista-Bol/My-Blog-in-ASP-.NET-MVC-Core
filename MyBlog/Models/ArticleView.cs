using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class ArticleView
    {
        public Article Article { get; set; }

        public Boolean Published { get; set; }

        [Display(Name = "Image")]
        public IFormFile Image { get; set; }

        [Display(Name = "Category")]
        public int CatId { get; set; }
    }
}
