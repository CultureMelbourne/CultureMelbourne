using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace TP03MainProj.Models
{
    public class MyApplicationContext : DbContext
    {
        public MyApplicationContext() : base("name=MyDatabase") { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Events> Events { get; set; }
        public DbSet<CalenderDate> CalenderDates { get; set; }
    }
}