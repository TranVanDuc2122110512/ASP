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
    public class PostController : Controller
    {
        private readonly OnlineShopEntities4 db = new OnlineShopEntities4();

        // GET: Admin/Post
        public ActionResult Index(string searchString, string currentFilter, int? page)
        {
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var posts = db.Posts.Include(p => p.PostId).AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                posts = posts.Where(p => p.Title.Contains(searchString));
            }

            posts = posts.OrderByDescending(p => p.PostId);

            int pageSize = 4;
            int pageNumber = page ?? 1;

            return View(posts.ToPagedList(pageNumber, pageSize)); // This should be IPagedList<Post>
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Topic_id = new SelectList(db.Topics, "Topic_id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Post post, HttpPostedFileBase imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null && imageUrl.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(imageUrl.FileName);
                    string extension = Path.GetExtension(imageUrl.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    post.ImageUrl = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/posts/"), fileName);
                    imageUrl.SaveAs(path);
                }

                db.Posts.Add(post);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Topic_id = new SelectList(db.Topics, "Topic_id", "Name", post.TopicId);
            return View(post);
        }

        [HttpGet]
        public ActionResult Details(long id)
        {
            var post = db.Posts.Include(p => p.PostId).FirstOrDefault(p => p.PostId == id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpGet]
        public ActionResult Delete(long id)
        {
            var post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var post = db.Posts.Find(id);
            if (post != null)
            {
                db.Posts.Remove(post);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }

            ViewBag.Topic_id = new SelectList(db.Topics, "Topic_id", "Name", post.TopicId);
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post, HttpPostedFileBase imageUrl)
        {
            if (ModelState.IsValid)
            {
                if (imageUrl != null && imageUrl.ContentLength > 0)
                {
                    string fileName = Path.GetFileNameWithoutExtension(imageUrl.FileName);
                    string extension = Path.GetExtension(imageUrl.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    post.ImageUrl = fileName;
                    string path = Path.Combine(Server.MapPath("~/Content/images/posts/"), fileName);
                    imageUrl.SaveAs(path);
                }

                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Topic_id = new SelectList(db.Topics, "Topic_id", "Name", post.TopicId);
            return View(post);
        }
    }
}
