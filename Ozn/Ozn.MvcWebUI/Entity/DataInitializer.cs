using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Ozn.MvcWebUI.Entity
{
    //eğer model değiştiyse veritabanını oluştur->datacontextin bize gösterdiği veri tabanını oluşturur.
    public class DataInitializer: DropCreateDatabaseIfModelChanges<DataContext>
    {
        protected override void Seed(DataContext context)
        {
        var kategoriler = new List<Category>()
            {
                new Category(){Name="Kamera",Description="Kamera Ürünleri"},
                new Category(){Name="Bilgisayar",Description="Bilgisayar Ürünleri"},
                new Category(){Name="Elektronik",Description="Elektronik Ürünleri"},
                new Category(){Name="Telefon",Description="Telefon Ürünleri"},
                new Category(){Name="Beyaz Eşya",Description="Beyaz Eşya Ürünleri"}
            };


            foreach (var kategori in kategoriler)
            {
                context.Categories.Add(kategori);
            }
            context.SaveChanges();


            var urunler = new List<Product>()
            {
                new Product() {Name="Canon",Description="Kullanmayı çok seveceğiniz ergonomik tasarım Optik vizör, çekiminizi oluşturmanıza ve tahmin etmenize olanak tanıyarak her zaman anın arkasındaki duyguyu yakalamak için hazır olmanızı sağlar. Sezgisel kullanımlı kullanıcı dostu kontrolleri ve görüntüyü incelemek için 7,5 cm'lik (3 inç) geniş LCD ekranıyla EOS 1200D'yi kullanması çok keyiflidir.",Price=1200,Stock=5,IsApproved=true,CategoryId=1,IsHome=true ,Image="6.jpg"},
                new Product() {Name="Canon2",Description="Kullanmayı çok seveceğiniz ergonomik tasarım Optik vizör, çekiminizi oluşturmanıza ve tahmin etmenize olanak tanıyarak her zaman anın arkasındaki duyguyu yakalamak için hazır olmanızı sağlar. Sezgisel kullanımlı kullanıcı dostu kontrolleri ve görüntüyü incelemek için 7,5 cm'lik (3 inç) geniş LCD ekranıyla EOS 1200D'yi kullanması çok keyiflidir.",Price=2400,Stock=10,IsApproved=true,CategoryId=1,Image="6.jpg"},

                new Product() {Name="Monster",Description="Kullanmayı çok seveceğiniz ergonomik tasarım Optik vizör, çekiminizi oluşturmanıza ve tahmin etmenize olanak tanıyarak her zaman anın arkasındaki duyguyu yakalamak için hazır olmanızı sağlar. Sezgisel kullanımlı kullanıcı dostu kontrolleri ve görüntüyü incelemek için 7,5 cm'lik (3 inç) geniş LCD ekranıyla EOS 1200D'yi kullanması çok keyiflidir.",Price=1000,Stock=2,IsApproved=true,CategoryId=2,Image="2.jpg"},
                new Product() {Name="Monster",Description="Bilgisayar",Price=1000,Stock=2,IsApproved=true,CategoryId=2,Image="2.jpg"},
                new Product() {Name="Monster",Description="Bilgisayar",Price=1000,Stock=2,IsApproved=true,CategoryId=2 ,Image="2.jpg"},

                new Product() {Name="Ütü",Description="Elektronik",Price=3000,Stock=3,IsApproved=true,CategoryId=3,IsHome=true, Image="3.jpg" },

                new Product() {Name="İphone",Description="Telefon",Price=35000,Stock=4,IsApproved=true,CategoryId=4,Image="4.jpg" }

                //new Product() {Name="Buz Dolabı",Description="Beyaz Eşya",Price=8000,Stock=5,IsApproved=true,CategoryId=5}

            };


            foreach (var urun in urunler)
            {
                context.Products.Add(urun);
            }

            context.SaveChanges();

            base.Seed(context);
        }
    }
}