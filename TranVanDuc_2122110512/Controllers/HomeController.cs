    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using TranVanDuc_2122110512.Context;

    namespace TranVanDuc_2122110512.Controllers
    {
        public class HomeController : Controller
        {
            private OnlineShopEntities4 db = new OnlineShopEntities4();

            // GET: product
            public ActionResult Index()
            {
                // Lấy danh mục từ cơ sở dữ liệu
                var categories = db.Categories.ToList();
                ViewBag.Categories = categories; // Truyền danh mục vào ViewBag

                // Lấy sản phẩm từ cơ sở dữ liệu
                var products = db.Products.ToList();
                return View(products); // Trả về view với danh sách sản phẩm
            }


            public ActionResult About()
            {
                ViewBag.Message = "Your application description page.";
                return View();
            }

            public ActionResult Contact()
            {
                ViewBag.Message = "Your contact page.";
                return View();
            }
        }
    }
