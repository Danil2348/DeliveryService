using DeliveryService.Application.Interfaces.Services;
using DeliveryService.ConsoleApp;
using Microsoft.Extensions.Logging;
using Moq;

public abstract class TestBase
{
    protected readonly Mock<ILogger<Program>> LoggerMock;
    protected readonly Mock<IOrderService> OrderServiceMock;

    protected TestBase()
    {
        LoggerMock = new Mock<ILogger<Program>>();
        OrderServiceMock = new Mock<IOrderService>();
    }
}