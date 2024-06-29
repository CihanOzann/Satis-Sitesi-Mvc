using Ozn.MvcWebUI.Entity;
using Ozn.MvcWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ozn.MvcWebUI.Controllers
{
    public class CartController : Controller
    {
        // GET: Cart

        private DataContext db = new DataContext();
        // GET: Cart
        public ActionResult Index()
        {
            return View(GetCart());
        }

        public ActionResult AddToCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);

            if (product != null)
            {
                GetCart().AddProduct(product, 1);
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int Id)
        {
            var product = db.Products.FirstOrDefault(i => i.Id == Id);

            if (product != null)
            {
                GetCart().DeleteProduct(product);
            }

            return RedirectToAction("Index");
        }

        public Cart GetCart()
        {
            var cart = (Cart)Session["Cart"];

            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public PartialViewResult Summary()
        {
            return PartialView(GetCart());
        }

        public ActionResult Checkout()
        {
            return View(new ShippingDetails());
        }

        [HttpPost]
        public ActionResult Checkout(ShippingDetails entity)
        {
            var cart = GetCart(); //GetCArt ile session içerisinden kartı aldım

            if (cart.CartLines.Count == 0)//cartta ürün yoksa
            {
                ModelState.AddModelError("UrunYokError", "Sepetinizde ürün bulunmamaktadır."); //error uyarı
            }

            if (ModelState.IsValid)//her şey doğru gerçekleytiyse
            {
                SaveOrder(cart, entity);
                cart.Clear();  //cart'ı sıfırla    
                return View("Completed"); //sayfayı göster
            }
            else
            {
                return View(entity);
            }
        }

 
        private void SaveOrder(Cart cart, ShippingDetails entity)
        {
            var order = new Order();
            //Her sipariş için rastgele sayı ile kod oluşturdum.
            order.OrderNumber = "A" + (new Random()).Next(11111, 99999).ToString();
            order.Total = cart.Total();//Total hesaplama
            order.OrderDate = DateTime.Now;//Şimdiki sistem saati
            order.OrderState = EnumOrderState.Waiting;//bekleniyor 

              //entity'den bilgileri çekme
            order.Username = User.Identity.Name;


            order.AdresBasligi = entity.AdresBasligi;
            order.Adres = entity.Adres;
            order.Sehir = entity.Sehir;
            order.Semt = entity.Semt;
            order.Mahalle = entity.Mahalle;
            order.PostaKodu = entity.PostaKodu;

            order.Orderlines = new List<OrderLine>();//Orderlines listesi
            //Orderlines içine OrderLine entity içindeki bilgilerle doldurdum
            foreach (var pr in cart.CartLines)
            {
                var orderline = new OrderLine();
                orderline.Quantity = pr.Quantity;
                orderline.Price = pr.Quantity * pr.Product.Price;
                orderline.ProductId = pr.Product.Id;
                //order içindeki orderlines
                order.Orderlines.Add(orderline);//orderline ile orderlines'a ekledim.
            }
            db.Orders.Add(order);
            db.SaveChanges();//kayıt işlemini yaptım
        }
    }
}