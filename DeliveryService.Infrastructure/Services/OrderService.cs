using DeliveryService.Application.Interfaces.Repositories;
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

        public List<Order> FilterOrders(string district, DateTime firstDeliveryDateTime)
        {

            var orders = _orderRepository.GetOrders();
            var endTime = firstDeliveryDateTime.AddMinutes(30);

            return orders.Where(o => o.District == district &&
                                      o.DeliveryTime >= firstDeliveryDateTime &&
                                      o.DeliveryTime <= endTime)
                         .Select(o => new Order
                         {
                             OrderId = o.OrderId,
                             Weight = o.Weight,
                             District = o.District,
                             DeliveryTime = o.DeliveryTime
                         }).ToList();
        }

        public void SaveFilteredOrders(List<Order> filteredOrders, string outputFilePath)
        {
            var ordersToSave = filteredOrders.Select(o => new Order
            {
                OrderId = o.OrderId,
                Weight = o.Weight,
                District = o.District,
                DeliveryTime = o.DeliveryTime
            }).ToList();

            _orderRepository.SaveOrders(ordersToSave);
        }

        public List<Order> LoadAllOrders()
        {
            var orders = _orderRepository.GetOrders();
            return orders.Select(o => new Order
            {
                OrderId = o.OrderId,
                Weight = o.Weight,
                District = o.District,
                DeliveryTime = o.DeliveryTime
            }).ToList();
        }
    }
}