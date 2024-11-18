using DeliveryService.Domain.Interfaces.Repositories;
using DeliveryService.Application.Interfaces.Services;
using DeliveryService.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DeliveryService.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public List<Order> FilterOrders(string district, DateTime firstDeliveryDateTime, List<Order> orders)
        {
            var endTime = firstDeliveryDateTime.AddMinutes(30);
            var filteredOrders = orders
                .Where(o => o.District == district
                      && o.DeliveryTime >= firstDeliveryDateTime
                      && o.DeliveryTime <= endTime)
                .ToList();
            return filteredOrders;
        }

        public void SaveFilteredOrders(List<Order> filteredOrders, string outputFilePath)
        {
            var ordersToSave = filteredOrders.ToList();
            _orderRepository.SaveOrders(ordersToSave);
        }

        public List<Order> LoadAllOrders()
        {
            var orders = _orderRepository.GetOrders();
            return orders;
        }
    }
}