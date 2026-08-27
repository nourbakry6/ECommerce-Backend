using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Interface
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        void Add(Category category) ;
        void Update(Category category);
        void Delete(Category category);
        Category? GetById(int id);
         

        
    }
}
