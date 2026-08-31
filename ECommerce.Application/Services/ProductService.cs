using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using Microsoft.Extensions.Caching.Distributed;
namespace ECommerce.Application.Services;

using System.Text.Json;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDistributedCache _cache;// interfacej hez b asp.net core bi khali ytaeml mae cache khrji mtl redis
    public ProductService(IProductRepository productRepository,IUnitOfWork unitOfWork,IDistributedCache distributedCache) 
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _cache= distributedCache;
    }

    public async Task Add(ProductCreateDTO productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Stock = productDto.Stock,
            ImageUrl = productDto.ImageUrl,
            CategoryId = productDto.CategoryId
        };

        await _productRepository.Add(product);
        await _cache.RemoveAsync("products");
    }

    public async Task<List<ProductDTO>> GetAll()
    {
        var cachekey = "products";
        var cachedproduct= await _cache.GetStringAsync(cachekey);
        if (cachedproduct != null) {

        //ba3mol deserialize krml rdn object
            return JsonSerializer.Deserialize < List < ProductDTO >> (cachedproduct)!;
        }

        var products = await _productRepository.GetAll();

        var productDto= products.Select(c => new ProductDTO
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Price = c.Price,
            Stock = c.Stock,
            ImageUrl = c.ImageUrl,
            Categoryname = c.Category.Name
        }).ToList();
        var option = new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow=TimeSpan.FromMinutes(10)};
        await _cache.SetStringAsync(
        //waeta bdi ht dat ab redis ba3ml serialize bhwln l ajson
        cachekey, JsonSerializer.Serialize(productDto), option
        );
        return productDto;

    }

    public async Task<ProductDTO?> GetById(int id)
    {
        var cacheKey = $"product:{id}";

        var cachedProduct = await _cache.GetStringAsync(cacheKey);

        if (cachedProduct != null)
        {
            return JsonSerializer.Deserialize<ProductDTO>(cachedProduct);
        }
        var  product = await _productRepository.GetById(id);

        if (product == null)
            throw new KeyNotFoundException(
              $"Product with ID {id} not found.");

        var productDto =new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            Categoryname = product.Category.Name
        };
        var option = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
        };
        await _cache.SetStringAsync(
        cacheKey,JsonSerializer.Serialize(productDto), option) ;
        return productDto;
    }

    public async Task<bool> Update(int id, ProductUpdateDTO productDTO)
    {
        var product = await _productRepository.GetById(id);

        if (product == null)
            throw new KeyNotFoundException(
          $"Product with ID {id} not found.");

        product.Name = productDTO.Name;
        product.Description = productDTO.Description;
        product.Price = productDTO.Price;
        product.Stock = productDTO.Stock;
        product.ImageUrl = productDTO.ImageUrl;
        product.CategoryId = productDTO.CategoryId;

        await _productRepository.Update(product);
        await _unitOfWork.SaveChangesAsync();
        await _cache.RemoveAsync($"product:{id}");
        await _cache.RemoveAsync($"products");
        return true;
    }

    public async Task<bool> DeleteById(int id)
    {
        var product = await _productRepository.GetById(id);

        if (product == null)
            throw new KeyNotFoundException(
          $"Product with ID {id} not found.");

        await _productRepository.DeleteById(product);
        await _cache.RemoveAsync($"product:{id}");
        await _cache.RemoveAsync($"products");
        return true;
    }
}