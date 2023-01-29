using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace roomie.Models
{
    public class UserModel
    {

        //public IEnumerable<tbl_product> Products { get; set; }
        public string pro_name { get; set; }
        public int pro_id { get; set; }
        public string pro_address_street { get; set; }
        public int pro_price { get; set; }


        public int pro_fk_user { get; set; }
        public string user_name { get; set; }
        public string user_email { get; set; }
        public string user_image { get; set; }
        public string user_contact { get; set; }
        public Nullable<System.DateTime> user_birthdate { get; set; }
        public Nullable<bool> user_smoke { get; set; }
        public Nullable<bool> user_pet { get; set; }
        public string user_description { get; set; }


        public int pro_fk_address_city { get; set; }
        public string nazwa_miasta { get; set; }
        public string nazwa_województwa { get; set; }

        public string tbl_pro_img { get; set; }

        public IEnumerable<string> pro_img_path { get; set; }
        public IEnumerable<int> pro_img_fk_pro_id { get; set; }
        public IEnumerable<int> pro_img_id { get; set; }

        public List<UserModel> profile { get; set; }

    }
}