using System;
using System.Collections.Generic;
using System.Text;

namespace WebServiceForAngular.DAL.Models
{
    public partial class CheckItem : BaseEntity
    {
        public string Body { get; set; }
        public bool Checked { get; set; }
        public int? CheckListPostId { get; set; }
        public CheckListPost CheckListPost { get; set; }
    }
}
