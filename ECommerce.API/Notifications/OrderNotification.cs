using ECommerce.API.Hubs;
using ECommerce.Application.Interface;
using Microsoft.AspNetCore.SignalR;
namespace ECommerce.API.Notifications
{
    public class OrderNotification : IOrderNotification
    {
        private readonly IHubContext<OrderHub> _hubcontext;//hubcontext bsmah la aya server b backend yhki mae clinet aemol connect a hub
        public OrderNotification(IHubContext <OrderHub> hubContext){
        _hubcontext = hubContext;
        }
        public  async Task SendOrderStatusUpdate(int userId, int orderId, string status)
        {
            await _hubcontext.Clients
                   .User(userId.ToString())//hdadna la ayaa client ha ttwda
                   .SendAsync(
                       "OrderStatusUpdated",//esm  event b react ba3dn bi ul iza wsl h event nfz he code..
                       orderId,//data l am nwdyn
                       status);
        }
    }
}
