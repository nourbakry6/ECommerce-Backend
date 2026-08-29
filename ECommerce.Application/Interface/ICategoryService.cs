using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTO;
namespace ECommerce.Application.Interface
{
    public interface ICategoryService
    {

        Task<List<CategoryDTO>> GetAll();

        Task Add(CategoryCreateDTO categoryDTO);

        Task<bool> Update(int id, CategoryUpdateDTO categoryUpdateDTO);

        Task<bool> Delete(int id);

        Task<CategoryDetailsDTO?> GetById(int id);
    }
}
