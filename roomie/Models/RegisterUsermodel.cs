using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace roomie.Models
{
    public class RegisterUsermodel
    {
        public int user_id { get; set; }

        [Required(ErrorMessage = "Proszę wprowadź swoje imię i nazwisko")]
        [StringLength(100, MinimumLength = 3)]
        public string user_name { get; set; }

        [Required(ErrorMessage = "Proszę wprowadź poprawny adres e-mail")]
        [MaxLength(50)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "E-mail został wprowadzony niepoprawnie")]
        [DataType(DataType.EmailAddress)]
        public string user_email { get; set; }

        [Required(ErrorMessage = "Proszę wprowadź hasło")]
        public string user_password { get; set; }

    }
}