using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
namespace ECommerce.Application.Interface
{
     public interface IProductRepository
    {
        Task<List<Product>> GetAll();
        Task Add(Product product);
        Task<Product?> GetById(int id);
        Task Update(Product product);
        Task DeleteById(Product product);
    }
}
