using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
     public interface IUserServercs
    {
        Task<bool> Register(RegisterDTO registerDTO);
     
        Task<List<UserDTO>> GetAll();
       
        Task<bool> Update(int id,UserUpdateDTO userUpdateDTO);
        Task<bool> Delete(int id);
        Task<UserDTO?> GetById(int id);
        Task<(string? Token, bool Success, string? Error)> Login(LoginDTO loginDTO);
    }
}
