using DeliveryService.ConsoleApp;
using DeliveryService.Core.Entities;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace DeliveryService.Tests
{
    public class OrderServiceTests : TestBase
    {
        [Fact]
        public void FilterAndSaveOrders_ShouldCallSaveFilteredOrders_WhenValidParameters()
        {
            var cityDistrict = "District";
            var firstDeliveryDateTime = DateTime.Now;

            var filteredOrders = new List<Order> { new Order() }; // Предположим, что Order — это ваш класс заказа.

            OrderServiceMock.Setup(service => service.FilterOrders(cityDistrict, firstDeliveryDateTime))
                .Returns(filteredOrders);

            var configurationMock = new Mock<IConfigurationRoot>();
            configurationMock.Setup(c => c["FilteredOrdersFilePath"]).Returns("test_output.txt");

            Program.FilterAndSaveOrders(configurationMock.Object, LoggerMock.Object, OrderServiceMock.Object, cityDistrict, firstDeliveryDateTime);

            OrderServiceMock.Verify(service => service.SaveFilteredOrders(filteredOrders, "test_output.txt"), Times.Once);
        }
    }
}