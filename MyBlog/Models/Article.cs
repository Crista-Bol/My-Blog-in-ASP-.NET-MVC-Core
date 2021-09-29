﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Header { get; set; }

        [Required]
        public string Body { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="0:dd-MM-yyyy")]
        public DateTime Created_Date { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="0:dd-MM-yyyy")]
        public DateTime? Published_Date { get; set; }

        public string Image { get; set; }
        
        public ICollection<Comment> Comments { get; set; }

        public virtual ArticleCategory ArticleCategory { get; set; }
    }
}
