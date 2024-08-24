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
        public class ProductAdminController : Controller
        {
            private OnlineShopEntities4 db = new OnlineShopEntities4();

            // GET: Admin/ProductAdmin
            public ActionResult Index(string SearchString, string currentFilter, int? page)
            {
                var listProduct = new List<Product>();
                if (SearchString != null)
                {
                    page = 1;
                }
                else
                {
                    SearchString = currentFilter;
                }
                if (!string.IsNullOrEmpty(SearchString))
                {
                    listProduct = db.Products.Where(n => n.ProductName.Contains(SearchString)).ToList();
                }
                else
                {
                    listProduct = db.Products.ToList();
                }
                ViewBag.CurrentFilter = SearchString;
                int pageSize = 4;
                int pageNumber = (page ?? 1);
                listProduct = listProduct.OrderByDescending(n => n.ProductID).ToList();
                return View(listProduct.ToPagedList(pageNumber, pageSize));
            }

            [HttpGet]
            public ActionResult Create()
            {
                var categories = db.Categories.ToList();
                ViewBag.Category_id = new SelectList(categories, "Id", "Name");

                var brands = db.Brands.ToList();
                ViewBag.Brand_id = new SelectList(brands, "Id", "Name");

                return View();
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Create(Product objProduct, HttpPostedFileBase ImageUpload)
            {
                try
                {
                    if (ImageUpload != null && ImageUpload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                        string extension = Path.GetExtension(ImageUpload.FileName);
                        fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        objProduct.Image = fileName;
                        string path = Path.Combine(Server.MapPath("~/Content/images/items/"), fileName);
                        ImageUpload.SaveAs(path);
                    }

                    db.Products.Add(objProduct);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                    ViewBag.Category_id = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ViewBag.Brand_id = new SelectList(db.Brands.ToList(), "Id", "Name");
                    return View(objProduct);
                }
            }

            [HttpGet]
            public ActionResult Details(int id)
            {
                var objProduct = db.Products.Where(n => n.ProductID == id).FirstOrDefault();
                return View(objProduct);
            }

            [HttpGet]
            public ActionResult Delete(int id)
            {
                var objProduct = db.Products.Where(n => n.ProductID == id).FirstOrDefault();
                return View(objProduct);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult DeleteConfirmed(int id)
            {
                var objProduct = db.Products.Where(n => n.ProductID == id).FirstOrDefault();
                if (objProduct != null)
                {
                    db.Products.Remove(objProduct);
                    db.SaveChanges();
                }
                return RedirectToAction("Index");
            }

            [HttpGet]
            public ActionResult Edit(int id)
            {
                var objProduct = db.Products.Find(id);
                if (objProduct == null)
                {
                    return HttpNotFound();
                }

                ViewBag.Category_id = new SelectList(db.Categories.ToList(), "Id", "Name", objProduct.CategoryID);
                ViewBag.Brand_id = new SelectList(db.Brands.ToList(), "Id", "Name", objProduct.BrandID);

                return View(objProduct);
            }

            [HttpPost]
            [ValidateAntiForgeryToken]
            public ActionResult Edit(Product objProduct, HttpPostedFileBase ImageUpload)
            {
                try
                {
                    if (ImageUpload != null && ImageUpload.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(ImageUpload.FileName);
                        string extension = Path.GetExtension(ImageUpload.FileName);
                        fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        objProduct.Image = fileName;
                        string path = Path.Combine(Server.MapPath("~/Content/images/items/"), fileName);
                        ImageUpload.SaveAs(path);
                    }

                    db.Entry(objProduct).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                    ViewBag.Category_id = new SelectList(db.Categories.ToList(), "Id", "Name", objProduct.CategoryID);
                    ViewBag.Brand_id = new SelectList(db.Brands.ToList(), "Id", "Name", objProduct.BrandID);
                    return View(objProduct);
                }
            }
        }
    }
