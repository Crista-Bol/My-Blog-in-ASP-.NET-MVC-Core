using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [ForeignKey("ArticleId")]
        public int ArticleId { get; set; }

        public DateTime Date { get; set; }

        public string IP { get; set; }

        public int HeartVoteNumber { get; set; }
        public int BrokenHeartVoteNumber { get; set; }
    }
}
