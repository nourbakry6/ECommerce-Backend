using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using System;
using System.Collections.Generic;
using System.Text;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
    private readonly  ICategoryRepository _categoryRepository;
    public CategoryService(ICategoryRepository categoryRepository){
    _categoryRepository = categoryRepository;
    }

        public void Add(CategoryCreateDTO categoryDTO)
        {
            var category = new Category
            {
                Name = categoryDTO.Name
            };
            _categoryRepository.Add(category);

        }

     

        public bool Delete(int id)
        {
           var category=_categoryRepository.GetById(id);
           if(category==null)return false;
           _categoryRepository.Delete(category);

           return true;
        }

        public List<CategoryDTO> GetAll()
        {
            var category=_categoryRepository.GetAll();
            return category.Select(c => new CategoryDTO
            {
                Name = c.Name,
            }).ToList();
           
        }

        public CategoryDetailsDTO? GetById(int id)
        {
           var category=_categoryRepository.GetById(id);
            if (category == null) return null;
            return new CategoryDetailsDTO
            {
            id=category.Id,
                Name = category.Name,
                products= category.Products.Select(p => new ProductDTO
                {
                    Name = p.Name,
                    Price = p.Price
                }).ToList()

            };
        }

        public bool Update(int id, CategoryUpdateDTO categoryUpdateDTO)
        {
           var category=_categoryRepository.GetById(id);
           if(category == null) return false;
           category.Name=categoryUpdateDTO.Name;
           _categoryRepository.Update(category);
           return true;
        }
    }
}
