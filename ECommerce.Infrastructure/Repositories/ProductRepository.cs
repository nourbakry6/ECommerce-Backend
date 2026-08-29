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

        public async Task Add(Product product)
        {
             await _context.Products.AddAsync(product);
             await _context.SaveChangesAsync();
        }

      

        public async Task<List<Product>> GetAll()
        {
        //btruh a dbset taeit prodcut w btred klchi hka list mtl select * from prduct
            return await  _context.Products
            .Include(c=>c.Category)
            .ToListAsync();
        }

        public async Task<Product?> GetById(int id)
        {
            return await  _context.Products
            .Include(c=>c.Category)
            //first btred abl value byzbt chart mma3u bs iza m alaet btred exption so mnktb or default btrednull iza m laet
            .FirstOrDefaultAsync(p=>p.Id == id);
        }

        public   Task Update(Product product)
        {    _context.Products.Update(product);
            return Task.CompletedTask;
        }
        public async Task DeleteById(Product product)
        {
            _context.Products.Remove(product);
           await  _context.SaveChangesAsync();
        }
    }
}
