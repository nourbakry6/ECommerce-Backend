using ECommerce.Application.Interface;
using ECommerce.Domain.entites;
using ECommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _Context;

        public OrderRepository(ApplicationDbContext applicationDbContext)
        {
            _Context = applicationDbContext;
        }

        public  Task AddOrder(Order order)
        {
            _Context.Orders.Add(order);
            return Task.CompletedTask;
        }

        public async Task CancelOrder(Order order)
        {
            _Context.Orders.Update(order);
            await _Context.SaveChangesAsync();
        }

        public async Task<List<Order>> GetAllOrder()
        {
            return await _Context.Orders
                .Include(p => p.OrderItems)
                .ThenInclude(p => p.Product)
                .ToListAsync();
        }

        public async Task<Order?> GetByOrderId(int orderid)
        {
            return await _Context.Orders
                .Include(p => p.OrderItems)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(p => p.Id == orderid);
        }

        public async Task<List<Order>> GetMyOrders(int userId)
        {
            return await _Context.Orders
                .Include(p => p.OrderItems)
                .ThenInclude(p => p.Product)
                .Where(p => p.UserId == userId)
                .ToListAsync();
        }

        public async Task UpdateOrder(Order order)
        {
            _Context.Orders.Update(order);
            await _Context.SaveChangesAsync();
        }
    }
}