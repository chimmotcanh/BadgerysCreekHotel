using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BadgerysCreekHotel.Models;

namespace BadgerysCreekHotel.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<BadgerysCreekHotel.Models.Customer> Customer { get; set; }
        public DbSet<BadgerysCreekHotel.Models.Booking> Booking { get; set; }
        public DbSet<BadgerysCreekHotel.Models.Room> Room { get; set; }
    }
}
