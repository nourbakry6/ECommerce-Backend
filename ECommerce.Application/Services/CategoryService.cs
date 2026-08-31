using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace ECommerce.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        private readonly  IDistributedCache  _Cache;
        public CategoryService(
            ICategoryRepository categoryRepository,
            IUnitOfWork unitOfWork,IDistributedCache distributedCache)
        {
            _categoryRepository = categoryRepository;
            _Cache = distributedCache;
        
        }

        public async Task Add(CategoryCreateDTO categoryDTO)
        {
            var category = new Category
            {
                Name = categoryDTO.Name
            };

            await _categoryRepository.Add(category);
            await _Cache.RemoveAsync("category");
            
        }

        public async Task<bool> Delete(int id)
        {
            var category = await _categoryRepository.GetById(id);

            if (category == null)
                throw new KeyNotFoundException(
          $"Category with ID {id} not found.");

            await _categoryRepository.Delete(category);
            await _Cache.RemoveAsync($"category:{id}");
            await _Cache.RemoveAsync("category");

            return true;
        }

        public async Task<List<CategoryDTO>> GetAll()
        {
            var cacheKey = "category";

            var cachedCategory = await _Cache.GetStringAsync(cacheKey);

            if (cachedCategory != null)
            {
                return JsonSerializer.Deserialize<List<CategoryDTO>>(cachedCategory)!;
            }

            var categories = await _categoryRepository.GetAll();

            var categoryDto = categories.Select(c => new CategoryDTO
            {
                Name = c.Name
            }).ToList();

            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };

            await _Cache.SetStringAsync(
                cacheKey,
                JsonSerializer.Serialize(categoryDto),
                option
            );

            return categoryDto;
        }

        public async Task<CategoryDetailsDTO?> GetById(int id)
        {
            var cachekey = $"category:{id}";
            var cachecategory=await _Cache.GetStringAsync(cachekey);
            if (cachecategory != null) {

             return   JsonSerializer.Deserialize<CategoryDetailsDTO>(cachecategory);
            }

            var category = await _categoryRepository.GetById(id);

            if (category == null)
                throw new KeyNotFoundException(
          $"Category with ID {id} not found.");

            var categoryDto= new CategoryDetailsDTO
            {
                id = category.Id,
                Name = category.Name,

                products = category.Products.Select(p => new ProductDTO
                {
                    Name = p.Name,
                    Price = p.Price
                }).ToList()
            };
            var option = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
            };
            await _Cache.SetStringAsync(
            cachekey, JsonSerializer.Serialize(categoryDto), option
            );
            return categoryDto;


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
           await _Cache.RemoveAsync($"category:{id}");
            await _Cache.RemoveAsync("category");

            return true;
        }
    }
}