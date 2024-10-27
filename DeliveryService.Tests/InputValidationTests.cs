using DeliveryService.ConsoleApp;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using Xunit;

namespace DeliveryService.Tests
{
    public class InputValidationTests : TestBase
    {
        [Fact]
        public void ValidateInputArguments_ShouldLogError_WhenArgsAreLessThanTwo()
        {
            var args = new string[] { "District" };

            var result = Program.ValidateInputArguments(args, LoggerMock.Object, out var cityDistrict, out var firstDeliveryDateTime);

            Assert.False(result);
            Assert.Null(cityDistrict);
            Assert.Equal(default(DateTime), firstDeliveryDateTime);

            LoggerMock.Verify(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => string.Equals("Необходимо указать район и время первой доставки.", v.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public void ValidateInputArguments_ShouldReturnFalse_WhenDateIsInvalid()
        {
            var args = new string[] { "District", "InvalidDate" };

            var result = Program.ValidateInputArguments(args, LoggerMock.Object, out var cityDistrict, out var firstDeliveryDateTime);

            Assert.False(result);
            Assert.Equal("District", cityDistrict);
            Assert.Equal(default(DateTime), firstDeliveryDateTime);

            LoggerMock.Verify(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => string.Equals("Некорректный формат времени. Убедитесь, что время указано в формате: гггг-ММ-дд ЧЧ:мм:сс.", v.ToString(), StringComparison.InvariantCultureIgnoreCase)),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }
    }
}
