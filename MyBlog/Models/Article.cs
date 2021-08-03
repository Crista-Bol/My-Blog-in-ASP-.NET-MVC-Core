﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public DateTime Created_Date { get; set; }

        public bool published { get; set; }
    }
}
