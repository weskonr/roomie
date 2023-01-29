using roomie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scrypt;

namespace roomie.Controllers
{
    public class HomeController : Controller
    {

        projectEntities db = new projectEntities();

        public ActionResult Index()
        {
            return View();
        }

        //O PROJEKCIE
        public ActionResult About()
        {
            return View("About");
        }

        //------REJESTRACJA------
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(RegisterUsermodel model)
        {
            // czy użytkownik jest już zarejestrowany
            tbl_user user = db.tbl_user.Where(x => x.user_email == model.user_email).SingleOrDefault();
            if (user != null)
            {
                Response.Write("<script>alert('Istnieje już użytkownik z tym adresem e-mail'); </script>");
                return View("Index");
            }
            else
            {

                try
                {
                    ScryptEncoder encoder = new ScryptEncoder();

                    tbl_user u = new tbl_user();
                    u.user_name = model.user_name;
                    u.user_email = model.user_email;

                    u.user_password = encoder.Encode(model.user_password);
                    /*u.user_password = model.user_password;*/


                    db.tbl_user.Add(u);

                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            return View("Index");
                        }
                    }
                }

                Response.Write("<script>alert('Rejestracja przebiegła poprawnie, możesz się zalogować na swoje konto'); </script>");
                return View("Index");
            }
        
        }

        //---------------------

        //------LOGOWANIE------

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(RegisterUsermodel model)
        {
            ScryptEncoder encoder = new ScryptEncoder();

            tbl_user user = db.tbl_user.Where(x => x.user_email == model.user_email).SingleOrDefault();
            if (user != null)
            {
                bool isValidCustomer = encoder.Compare(model.user_password, user.user_password);
                if (isValidCustomer)
                {
                    Session["user_id"] = user.user_id.ToString();
                    return RedirectToAction("UserIndex", "User", new { area = "User" });

                } 
                else
                {
                    Response.Write("<script>alert('Niepoprawna nazwa użytkownika bądź hasło'); </script>");
                    return View("Index");
                }

            }
            else
            {
                Response.Write("<script>alert('Niepoprawna nazwa użytkownika bądź hasło'); </script>");
                return View("Index");
            }

        }

        //---------------------

        public ActionResult Contact()
        {
           return View();
        }


    }
}