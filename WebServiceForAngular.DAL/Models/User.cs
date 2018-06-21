using System;
using System.Collections.Generic;

namespace WebServiceForAngular.DAL.Models
{
    public partial class User : BaseEntity
    {
        public User()
        {
            Post = new HashSet<Post>();
        }
        
        public string Name { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public string Phone { get; set; }
        public string Username { get; set; }
        public ICollection<Post> Post { get; set; }
        public string IdentityId { get; set; }
        public AppUser Identity { get; set; }
    }
}
