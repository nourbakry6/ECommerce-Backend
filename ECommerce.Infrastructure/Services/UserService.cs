using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ECommerce.Infrastructure.Services
{
    public class UserService : IUserServercs
    {
        private readonly UserManager<ApplicationUser> _userManager;
        //usermanger b2dr ethkm bhl identityuser a3mol CreateAsyn,FindByIdAsync,.FindByEmailAsync,CheckPasswordAsync(ni karen pas l dkhlne mae pass mwjud),AddToRoleAsync,GetRolesAsync,UpdateAsync.ChangePasswordAsync,
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IJwtService _jwtService;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public UserService( SignInManager<ApplicationUser> signInManager,ApplicationDbContext applicationDbContext,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager,IJwtService jwtService){
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtService = jwtService;
        _context = applicationDbContext;
        _signInManager= signInManager;
        }

        public async Task<bool> Delete(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user == null)return false;
            user.IsActive = false;
             var result=await _userManager.UpdateAsync(user);
            return result.Succeeded;

        }

        public async Task<List<UserDTO>> GetAll()
        {
            var user =await _userManager.Users.ToListAsync();
            //var result = user.Select(user => new UserDTO
            //{
            //    Id = user.Id,
            //    Username = user.UserName!,
            //    Email = user.Email!
            //}).ToList();bs mfina nktb b chk l adi l await t3it role laenu selct msh msamameh hk
            var list = new List<UserDTO>();
            foreach(var u in user){
                var role = await _userManager.GetRolesAsync(u);
                list.Add(new UserDTO
                {
                    Id = u.Id,
                    Username = u.UserName!,//! ya3ni msh null
                    Email = u.Email!,
                    Role = role.FirstOrDefault() ?? "Customer",
                    IsActive = u.IsActive
                });
            }return list;

        }

        public async Task<UserDTO?> GetById(int id)
        {
            var user=await _userManager.FindByIdAsync(id.ToString());
            if(user == null) return null;
            var role= await _userManager.GetRolesAsync(user);
            return new UserDTO
            {
                Id = id,
                Username = user.UserName!,
                Email = user.Email!,
                Role = role.FirstOrDefault() ?? "Customer",
                IsActive = user.IsActive

            };
        }
        //bdi trdli ntije iza njhe aw lae w error mesg so hydi mniha krml aerf chu error 
        public async Task<(string? Token, bool Success, string? Error)> Login(LoginDTO loginDTO)
        {
            var user=await _userManager.FindByEmailAsync(loginDTO.Email);
            if(user == null ) return (null,false, "Email does not exist.");
            if (!user.IsActive)
                return (null, false, "Account is deactivated.");
            //var pass = await _signInManager.PasswordSignInAsync(user.UserName,loginDTO.Password, false//hydi persistent login y3ni iza htyta true bisir prg byb2a zkr neu h user aeml login ma bytr kl ma yfut y3mol
            //, true //lockoutOnFailure htyta trrue ya3ni kl mhwle ghlt yhsba mhwle dmn lockout
            //);

            //if (pass.IsLockedOut)
            //{
            //    return (null, false, "Account is locked,try after 5 min.");
            //}

            //if (!pass.Succeeded)
            //{
            //    return (null, false, "Password is incorrect.");
            //}
        
            if (await _userManager.IsLockedOutAsync(user))
            {
                return (null, false, "Account is locked.");
            }
            var validPassword = await _userManager.CheckPasswordAsync(
                user,
                loginDTO.Password);

            if (!validPassword)
            {
                await _userManager.AccessFailedAsync(user);//hydi btzid mhwlt iza ghlt bnhsb muhwle mnl lockout

                if (await _userManager.IsLockedOutAsync(user))
                    return (null, false, "Account is locked.");

                return (null, false, "Password is incorrect.");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var token=_jwtService.GEtToken(user.Id,user.Email!,roles,user.UserName!);
            return (token, true, null);
        } 


    public async Task<bool> Register(RegisterDTO registerDTO)
        {
            var user = new ApplicationUser { 
            UserName=registerDTO.Username,
            Email=registerDTO.Email,
            CreatedAt=DateTime.UtcNow
            };
            //createasync hwh l by3ml create lal user bfdtabs mae hashig lal pass w by3ml validTION Iza klu tmm bi kun succeded
            var result = await _userManager.CreateAsync(user, registerDTO.Password);

    if (!result.Succeeded)
    {
        var errors = string.Join(
            " | ",
            result.Errors.Select(e => e.Description)
        );

        throw new Exception(errors);
    }
            await _userManager.AddToRoleAsync(user, "Customer");

            var cart = new Cart
            {
                UserId = user.Id
            };

            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
            return result.Succeeded;

        }

        public async Task<bool> Update(int id, UserUpdateDTO userUpdateDTO)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if(user == null) return false;
            user.Email=userUpdateDTO.Email;
            user.UserName=userUpdateDTO.Username;
            
           var result= await _userManager.UpdateAsync(user);
            return result.Succeeded;
        }
    }
}
