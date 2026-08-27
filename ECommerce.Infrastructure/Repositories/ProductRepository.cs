using System;
using System.Collections.Generic;
using System.Text;
using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace ECommerce.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
          //bdna etisal a db hydi nskha an dbcontext
          private readonly ApplicationDbContext _context;
          public ProductRepository(ApplicationDbContext context){
          _context= context;
          }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

      

        public List<Product> GetAll()
        {
        //btruh a dbset taeit prodcut w btred klchi hka list mtl select * from prduct
            return _context.Products
            .Include(c=>c.Category)
            .ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products
            .Include(c=>c.Category)
            //first btred abl value byzbt chart mma3u bs iza m alaet btred exption so mnktb or default btrednull iza m laet
            .FirstOrDefault(p=>p.Id == id);
        }

        public void Update(Product product)
        {    _context.Products.Update(product);
            _context.SaveChanges();
        }
        public void DeleteById(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }
    }
}
