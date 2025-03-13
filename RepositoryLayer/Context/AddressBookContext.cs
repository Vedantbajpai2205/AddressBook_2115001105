﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using RepositoryLayer.Entity;

namespace RepositoryLayer.Context
{
    public class AddressBookContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<AddressBookEntity> AddressBookEntries { get; set; }

        public AddressBookContext(DbContextOptions<AddressBookContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AddressBookEntity>()
                .HasOne(ab => ab.User)
                .WithMany(u => u.AddressBookEntries)
                .HasForeignKey(ab => ab.UserId);
        }
    }
}