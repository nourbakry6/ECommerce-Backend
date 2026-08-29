using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
       

        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
        
        }

        public async Task Add(CategoryCreateDTO categoryDTO)
        {
            var category = new Category
            {
                Name = categoryDTO.Name
            };

            await _categoryRepository.Add(category);
            
        }

        public async Task<bool> Delete(int id)
        {
            var category = await _categoryRepository.GetById(id);

            if (category == null)
                throw new KeyNotFoundException(
          $"Category with ID {id} not found.");

            await _categoryRepository.Delete(category);
         

            return true;
        }

        public async Task<List<CategoryDTO>> GetAll()
        {
            var categories = await _categoryRepository.GetAll();

            return categories.Select(c => new CategoryDTO
            {
                Name = c.Name
            }).ToList();
        }

        public async Task<CategoryDetailsDTO?> GetById(int id)
        {
            var category = await _categoryRepository.GetById(id);

            if (category == null)
                throw new KeyNotFoundException(
          $"Category with ID {id} not found.");

            return new CategoryDetailsDTO
            {
                id = category.Id,
                Name = category.Name,

                products = category.Products.Select(p => new ProductDTO
                {
                    Name = p.Name,
                    Price = p.Price
                }).ToList()
            };
        }

        public async Task<bool> Update(
            int id,
            CategoryUpdateDTO categoryUpdateDTO)
        {
            var category = await _categoryRepository.GetById(id);

            if (category == null)
                throw new KeyNotFoundException(
          $"Category with ID {id} not found.");

            category.Name = categoryUpdateDTO.Name;

            await _categoryRepository.Update(category);
           

            return true;
        }
    }
}