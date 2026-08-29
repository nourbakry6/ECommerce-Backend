using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using ECommerce.Application.DTO;
using ECommerce.Domain.entites;
namespace ECommerce.Application.Interface
{
   public interface IProductService
    {

        Task Add(ProductCreateDTO productDTO);
        Task<List<ProductDTO>> GetAll();
        Task<ProductDTO?> GetById(int id);
        Task<bool> Update(int id, ProductUpdateDTO productUpdate);
        Task<bool> DeleteById(int id);

    }
}
