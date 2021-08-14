using System.ComponentModel.DataAnnotations;

namespace MyBlog.Controllers
{
    public class ProjectRole
    {
        [Key]
        public int Id { get; set; }
        public string RoleName { get; set; }
    }
}