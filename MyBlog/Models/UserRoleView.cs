using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyBlog.Models
{
    public class UserRoleView
    {
        public string RoleId { get; set; }
        
        public string RoleName { get; set; }

        public bool isChecked { get; set; }
    }
}
