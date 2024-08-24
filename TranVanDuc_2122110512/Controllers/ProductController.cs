using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Web;
using System.Web.Mvc;
using TranVanDuc_2122110512.Context;

namespace TranVanDuc_2122110512.Controllers
{
    public class ProductController : Controller
    {
        private OnlineShopEntities4 db = new OnlineShopEntities4();

        // GET: product
        public ActionResult Index()
        {
            return View(db.Products.ToList());
        }

        // GET: Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: product/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,category_id,brand_id,name,slug,detail,description,image,price,pricesale,created_at,updated_at,created_by,updated_by,status,qty")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: /Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: vmt_product/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,category_id,brand_id,name,slug,detail,description,image,price,pricesale,created_at,updated_at,created_by,updated_by,status,qty")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: /Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: /Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult DetailProduct(int id)
        {
            var product  =  db.ProductDetails.Find(id);
            return View(product);
        }
        public ActionResult ListingLargeProdutt()
        {
            return View();
        }

        // GET: CategoryProducts/5
        public ActionResult CategoryProducts(int id)
        {
            // Lấy sản phẩm theo danh mục
            var products = db.Products.Where(p => p.CategoryID == id).ToList();

            // Truyền danh mục vào ViewBag
            ViewBag.Categories = db.Categories.ToList();

            return View(products);
        }
    }
}
