using System;
using System.Collections.Generic;

namespace WebServiceForAngular.DAL.Models
{
    public partial class Post : BaseEntity
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public string Color { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
