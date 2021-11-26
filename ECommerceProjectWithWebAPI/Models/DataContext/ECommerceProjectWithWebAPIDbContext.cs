﻿using ECommerceProjectWithWebAPI.Models.Entities;
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
    }
}
