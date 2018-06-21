using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceForAngular.DAL.Models
{
    public partial class CheckListPost : BaseEntity
    {
        public CheckListPost()
        {
            CheckList = new HashSet<CheckItem>();
        }
        public string Title { get; set; }
        public ICollection<CheckItem> CheckList { get; set; }
        public string Color { get; set; }
        public int? UserId { get; set; }
        public User User { get; set; }
    }
}
