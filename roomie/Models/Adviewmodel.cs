using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace roomie.Models
{
    public class Adviewmodel
    {

        public int pro_id { get; set; }
        public string pro_name { get; set; }
        public string pro_image { get; set; }
        public string pro_address_street { get; set; }
        public int? pro_address_building_num { get; set; }
        public int? pro_address_floor { get; set; }
        public int? pro_price { get; set; }
        public string pro_description { get; set; }
        public string pro_type { get; set; }
        public int? pro_room_size { get; set; }
        public int? pro_room_num { get; set; }
        public int? pro_bathroom_num { get; set; }
        public int? pro_roommates { get; set; }
        public string pro_room_gender { get; set; }
        public bool pro_TV { get; set; }
        public bool pro_internet { get; set; }
        public bool pro_ac { get; set; }
        public bool pro_parking { get; set; }
        public string pro_heating { get; set; }
        public bool pro_elevator { get; set; }
        public bool pro_balcony { get; set; }
        public bool pro_handicapped { get; set; }
        public bool pro_smoking { get; set; }
        public bool pro_pet { get; set; }
        public int pro_prefe_gender { get; set; }



        public int pro_fk_user { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_image { get; set; }
        public string user_contact { get; set; }
        public string user_description { get; set; }
        public bool user_smoke { get; set; }
        public bool user_pet { get; set; }



        public int pro_fk_address_city { get; set; }
        public string nazwa_miasta { get; set; }
        public string nazwa_województwa { get; set; }

        public string tbl_pro_img { get; set; }

        public IEnumerable<string> pro_img_path { get; set; }
        public IEnumerable<int> pro_img_fk_pro_id { get; set; }
        public IEnumerable<int> pro_img_id { get; set; }
    }
}