using PagedList;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using TranVanDuc_2122110512.Context;


namespace TranVanDuc_2122110512.Areas.Admin.Controllers
{
    public class ContactController : Controller
    {
        private readonly OnlineShopEntities4 db = new OnlineShopEntities4();

        // GET: Admin/Contact
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

            var contacts = db.Contacts.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(c => c.Name.Contains(searchString) || c.Email.Contains(searchString));
            }

            ViewBag.CurrentFilter = searchString;

            int pageSize = 10;  // Set the page size as needed
            int pageNumber = page ?? 1;

            contacts = contacts.OrderByDescending(c => c.Id);

            return View(contacts.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.User_id = new SelectList(db.Registers.Select(u => new
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName
            }).ToList(), "Id", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Contacts.Add(contact);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }
            ViewBag.User_id = new SelectList(db.Registers.Select(u => new
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName
            }).ToList(), "Id", "FullName", contact.User_id);
            return View(contact);
        }

        [HttpGet]
        public ActionResult Edit(long id)
        {
            var contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }
            ViewBag.User_id = new SelectList(db.Registers.Select(u => new
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName
            }).ToList(), "Id", "FullName", contact.User_id);
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Contact contact)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(contact).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                }
            }
            ViewBag.User_id = new SelectList(db.Registers.Select(u => new
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName
            }).ToList(), "Id", "FullName", contact.User_id);
            return View(contact);
        }
        [HttpGet]
        public ActionResult Details(long id)
        {
            var contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            // Optionally, you can also include related information here, such as user details
            // var user = db.Users.Find(contact.User_id);
            // ViewBag.User = user;

            return View(contact);
        }
        [HttpGet]
        public ActionResult Delete(long id)
        {
            var contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            // Optionally, you can also include related information here, such as user details
            // var user = db.Users.Find(contact.User_id);
            // ViewBag.User = user;

            return View(contact);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            var contact = db.Contacts.Find(id);
            if (contact == null)
            {
                return HttpNotFound();
            }

            try
            {
                db.Contacts.Remove(contact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra: " + ex.Message);
                return View(contact);
            }
        }

    }
}