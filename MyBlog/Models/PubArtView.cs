using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class PubArtView
    {
        public int Id { get; set; }
        public string Image { get; set; }

        public string Header { get; set; }
        public DateTime? PubDate { get; set; }

        public int ComCount { get; set; }

        public string Body { get; set; }

    }
}
