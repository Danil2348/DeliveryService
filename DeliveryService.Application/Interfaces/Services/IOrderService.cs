using DeliveryService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeliveryService.Application.Interfaces.Services
{
    public interface IOrderService
    {
        List<Order> FilterOrders(string district, DateTime firstDeliveryDateTime);
        void SaveFilteredOrders(List<Order> filteredOrders, string outputFilePath);
        List<Order> LoadAllOrders();
    }
}
