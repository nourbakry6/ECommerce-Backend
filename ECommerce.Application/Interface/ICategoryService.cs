using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTO;
namespace ECommerce.Application.Interface
{
    public interface ICategoryService
    {
        List<CategoryDTO> GetAll();
        void Add(CategoryCreateDTO categoryDTO);
        bool Update(int id, CategoryUpdateDTO categoryUpdateDTO);
        bool Delete(int id);
        CategoryDetailsDTO? GetById(int id);
    }
}
