using System.Collections.Generic;
using System.Web.Mvc;

namespace vts.Web.Models
{
    public class ViewModelBase
    {
        
        public static int ItemsPerPage { get; set; }
        //[UIHint("SelectItemsPerPage")]
        public static SelectList ItemsPerPageList
        {
            get
            {
                var selectList = new Dictionary<string, string>()
                                     {
                                         {"10", "10"},
                                         {"15", "15"},
                                         {"20", "20"},
                                         {"25", "25"},
                                         {"30", "30"}
                                     };

                return new SelectList(selectList, "Key", "Value", "----");
            }
        }
    }
}
