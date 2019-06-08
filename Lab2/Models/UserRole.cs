using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab2.Models
{
    public class UserRole
    {
        public  int ID { get; set; }
        public  string Name { get; set; }
        public  string Description { get; set; }
        public IEnumerable<UserUserRol> UserUserRols { get; set; }
    }
}
