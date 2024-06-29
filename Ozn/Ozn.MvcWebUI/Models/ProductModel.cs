using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ozn.MvcWebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public string Image { get; set; }

        //Tablo anahtar kullanma category diğer tablodan çekiliyor
        public int CategoryId { get; set; }
    }
}