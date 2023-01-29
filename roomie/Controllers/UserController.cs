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
    public class UserController : Controller
    {
        projectEntities db = new projectEntities();

        public ActionResult About()
        {
            return RedirectToAction("About", "Home");
        }

        //------ OGŁOSZENIA WYŚWIETLANE NA GŁÓWNEJ STRONIE ------   

        // GET: User
        public ActionResult UserIndex(int? page)
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
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
        //---------------------

        //------DODAJ OGŁOSZENIE------


        public ActionResult CreateAd()
        {
            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                List<tbl_wojewodztwa_miasta> li = db.tbl_wojewodztwa_miasta.OrderBy(a => a.nazwa_miasta).ToList();
                ViewBag.categorylist = new SelectList(li, "id", "nazwa_miasta", "nazwa województwa");

                return View();
            }
        }

        [HttpPost]
        public ActionResult CreateAd(tbl_product pvm, IEnumerable<HttpPostedFileBase> imgfiles)
        {

            List<tbl_wojewodztwa_miasta> li = db.tbl_wojewodztwa_miasta.ToList();
            ViewBag.categorylist = new SelectList(li, "id", "nazwa_miasta", "nazwa województwa");

            if (imgfiles == null || !imgfiles.Any())
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                return View();
            }

            List<string> filePaths = new List<string>();
            foreach (var file in imgfiles)
            {
                string path = UploadFile(file);
                filePaths.Add(path);
            }

            if (filePaths.Any(x => x.Equals("-1")))
            {
                ViewBag.error = "Obraz nie może być załadowany";
            }
            else
            {
                try
                {
                    tbl_product p = new tbl_product
                    {
                        pro_name = pvm.pro_name,
                        pro_fk_address_city = pvm.pro_fk_address_city,
                        pro_address_street = pvm.pro_address_street,
                        pro_address_building_num = pvm.pro_address_building_num,
                        pro_address_floor = pvm.pro_address_floor,
                        pro_price = pvm.pro_price,
                        pro_description = pvm.pro_description,
                        pro_type = pvm.pro_type,
                        pro_room_size = pvm.pro_room_size,
                        pro_room_num = pvm.pro_room_num,
                        pro_roommates = pvm.pro_roommates,

                        pro_TV = pvm.pro_TV,
                        pro_internet = pvm.pro_internet,
                        pro_ac = pvm.pro_ac,
                        pro_parking = pvm.pro_parking,
                        pro_heating = pvm.pro_heating,
                        pro_elevator = pvm.pro_elevator,
                        pro_balcony = pvm.pro_balcony,
                        pro_handicapped = pvm.pro_handicapped,
                        pro_smoking = pvm.pro_smoking,
                        pro_pet = pvm.pro_pet,
                        tbl_pro_img = filePaths.Select(x => new tbl_pro_img() { pro_img_path = x }).ToList(),

                        pro_fk_user = Convert.ToInt32(Session["user_id"].ToString())
                    };

                    db.tbl_product.Add(p);

                    db.SaveChanges();

                    return RedirectToAction("UserIndex");
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                            Response.Write("Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                        }
                    }
                }
            }

            return View();
        }

        //---------------------

       //------WYŚWIETL OGŁOSZENIE ------

        public ActionResult ViewAd(int id, int? page)
        {
            if (Session["user_id"] == null)
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

        //---------------------

        //------USUŃ OGŁOSZENIE------

        public ActionResult DeleteAd(int? id)
        {
            Adviewmodel delete= new Adviewmodel();

            tbl_product p = db.tbl_product.Where(x => x.pro_id == id).SingleOrDefault();

            var images = db.tbl_pro_img.Where(x => x.pro_img_fk_pro_id == id).ToList();
            db.tbl_pro_img.RemoveRange(images);

            db.tbl_product.Remove(p);
            db.SaveChanges();

            return RedirectToAction("UserIndex", "User", new { area = "user" });
        }

        //---------------------------   

        //------ WYSZUKIWARKA ------


        public ActionResult Search(int? id, int? page, string search, int? price_from, int? price_to, bool TV = false, bool internet = false, bool ac = false, bool parking = false, bool elevator = false, bool balcony = false, bool handicapped = false, bool smoking = false, bool pet = false)
        {

            if (Session["user_id"] == null)
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

                if (TV != false)
                {
                    query = query.Where(x => x.pro_TV == true);
                }

                if (internet != false)
                {
                    query = query.Where(x => x.pro_internet == true);
                }

                if (ac != false)
                {
                    query = query.Where(x => x.pro_ac == true);
                }

                if (parking != false)
                {
                    query = query.Where(x => x.pro_parking == true);
                }

                if (elevator != false)
                {
                    query = query.Where(x => x.pro_elevator == true);
                }

                if (balcony != false)
                {
                    query = query.Where(x => x.pro_balcony == true);
                }

                if (handicapped != false)
                {
                    query = query.Where(x => x.pro_handicapped == true);
                }

                if (smoking != false)
                {
                    query = query.Where(x => x.pro_smoking == true);
                }

                if (pet != false)
                {
                    query = query.Where(x => x.pro_pet == true);
                }

                var list = query.OrderByDescending(x => x.pro_id).ToList();

                IPagedList<tbl_product> stu = list.ToPagedList(pageindex, pagesize);

                return View(stu);
            }
        }

        //----------------------

        //------EDYTUJ UŻYTKOWNIKA------

        [HttpGet]
        public ActionResult EditUser()
        {
            int userId = Convert.ToInt32(Session["user_id"]);
            if (userId == 0)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                projectEntities db = new projectEntities();
                tbl_user u = db.tbl_user.Where(x => x.user_id == userId).SingleOrDefault();

                db.Dispose();

                var viewModel = new UserProfileEditmodel
                {
                    user_name = u.user_name,
                    user_email = u.user_email,
                    user_contact = u.user_contact,
                    user_birthdate = u.user_birthdate,
                    user_smoke = u.user_smoke,
                    user_pet = u.user_pet,
                    user_description = u.user_description,

                };
                return View(viewModel);
            }
        }

        [HttpPost]
        public ActionResult SaveNewUser(UserProfileEditmodel model)
        {

            int userId = Convert.ToInt32(Session["user_id"]);
            tbl_user p = db.tbl_user.Where(x => x.user_id == userId).SingleOrDefault();

            p.user_name = model.user_name;

            if (!string.IsNullOrWhiteSpace(model.user_email))
            {
                p.user_email = model.user_email;
            }

            if (model.user_image != null)
            {
                var path = UploadFile(model.user_image);
                p.user_image = path;
            }

            if (!string.IsNullOrWhiteSpace(model.user_contact))
            {
                p.user_contact = model.user_contact;
            }

            if (!model.user_birthdate.Equals(p.user_birthdate))
            {
                p.user_birthdate = model.user_birthdate;
            }

            if (!model.user_smoke.Equals(p.user_smoke))
            {
               p.user_smoke = model.user_smoke;
            }

            if (!model.user_pet.Equals(p.user_pet))
            {
                p.user_pet = model.user_pet;
            }

            if (!string.IsNullOrWhiteSpace(model.user_description))
            {
                p.user_description = model.user_description;
            }


            db.SaveChanges();
            db.Dispose();
            return RedirectToAction("UserIndex", "User", new { area = "user" });
        }


        //---------------------

        //------Profil użytkownika------

        [HttpGet]
        public ActionResult Profile(int? page, int? id)
        { 

            if (Session["user_id"] == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {

                int pagesize = 9, pageindex = 1;
                pageindex = page.HasValue ? Convert.ToInt32(page) : 1;

                int userId = Convert.ToInt32(Session["user_id"]);

                var products = db.tbl_product
                    .Where(x => x.pro_fk_user == userId)
                    .OrderByDescending(x => x.pro_id)
                    .ToPagedList(pageindex, pagesize);

                var user = db.tbl_user.SingleOrDefault(x => x.user_id == userId);

                var viewModel = new UserProfileModel
                {
                    User = user,
                    Products = products
                };

                return View(viewModel);

            }
        }

        //--------------------

        //---WYLOGOWANIE-------

        public ActionResult Signout()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Index", "Home", new { area = "user" });
        }

        //---------------------

        //USUWANIE UŻYTKOWNIKA

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

            return RedirectToAction("Index", "Home", new { area = "user" });

        }

        //--------------------


        public string UploadFile(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {

                        path = Path.Combine(Server.MapPath("~/Content/user_upload"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Content/user_upload/" + random + Path.GetFileName(file.FileName);

                        //    ViewBag.Message = "File uploaded successfully";
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }
            else
            {
                path = "-1";
            }

            return path;
        }
    }
}
