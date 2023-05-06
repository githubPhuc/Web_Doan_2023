using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Web_Doan_2023.Models;

namespace Web_Doan_2023.Data
{
    public class Web_Doan_2023Context : IdentityDbContext<User>
    {
        public Web_Doan_2023Context(DbContextOptions<Web_Doan_2023Context> options)
            : base(options)
        {
        }

        public DbSet<Menu> Menu { get; set; }
        public DbSet<EditAccountModel> EditaccountModel { get; set; }
        public DbSet<Web_Doan_2023.Models.Page>? Page { get; set; }
        public DbSet<Web_Doan_2023.Models.Depot>? Depot { get; set; }
        public DbSet<Web_Doan_2023.Models.User_Page>? User_Page { get; set; }
        public DbSet<Web_Doan_2023.Models.CategoryProduct>? CategoryProduct { get; set; }
        public DbSet<Web_Doan_2023.Models.Department>? Department { get; set; }
        public DbSet<Web_Doan_2023.Models.City>? City { get; set; }
        public DbSet<Web_Doan_2023.Models.Product>? Product { get; set; }
        public DbSet<Web_Doan_2023.Models.Producer>? Producer { get; set; }
        public DbSet<Web_Doan_2023.Models.Wards>? Wards { get; set; }
        public DbSet<Web_Doan_2023.Models.District>? District { get; set; }
        public DbSet<Web_Doan_2023.Models.Sale>? Sale { get; set; }
        public DbSet<Web_Doan_2023.Models.enterWarehouseVouchers>? enterWarehouseVouchers { get; set; }
        public DbSet<Web_Doan_2023.Models.exportWarehouseVouchers>? exportWarehouseVouchers { get; set; }
        public DbSet<Web_Doan_2023.Models.CartProduct>? CartProduct { get; set; }
        public DbSet<Web_Doan_2023.Models.CartDetailProduct>? CartDetailProduct { get; set; }
        public DbSet<Web_Doan_2023.Models.productDepot>? productDepot { get; set; }
        public DbSet<Web_Doan_2023.Models.ImportBillDepot>? ImportBillDepot { get; set; }
        public DbSet<Web_Doan_2023.Models.Images>? Images { get; set; }
    }
}
