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

        void Add(ProductCreateDTO productDTO);
        List<ProductDTO> GetAll();
        ProductDTO? GetById(int id);
        bool Update(int id,ProductUpdateDTO productUpdate);
        bool DeleteById(int id);

    }
}
