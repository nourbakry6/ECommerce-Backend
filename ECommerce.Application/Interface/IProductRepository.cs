using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
namespace ECommerce.Application.Interface
{
     public interface IProductRepository
    {
      List<Product> GetAll();
        void Add(Product product);
        //? y3ni mumkn value ykun null
        Product? GetById(int id);
        void Update( Product product);
        void DeleteById(Product product);

    }
}
