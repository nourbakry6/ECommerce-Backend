using ECommerce.Application.DTO;
using ECommerce.Application.DTO;
using ECommerce.Application.Interface;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Domain.entites;

namespace ECommerce.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public void Add(ProductCreateDTO productDto)
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

        _productRepository.Add(product);
    }

    

    public List<ProductDTO> GetAll()
    {//ba3ml select mae list
        var products=_productRepository.GetAll();
        return products.Select(c => new ProductDTO { 
        Name=c.Name,
        Description=c.Description,
        Price=c.Price,
        Stock=c.Stock,
        ImageUrl=c.ImageUrl,
        Categoryname = c.Category.Name,
       Id=c.Id
        
        
        }).ToList();


    }


    public ProductDTO? GetById(int id)
    { var product =_productRepository.GetById(id);
    if(product == null)return null;
        return new ProductDTO {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Stock = product.Stock,
            ImageUrl = product.ImageUrl,
            Categoryname = product.Category.Name,
         

        };


    }

    public bool Update(int id, ProductUpdateDTO productDTO)
    {
        var product = _productRepository.GetById(id);
      if(product == null) return false ;
      product.Name = productDTO.Name;
      product.Description = productDTO.Description;
      product.Price= productDTO.Price;
      product.Stock= productDTO.Stock;
      product.ImageUrl = productDTO.ImageUrl;
      product.CategoryId= productDTO.CategoryId;
        _productRepository.Update(product);
        return true;
    }
    public bool DeleteById(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null) return false;
        _productRepository.DeleteById(product);
        return true;
    }
}