using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServiceForAngular.ViewModels
{
    public class CheckItemViewModel
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public bool Checked { get; set; }
        public int? CheckListPostId { get; set; }
    }
}
