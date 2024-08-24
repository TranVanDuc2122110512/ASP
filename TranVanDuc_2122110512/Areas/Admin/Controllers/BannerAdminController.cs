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
    public class BannerController : Controller
    {
        private OnlineShopEntities4 db = new OnlineShopEntities4();

        // GET: Admin/Banner
        public ActionResult Index(string SearchString, string currentFilter, int? page)
        {
            // Handle the search query
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            ViewBag.CurrentFilter = SearchString;

            // Query banners based on the search filter
            var listBanner = db.Banners.AsQueryable();
            if (!string.IsNullOrEmpty(SearchString))
            {
                listBanner = listBanner.Where(n => n.BannerTitle.Contains(SearchString));
            }

            // Order by descending Id
            listBanner = listBanner.OrderByDescending(n => n.BannerId);

            // Pagination settings
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            return View(listBanner.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Banner objBanner, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    string extension = Path.GetExtension(ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objBanner.BannerImage = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/banners/"), fileName);
                    ImageUpload.SaveAs(path);
                }
                else
                {
                    // Optional: Set a default image if no image is uploaded
                    objBanner.BannerImage = "default-banner.png"; // Change this to your default image name
                }

                db.Banners.Add(objBanner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(objBanner);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objBanner = db.Banners.Find(id);
            if (objBanner == null)
            {
                return HttpNotFound();
            }

            return View(objBanner);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBanner = db.Banners.Find(id);
            if (objBanner == null)
            {
                return HttpNotFound();
            }

            return View(objBanner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var bannerToDelete = db.Banners.Find(id);
            if (bannerToDelete != null)
            {
                db.Banners.Remove(bannerToDelete);
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBanner = db.Banners.Find(id);
            if (objBanner == null)
            {
                return HttpNotFound();
            }

            return View(objBanner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Banner objBanner, HttpPostedFileBase ImageUpload)
        {
            if (ModelState.IsValid)
            {
                if (ImageUpload != null && ImageUpload.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                    string extension = Path.GetExtension(ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objBanner.BannerImage = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/banners/"), fileName);
                    ImageUpload.SaveAs(path);
                }

                db.Entry(objBanner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(objBanner);
        }
    }
}
