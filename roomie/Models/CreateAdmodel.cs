using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace roomie.Models
{
    public class CreateAdmodel
    {

        public int pro_id { get; set; }

        [Required(ErrorMessage = "Nie wprowadziłeś tytuł swojego ogłoszenia")]
        [StringLength(30, MinimumLength = 3)]
        public string pro_name { get; set; }


        public HttpPostedFileBase[] pro_image { get; set; }

        [Required(ErrorMessage = "Nie wybrałeś miasta")]
        public int pro_fk_address_city { get; set; }

        [Required(ErrorMessage = "Nie wpisałeś ulicy")]
        public string pro_address_street { get; set; }

        public Nullable<int> pro_address_building_num { get; set; }
        public Nullable<int> pro_address_floor { get; set; }

        [Required(ErrorMessage = "Nie podałeś ceny wynajmu")]
        public int pro_price { get; set; }

        [Required(ErrorMessage = "Nie opisałeś swojego ogłoszenia")]
        [StringLength(300, MinimumLength = 3)]
        public string pro_description { get; set; }

        public string pro_type { get; set; }

        public Nullable<int> pro_room_size { get; set; }
        public Nullable<int> pro_room_num { get; set; }
        public Nullable<int> pro_bathroom_num { get; set; }
        public Nullable<int> pro_roommates { get; set; }

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
        public int pro_fk_user { get; set; }

        public virtual tbl_user tbl_user { get; set; }
        public virtual tbl_wojewodztwa_miasta tbl_wojewodztwa_miasta { get; set; }
        public virtual tbl_pro_img tbl_pro_img { get; set; }

    }
}