using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataBase.Data
{
    public class ApplicationDBContext : IdentityDbContext<PharmcyInfo>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {

        }
        public DbSet<Drug> Drug { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<FinalOrder> finalOrders { get; set; }
    }
}
