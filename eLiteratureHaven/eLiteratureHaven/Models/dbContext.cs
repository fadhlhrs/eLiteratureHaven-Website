using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using eLiteratureHaven.Models;
using System.Data.Entity;

namespace eLiteratureHaven.Models
{
    public class dbContext
    {
        public DbSet<users> users { get; set; }
    }
}