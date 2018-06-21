using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceForAngular.DAL.Models
{
    public class UserPost : BaseEntity
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
