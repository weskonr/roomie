using Microsoft.AspNetCore.Identity;
using PagedList;
using roomie.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace roomie.Controllers
{
    public class AdminController : Controller
    {

        projectEntities db = new projectEntities();

        public ActionResult AdminIndex()
        {
            return View();
        }

        // Logowamie

        public ActionResult Adminlogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Adminlogin(tbl_admin avm)
        {
            tbl_admin admin = db.tbl_admin.Where(x => x.admin_username == avm.admin_username && x.admin_password == avm.admin_password).SingleOrDefault();
            if (admin != null)
            {
                Session["admin_id"] = admin.admin_id.ToString();
                return RedirectToAction("AdminPage", "Admin", new { area = "User" });
            }
            else
            {
                ViewBag.error = "Niepoprawna nazwa użytkownika bądź hasło";
            }

            return View();
        }

        //------------------------------------

        public ActionResult AdminPage(int? page)
        {
            if (Session["admin_id"] == null)
            {
                return RedirectToAction("AdminPage", "Admin", new { area = "User" });
            }
            else
            {
                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
                var list = db.tbl_product.OrderByDescending(x => x.pro_id).ToList(); // <- ustawia najnowsze ogłoszenia jako pierwsze
                IPagedList<tbl_product> stu = list.ToPagedList(pageindex, pagesize);

                return View(stu);
            }

            return View();
        }

        //------------------------------------
        public ActionResult AdminViewAd(int id, int? page)
        {
            if (Session["admin_id"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                Adviewmodel adViewModel = new Adviewmodel();

                tbl_product product = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();
                adViewModel.pro_id = product.pro_id;
                adViewModel.pro_name = product.pro_name;
                adViewModel.pro_address_street = product.pro_address_street;
                adViewModel.pro_address_building_num = (int?)product.pro_address_building_num;
                adViewModel.pro_address_floor = (int?)product.pro_address_floor;
                adViewModel.pro_price = (int)product.pro_price;
                adViewModel.pro_description = product.pro_description;
                adViewModel.pro_type = product.pro_type;
                adViewModel.pro_room_size = (int?)product.pro_room_size;
                adViewModel.pro_room_num = (int?)product.pro_room_num;
                adViewModel.pro_roommates = (int?)product.pro_roommates;
                adViewModel.pro_TV = (bool)product.pro_TV;
                adViewModel.pro_internet = (bool)product.pro_internet;
                adViewModel.pro_ac = (bool)product.pro_ac;
                adViewModel.pro_parking = (bool)product.pro_parking;
                adViewModel.pro_heating = product.pro_heating;
                adViewModel.pro_elevator = (bool)product.pro_elevator;
                adViewModel.pro_balcony = (bool)product.pro_balcony;
                adViewModel.pro_handicapped = (bool)product.pro_handicapped;
                adViewModel.pro_smoking = (bool)product.pro_smoking;
                adViewModel.pro_pet = (bool)product.pro_pet;

                tbl_user user = db.tbl_user.Where(x => x.user_id == product.pro_fk_user).SingleOrDefault();
                adViewModel.user_name = user.user_name;
                adViewModel.user_image = user.user_image;
                adViewModel.user_contact = user.user_contact;
                adViewModel.user_description = user.user_description;
                adViewModel.pro_fk_user = user.user_id;


                tbl_wojewodztwa_miasta wm = db.tbl_wojewodztwa_miasta.Where(x => x.id == product.pro_fk_address_city).SingleOrDefault();
                adViewModel.nazwa_miasta = wm.nazwa_miasta;
                adViewModel.nazwa_województwa = wm.nazwa_województwa;


                var imagePaths = db.tbl_pro_img.Where(x => x.pro_img_fk_pro_id == product.pro_id).Select(x => x.pro_img_path).ToList();
                adViewModel.pro_img_path = imagePaths;

                return View(adViewModel);
            }
        }

        //------USUŃ OGŁOSZENIE------

        public ActionResult DeleteAd(int? id)
        {
            Adviewmodel delete = new Adviewmodel();

            tbl_product p = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();

            var images = db.tbl_pro_img.Where(x => x.pro_img_fk_pro_id == id).ToList();
            db.tbl_pro_img.RemoveRange(images);

            db.tbl_product.Remove(p);
            db.SaveChanges();

            return RedirectToAction("UserIndex", "User", new { area = "user" });
        }

        //---------------------------


        //------ WYSZUKIWARKA ------


        public ActionResult Search(int? id, int? page, string search, int? price_from, int? price_to, int? adId, int? userId)
        {

            if (Session["admin_id"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

                var query = from m in db.tbl_product select m;

                if (search != null)
                {
                    query = query.Where(x => x.pro_description.Contains(search));
                }

                if (price_from != null)
                {
                    int from = Convert.ToInt32(price_from);
                    query = query.Where(x => x.pro_price >= from);
                }

                if (price_to != null)
                {
                    int to = Convert.ToInt32(price_to);
                    query = query.Where(x => x.pro_price <= to);
                }

                if (adId != null)
                {
                    int ad_id = Convert.ToInt32(adId);
                    query = query.Where(x => x.pro_id == ad_id);
                }

                if (userId != null)
                {
                    int user_id = Convert.ToInt32(userId);
                    query = query.Where(x => x.tbl_user.user_id == user_id);
                }



                var list = query.OrderByDescending(x => x.pro_id).ToList();

                IPagedList<tbl_product> stu = list.ToPagedList(pageindex, pagesize);

                return View(stu);
            }
        }

        //----------------------

        //-------- UŻYTKOWNICY ---------

        public ActionResult AdminUsers (int? id, int? page, string search)
        {

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.tbl_user.OrderByDescending(x => x.user_id).ToList();
            IPagedList<tbl_user> stu = list.ToPagedList(pageindex, pagesize);
            return View(stu);
        }

        //-----------------------------

        //------USUŃ UŻYTKOWNIKA------

        public ActionResult DeleteUser(int? id)
        {
            var user = db.tbl_user.SingleOrDefault(x => x.user_id == id);

            var products = db.tbl_product.Where(x => x.pro_fk_user == id).ToList();

            var productIds = products.Select(x => x.pro_id).ToList();

            var images = db.tbl_pro_img.Where(x => productIds.Contains(x.pro_img_fk_pro_id)).ToList();

            db.tbl_pro_img.RemoveRange(images);
            db.tbl_product.RemoveRange(products);
            db.tbl_user.Remove(user);

            db.SaveChanges();

            return RedirectToAction("AdminUsers", "Admin", new { area = "admin" });

        }

        //---------------------------

        //---WYLOGOWANIE-------

        public ActionResult Signout()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Index", "Home", new { area = "user" });
        }

        //---------------------
    }
}