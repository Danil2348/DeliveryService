using DeliveryService.Core.Entities;
using System;
using System.Collections.Generic;

namespace DeliveryService.Application.Interfaces.Services
{
    public interface IOrderService
    {
        List<Order> FilterOrders(string district, DateTime firstDeliveryDateTime);
        void SaveFilteredOrders(List<Order> filteredOrders, string outputFilePath);
        List<Order> LoadAllOrders();
    }
}
