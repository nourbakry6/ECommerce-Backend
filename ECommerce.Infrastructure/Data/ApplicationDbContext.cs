using ECommerce.Domain.entites;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ECommerce.Infrastructure.Data
{// hyda classs ms2ul mae t3mol mae database
 //hwh context mbni foe ef core ta y3ml mae identity so hiye dbcontext +asp core identity
 //law emlta IdentityDbContext<ApplicationUser,,iny>bidun role kn ns emla tanle idenity tie user bs iza zdt role mtl tht user sar fi ykhd kza role
 //bs lh role identity ? ya3ni lh ma emltu table adi w khlt enud al2a mae user ? laenu role hw jz2 mn identity msh mtl cart ya3ni 
 //user +role:
//    AspNetUsers
//   AspNetRoles
//AspNetUserRoles
//AspNetUserClaims
//AspNetRoleClaims
//AspNetUserLogins
//AspNetUserTokens
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole<int>,int> {
        //dbcontext option b 2lbu ef core 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)//y3ni wdi hl option lal parent  claass li hwh dbcontext
        {

     }
     //y3ni t3ml mae user l emltu b entity t3ml ma3u kaenu table
     //public DbSet<User> Users { get; set; }
     public DbSet<Product> Products { get; set; }
     public DbSet<Category> Categories { get; set; }
     public DbSet<Cart> Carts { get; set; }
     public DbSet<CartItem> CartItems { get; set; }
     public DbSet<Order>Orders{  get; set; }
     public DbSet<OrderItem> OrderItems { get; set; }
        public object Category { get; internal set; }


        //krml ef core b sya3mol db  model y3tmd hl 3alea ben entity
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // User → Cart
            builder.Entity<ApplicationUser>()
                .HasOne<Cart>()//kl user endu cart 1
                .WithOne()//kl cart end user whad
                .HasForeignKey<Cart>(c => c.UserId) //forgin  key hwh userid
                .OnDelete(DeleteBehavior.Cascade);//iza mhit user btnmha cart

            // User → Orders
            builder.Entity<ApplicationUser>()
                .HasMany<Order>()
                .WithOne()//ma ktbna chi hn laenu ma endi navigation la ordeer bi alb applicationuserr
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);//iza mhi user msh ha ynmha order

            //an amh7adide 3le2a ben cart w cartitem bs bdi ul enu iza mhit cart tnmha cartite ma3a so mtra erjae3 ektb 3lea byntn
            builder.Entity<Cart>()
            .HasMany(c => c.CartItems)
            .WithOne(c => c.Cart)
            .HasForeignKey(c => c.CartId)
            .OnDelete(DeleteBehavior.Cascade);

            //order ordeitm
            builder .Entity<Order>()
            .HasMany(c=>c.OrderItems)//bktba hk iza kn endi collection
            .WithOne(c=>c.Order)
            .HasForeignKey(c=>c.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
           
            //product cartitem
            builder.Entity<Product>()
            .HasMany<CartItem>()
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

            //prodcut ordetirm
            builder.Entity<Product>()
            .HasMany<OrderItem>()
            .WithOne(c => c.Product)
            .HasForeignKey(c => c.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
            

            //category product
            builder.Entity<Category>()
            .HasMany(c => c.Products)
            .WithOne(c => c.Category)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CartItem>()
            .Property(c => c.ProductPrice)
            .HasPrecision(18, 2);//bst3mla mae  decimal krml haded abl w baed ,
            builder.Entity<OrderItem>()
            .Property(c => c.Price)
            .HasPrecision(18,2);

            builder.Entity<Order>()
            .Property(c=>c.TotalAmount) .HasPrecision(18,2);
            builder.Entity<Product>()
            .Property(c=>c.Price) .HasPrecision (18,2);

        }
       
    }

}
