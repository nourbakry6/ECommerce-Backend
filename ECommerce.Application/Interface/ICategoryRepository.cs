using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetAll();

        Task Add(Category category);

        Task Update(Category category);

        Task Delete(Category category);

        Task<Category?> GetById(int id);

    }
}
