using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using App32_MasterDetail.Models;

namespace App32_MasterDetail.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext (DbContextOptions<AppDBContext> options)
            : base(options)
        {
        }

        public DbSet<App32_MasterDetail.Models.Product> Product { get; set; } = default!;
        public DbSet<App32_MasterDetail.Models.SalesMaster> SalesMaster { get; set; } = default!;
        public DbSet<App32_MasterDetail.Models.SalesDetail> SalesDetail { get; set; } = default!;
    }
}
