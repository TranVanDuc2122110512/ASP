using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TranVanDuc_2122110512.Context;


namespace TranVanDuc_2122110512.Areas.Admin.Controllers
{
    
    public class HomeAdminController : Controller
    {
        private OnlineShopEntities4 db = new OnlineShopEntities4();
        // GET: Admin/HomeAdmin
        public ActionResult Index()
        {
            var totalProducts = db.Products.Count();
            var totalCategories = db.Categories.Count();
            var totalOrders = db.Orders.Count(); // Thêm thống kê đơn hàng nếu cần
            var totalUsers = db.Registers.Count(); // Thêm thống kê người dùng nếu cần
            var totalBrand = db.Brands.Count();
            ViewBag.TotalProducts = totalProducts;
            ViewBag.TotalBrand = totalBrand;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.TotalOrders = totalOrders;
            ViewBag.TotalUsers = totalUsers;
            return View();
            
        }
    }
}