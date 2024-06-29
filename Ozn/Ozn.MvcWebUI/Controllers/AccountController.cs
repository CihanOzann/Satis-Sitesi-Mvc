using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using Ozn.MvcWebUI.Entity;
using Ozn.MvcWebUI.Identity;
using Ozn.MvcWebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Ozn.MvcWebUI.Controllers
{
    public class AccountController : Controller
    {
        private DataContext db = new DataContext();

        private UserManager<ApplicationUser> UserManager;
        private RoleManager<ApplicationRole> RoleManager;

        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            UserManager = new UserManager<ApplicationUser>(userStore);
            var roleStore = new RoleStore<ApplicationRole>(new IdentityDataContext());
            RoleManager = new RoleManager<ApplicationRole>(roleStore);

        }
        [Authorize]
        public ActionResult Index()
        {
            var username = User.Identity.Name;
            var orders = db.Orders
                .Where(i => i.Username == username)
                .Select(i => new UserOrderModel()
                {
                    Id = i.Id,
                    OrderNumber = i.OrderNumber,
                    OrderDate = i.OrderDate,
                    OrderState = i.OrderState,
                    Total = i.Total
                }).OrderByDescending(i => i.OrderDate).ToList();

            return View(orders);
        }
        [Authorize]

        public ActionResult Details(int id)
        {
            var entity = db.Orders.Where(i => i.Id == id)
                   .Select(i => new OrderDetailsModel()
                   {
                       OrderId = i.Id,
                       OrderNumber = i.OrderNumber,
                       Total = i.Total,
                       OrderDate = i.OrderDate,
                       OrderState = i.OrderState,
                       AdresBasligi = i.AdresBasligi,
                       Adres = i.Adres,
                       Sehir = i.Sehir,
                       Semt = i.Semt,
                       Mahalle = i.Mahalle,
                       PostaKodu = i.PostaKodu,
                       Orderlines = i.Orderlines.Select(a => new OrderLineModel()
                       {
                           ProductId = a.ProductId,
                           ProductName = a.Product.Name.Length > 50 ? a.Product.Name.Substring(0, 47) +
                           "..." : a.Product.Name,
                           Image = a.Product.Image,
                           Quantity = a.Quantity,
                           Price = a.Price
                       }).ToList()
                   }).FirstOrDefault();
            return View(entity);
        }



        // GET: Account
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //güvenlik için
        public ActionResult Register(Register model) 
        { //model, kullanıcının kayıt işlemi için gerekli bilgileri içerir.(kullanıcı adı, parola vb.
            if (ModelState.IsValid)//Eğer ModelState geçerliyse
                                   //(yani, form doğrulaması başarılıysa), kayıt işlemi yapılır.
            {
                //Kayıt işlemleri
                var user = new ApplicationUser();
                user.Name = model.Name;
                user.Surname = model.SurName;
                user.Email = model.Email;
                user.UserName = model.UserName;

                var result = UserManager.Create(user, model.Password);

                if (result.Succeeded)
                {   //kullanıcı oluştu ve kullanıcıyı bir role atayabilirsiniz.
                    if (RoleManager.RoleExists("user"))
                    {
                        UserManager.AddToRole(user.Id, "user");//kullanıcı oluşmuşsa role atıyoruz.
                    }
                    return RedirectToAction("Login", "Account");
                }
                else
                {//kayıt işlemi başarısızsa, ModelState'e bir hata eklenir ve sorun olduğunu bildirir.
                    ModelState.AddModelError("RegisterUserError", "Kullanıcı  oluşturma hatası.");
                }
            }
            return View(model);
        }



        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]                  // @Html.AntiForgeryToken() kullanılan sayfa
        [ValidateAntiForgeryToken] //bu sayfayı çağıran kullanıcıyla post eden kullanıcı aynı mı konrtol et
        public ActionResult Login(Login model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                //Login işlemleri           // veritabanında kullanıcı adı ve parolayı kontrol eder.
                var user = UserManager.Find(model.UserName, model.Password);//kullanıcı bul

                if (user != null)//kullanıcı varsa
                {
                    // varolan kullanıcıyı sisteme dahil et.
                    // ApplicationCookie oluşturup sisteme bırak. "Rememberme beni hatırla kutusu"

                    var authManager = HttpContext.GetOwinContext().Authentication;
                    var identityclaims = UserManager.CreateIdentity(user, "ApplicationCookie");
                    var authProperties = new AuthenticationProperties();
                    authProperties.IsPersistent = model.RememberMe;//Cookie kalıcı mı silinecek mi
                    authManager.SignIn(authProperties, identityclaims);//kullanıcıyı sisteme dahil et

                    if (!String.IsNullOrEmpty(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                else
                {               
                    ModelState.AddModelError("LoginUserError", "Böyle bir kullanıcı yok.");
                }   //ValidationSummary olduğu kısma çıkar bu error bilgisi
            }
            return View(model);
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;
            authManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

    }
}