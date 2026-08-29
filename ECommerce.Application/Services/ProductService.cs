using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;

namespace ECommerce.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    public ProductService(IProductRepository productRepository,IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
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
    }

    public async Task<List<ProductDTO>> GetAll()
    {
        var products = await _productRepository.GetAll();

        return products.Select(c => new ProductDTO
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            Price = c.Price,
            Stock = c.Stock,
            ImageUrl = c.ImageUrl,
            Categoryname = c.Category.Name
        }).ToList();
    }

    public async Task<ProductDTO?> GetById(int id)
    {
        var product = await _productRepository.GetById(id);

        if (product == null)
            throw new KeyNotFoundException(
              $"Product with ID {id} not found.");

        return new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            Categoryname = product.Category.Name
        };
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
        return true;
    }

    public async Task<bool> DeleteById(int id)
    {
        var product = await _productRepository.GetById(id);

        if (product == null)
            throw new KeyNotFoundException(
          $"Product with ID {id} not found.");

        await _productRepository.DeleteById(product);

        return true;
    }
}