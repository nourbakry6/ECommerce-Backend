using ECommerce.Domain.entites;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Infrastructure.Data
{
    public class DbSeeder
    {
        public static async Task Seed(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            //amchuf iza role admin exit iza la baeml create
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(
                    new IdentityRole<int>("Customer")
                );
            }

            //am chuf iza email mwjud iza ee ma b3ml chi iza la baeml create
            var userr = await userManager.FindByEmailAsync("nour@gmail.com");
            if (userr != null) return;

            var user = new ApplicationUser
            {
                UserName = "nour",
                Email = "nour@gmail.com",
                CreatedAt = DateTime.UtcNow,
            };
            //krml ma n3mol PasswordHash = BCrypt.Net.BCrypt.HashPassword("nour")
            //nnhan bi idna bs b asp.net identity mfi dei lan endi creatasync hwh by3mlu hash lal password w bihtu b paswordhash
            var pass = await userManager.CreateAsync(user, "Riri2005.");
            //user taeit applicationuser ha yruh a passwordhash yhut nour hashing
            if (!pass.Succeeded)
            {
                throw new Exception(
                    string.Join(", ",
                        pass.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(user,"Admin");


        }



    }
}
