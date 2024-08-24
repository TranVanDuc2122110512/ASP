using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TranVanDuc_2122110512.Context;


namespace TranVanDuc_2122110512.Areas.Admin.Controllers
{
    public class TopicController : Controller
    {
        private readonly OnlineShopEntities4 db = new OnlineShopEntities4();

        // GET: Admin/Topic
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

            var topics = db.Topics.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                topics = topics.Where(t => t.TopicName.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 4;
            int pageNumber = page ?? 1;

            topics = topics.OrderByDescending(t => t.TopicId);

            return View(topics.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Topic topic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Topics.Add(topic);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }
            return View(topic);
        }

        [HttpGet]
        public ActionResult Details(long id)
        {
            var topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        [HttpGet]
        public ActionResult Delete(long id)
        {
            var topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(long id, FormCollection collection)
        {
            var topic = db.Topics.Find(id);
            if (topic != null)
            {
                try
                {
                    db.Topics.Remove(topic);
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var topic = db.Topics.Find(id);
            if (topic == null)
            {
                return HttpNotFound();
            }
            return View(topic);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Topic topic)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(topic).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }
            return View(topic);
        }
    }
}
