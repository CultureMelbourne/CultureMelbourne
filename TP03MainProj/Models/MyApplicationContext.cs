using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TP03MainProj.Models
{
    public class MyApplicationContext : DbContext
    {
        public MyApplicationContext() : base("name=MyDatabase") { }

        public DbSet<Product> Products { get; set; }
    }
}