using Ozn.MvcWebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ozn.MvcWebUI.Models
{
    public class Cart
    {
        private List<CartLine> _cardLines = new List<CartLine>();
        public List<CartLine> CartLines
        {
            get { return _cardLines; }
        }
        public void AddProduct(Product product, int quantity)//ekleme kısmı
        {
            var line = _cardLines.FirstOrDefault(i => i.Product.Id == product.Id);
            if (line == null) //ürün yoksa 
            {
                _cardLines.Add(new CartLine() { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;//var olan ürünü eklersek adet sayısı artacak
            }
        }
        public void DeleteProduct(Product product)//eleman silme kısmı
        {
            _cardLines.RemoveAll(i => i.Product.Id == product.Id);
        }
        public double Total()
        {   //toplam fiyat
            return _cardLines.Sum(i => i.Product.Price * i.Quantity);
        }   //her ürünün fiytını alıyor i ye atıyor her ürünün adediyle çarpıyor.
        public void Clear()
        {                    //tüm elemanları silme kısmı
            _cardLines.Clear();
        }
    }
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}