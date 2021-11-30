using ECommerceProjectWithWebAPI.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceProjectWithWebAPI.Models.DataContext
{
    public class ECommerceProjectWithWebAPIDbContext : DbContext
    {
        public ECommerceProjectWithWebAPIDbContext(DbContextOptions options):base(options)
        {

        }


        public DbSet<User> Users { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<TrackAction> TrackActions { get; set; }
    }
}
