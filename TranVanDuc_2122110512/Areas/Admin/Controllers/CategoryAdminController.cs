using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TranVanDuc_2122110512.Context;

namespace TranVanDuc_2122110512.Areas.Admin.Controllers
{
    public class CategoryAdminController : Controller
    {
        private OnlineShopEntities4 db = new OnlineShopEntities4();

        // GET: Admin/CategoryAdmin
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            // Cập nhật số trang khi tìm kiếm
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            // Lấy danh sách danh mục theo từ khóa tìm kiếm hoặc tất cả danh mục nếu không tìm kiếm
            var listCategory = db.Categories
                .Where(n => string.IsNullOrEmpty(SearchString) || n.Name.Contains(SearchString))
                .OrderByDescending(n => n.Id)
                .ToList();

            ViewBag.CurrentFilter = SearchString;

            // Số lượng item của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(listCategory.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            // Cung cấp dữ liệu cho dropdown list với lựa chọn "None" để chọn danh mục cha
            var categories = db.Categories.ToList();
            categories.Insert(0, new Category { Id = 0, Name = "None" }); // Thêm lựa chọn "None"

            ViewBag.Parent_Id = new SelectList(categories, "Id", "Name"); // Chỉnh sửa đây
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Sửa lỗi cú pháp ở đây
        public ActionResult Create(Category objCategory)
        {
            try
            {
                if (objCategory.ImageUpload != null && objCategory.ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    objCategory.Image = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/categories/"), fileName);
                    objCategory.ImageUpload.SaveAs(path);
                }

                // Nếu chọn "None", đặt Parent_Id = null
                if (objCategory.Parent_Id == 0)
                {
                    objCategory.Parent_Id = null;
                }

                db.Categories.Add(objCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                // Cung cấp dữ liệu cho dropdown list để người dùng chọn lại
                var categories = db.Categories.ToList();
                categories.Insert(0, new Category { Id = 0, Name = "None" }); // Thêm lựa chọn "None"
                ViewBag.Parent_Id = new SelectList(categories, "Id", "Name"); // Chỉnh sửa đây
                return View(objCategory);
            }
        }

        [HttpGet]
        public ActionResult Edit(int Id)
        {
            var objCategory = db.Categories.Find(Id);
            if (objCategory == null)
            {
                return HttpNotFound();
            }

            // Cung cấp dữ liệu cho dropdown list với lựa chọn "None"
            var categories = db.Categories.ToList();
            categories.Insert(0, new Category { Id = 0, Name = "None" }); // Thêm lựa chọn "None"

            ViewBag.Parent_Id = new SelectList(categories, "Id", "Name", objCategory.Parent_Id); // Chỉnh sửa đây
            return View(objCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Sửa lỗi cú pháp ở đây
        public ActionResult Edit(Category objCategory)
        {
            try
            {
                if (objCategory.ImageUpload != null && objCategory.ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    objCategory.Image = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/categories/"), fileName);
                    objCategory.ImageUpload.SaveAs(path);
                }

                // Nếu chọn "None", đặt Parent_Id = null
                if (objCategory.Parent_Id == 0)
                {
                    objCategory.Parent_Id = null;
                }

                db.Entry(objCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                // Cung cấp dữ liệu cho dropdown list để người dùng chọn lại
                var categories = db.Categories.ToList();
                categories.Insert(0, new Category { Id = 0, Name = "None" }); // Thêm lựa chọn "None"
                ViewBag.Parent_Id = new SelectList(categories, "Id", "Name", objCategory.Parent_Id); // Chỉnh sửa đây
                return View(objCategory);
            }
        }

        [HttpGet]
        public ActionResult Details(int Id)
        {
            var objCategory = db.Categories.Find(Id);
            if (objCategory == null)
            {
                return HttpNotFound();
            }
            return View(objCategory);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            var objCategory = db.Categories.Find(Id);
            if (objCategory == null)
            {
                return HttpNotFound();
            }
            return View(objCategory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // Sửa lỗi cú pháp ở đây
        public ActionResult CFDelete(Category objCater)
        {
            var objCategory = db.Categories.Where(n => n.Id == objCater.Id).FirstOrDefault();

            if (objCategory != null)
            {
                db.Categories.Remove(objCategory);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
    }
}
