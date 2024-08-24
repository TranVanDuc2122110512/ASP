using PagedList;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TranVanDuc_2122110512.Context;

namespace TranVanDuc_2122110512.Areas.Admin.Controllers
{
    public class UserController : Controller
    {
        private OnlineShopEntities4 db = new OnlineShopEntities4(); // Di chuyển khai báo biến db vào lớp

        // GET: Admin/User
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            // Xử lý truy vấn tìm kiếm
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            ViewBag.CurrentFilter = SearchString;

            // Truy vấn người dùng theo bộ lọc tìm kiếm
            var listUser = db.Registers.AsQueryable();
            if (!string.IsNullOrEmpty(SearchString))
            {
                listUser = listUser.Where(n => n.FirstName.Contains(SearchString) || n.Email.Contains(SearchString));
            }

            // Sắp xếp theo Id giảm dần
            listUser = listUser.OrderByDescending(n => n.Id);

            // Cài đặt phân trang
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(listUser.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Register objUser, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    string extension = Path.GetExtension(ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objUser.Image = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/users/"), fileName);
                    ImageUpload.SaveAs(path);
                }
                else
                {
                    // Đặt giá trị mặc định nếu không có hình ảnh tải lên
                    objUser.Image = "default-image.png"; // Thay thế bằng hình ảnh mặc định của bạn
                }

                db.Registers.Add(objUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(objUser);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objUser = db.Registers.Find(id);
            if (objUser == null)
            {
                return HttpNotFound();
            }

            return View(objUser);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objUser = db.Registers.Find(id);
            if (objUser == null)
            {
                return HttpNotFound();
            }

            return View(objUser);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var userToDelete = db.Registers.Find(id);
            if (userToDelete != null)
            {
                db.Registers.Remove(userToDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objUser = db.Registers.Find(id);
            if (objUser == null)
            {
                return HttpNotFound();
            }

            return View(objUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Register objUser, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                var userToUpdate = db.Registers.Find(id);
                if (userToUpdate == null)
                {
                    return HttpNotFound();
                }

                // Cập nhật các thuộc tính
                userToUpdate.FirstName = objUser.FirstName;
                userToUpdate.LastName = objUser.LastName;
                userToUpdate.Email = objUser.Email;
                // Cập nhật thêm các thuộc tính khác của objUser vào userToUpdate nếu cần

                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    string extension = Path.GetExtension(ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    userToUpdate.Image = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/users/"), fileName);
                    ImageUpload.SaveAs(path);
                }

                db.Entry(userToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(objUser);
        }
    }
}
