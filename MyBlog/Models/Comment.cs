using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Commenter { get; set; }

        [Required]
        public string Detail { get; set; }

        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public DateTime Date { get; set; }

        public string IP { get; set; }
    }
}
