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
    }
}
