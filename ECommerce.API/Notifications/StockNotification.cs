using ECommerce.API.Hubs;
using ECommerce.Application.Interface;
using Microsoft.AspNetCore.SignalR;
namespace ECommerce.API.Notifications
{
    public class StockNotification : IStockNotification
    {
        private readonly IHubContext<StockHub> _hubContext;
        StockNotification(IHubContext<StockHub> hubContext){
        _hubContext = hubContext;
        }
        public async Task SendStockUpdate(int productId, int stock)
        {
           await _hubContext.Clients
           .All//la kl customer l connect mae stockhub ha yusl l update
           .SendAsync("StockUpdated", productId, stock);
        }
    }
}
