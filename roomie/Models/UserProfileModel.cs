using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace roomie.Models
{
    public class UserProfileModel
    {
        public tbl_user User { get; set; }

        public PagedList.IPagedList<tbl_product> Products { get; set; }

    }
}