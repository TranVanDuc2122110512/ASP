using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using TranVanDuc_2122110512.Context;

namespace TranVanDuc_2122110512.Controllers
{
    public class CartController : Controller
    {
        private OnlineShopEntities4 db = new OnlineShopEntities4();

        // Thêm sản phẩm vào giỏ hàng
        [HttpPost]
        public ActionResult AddToCart(int productID, int quantity)
        {
            var product = db.Products.Find(productID);
            if (product == null)
            {
                return HttpNotFound();
            }

            string userIdString = User.Identity.GetUserId(); // Lấy UserId từ hệ thống xác thực

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Invalid user ID.");
            }

            var cart = db.Carts.FirstOrDefault(c => c.Id == userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    Id = userId, // Sử dụng Id là int
                    CreatedDate = DateTime.Now
                };
                db.Carts.Add(cart);
                db.SaveChanges();
            }

            var cartItem = db.CartItems.FirstOrDefault(ci => ci.CartID == cart.CartID && ci.ProductID == productID);
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    CartID = cart.CartID,
                    ProductID = productID,
                    Quantity = quantity
                };
                db.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity += quantity;
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // Hiển thị giỏ hàng
        public ActionResult Index()
        {
            string userIdString = User.Identity.GetUserId(); // Lấy UserId từ hệ thống xác thực

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Invalid user ID.");
            }

            var cart = db.Carts
                .Include(c => c.CartItems.Select(ci => ci.Product))
                .FirstOrDefault(c => c.Id == userId);

            return View(cart);
        }

        // Cập nhật giỏ hàng
        [HttpPost]
        public ActionResult UpdateCart(Dictionary<int, int> quantities)
        {
            string userIdString = User.Identity.GetUserId(); // Lấy UserId từ hệ thống xác thực

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Invalid user ID.");
            }

            var cart = db.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.Id == userId);

            if (cart != null)
            {
                foreach (var item in cart.CartItems.ToList())
                {
                    if (quantities.ContainsKey((int)item.ProductID))
                    {
                        int newQuantity = quantities[(int)item.ProductID];
                        if (newQuantity <= 0)
                        {
                            db.CartItems.Remove(item);
                        }
                        else
                        {
                            item.Quantity = newQuantity;
                            db.Entry(item).State = EntityState.Modified;
                        }
                    }
                }
                db.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public ActionResult RemoveFromCart(int productId)
        {
            string userIdString = User.Identity.GetUserId(); // Lấy UserId từ hệ thống xác thực

            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest, "Invalid user ID.");
            }

            var cart = db.Carts
                .Include(c => c.CartItems)
                .FirstOrDefault(c => c.Id == userId);

            if (cart != null)
            {
                var itemToRemove = cart.CartItems.FirstOrDefault(ci => ci.ProductID == productId);
                if (itemToRemove != null)
                {
                    db.CartItems.Remove(itemToRemove);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
