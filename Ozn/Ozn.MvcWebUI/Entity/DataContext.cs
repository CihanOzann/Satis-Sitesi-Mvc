﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace Ozn.MvcWebUI.Entity
{
    public class DataContext:DbContext
    {
        public DataContext() :base("dataConnection")
        {
            //Database.SetInitializer(new DataInitializer());
        }//DataInitializer ile test verileri direk ekleniyor

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

    }
}