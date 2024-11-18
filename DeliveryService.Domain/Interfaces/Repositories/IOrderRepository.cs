using DeliveryService.Core.Entities;
using System.Collections.Generic;

namespace DeliveryService.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        List<Order> GetOrders();
        void SaveOrders(List<Order> orders);
    }
}
