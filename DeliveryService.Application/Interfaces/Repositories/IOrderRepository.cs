using DeliveryService.Core.Entities;
using System.Collections.Generic;

namespace DeliveryService.Application.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetOrders();
        void SaveOrders(List<Order> orders);
    }
}
