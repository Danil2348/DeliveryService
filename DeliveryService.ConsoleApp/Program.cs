using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using DeliveryService.Application.Interfaces.Repositories;
using DeliveryService.Application.Interfaces.Services;
using DeliveryService.Application.Services;
using DeliveryService.Infrastructure.FileStorage;

namespace DeliveryService.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Убедитесь, что папка для логов существует
            string logDirectory = "logs";
            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            // Настройка логирования с использованием Serilog
            string logFilePath = Path.Combine(logDirectory, "delivery.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console() // Логирование в консоль
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day) // Логирование в файл
                .CreateLogger();

            // Создание конфигурации
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var serviceProvider = ConfigureServices(configuration); // Передаем конфигурацию в метод
            var logger = serviceProvider.GetRequiredService<ILogger<Program>>();
            var orderService = serviceProvider.GetRequiredService<IOrderService>();

            if (args.Length < 2)
            {
                logger.LogError("Необходимо указать район и время первой доставки.");
                return;
            }

            string cityDistrict = args[0];
            if (!DateTime.TryParse(args[1], out DateTime firstDeliveryDateTime))
            {
                logger.LogError("Некорректный формат времени. Убедитесь, что время указано в формате: гггг-ММ-дд ЧЧ:мм:сс.");
                return;
            }

            try
            {
                var filteredOrders = orderService.FilterOrders(cityDistrict, firstDeliveryDateTime);

                var outputFilePath = configuration["FilteredOrdersFilePath"];

                if (!File.Exists(outputFilePath))
                {
                    using (File.Create(outputFilePath)) { }
                }

                orderService.SaveFilteredOrders(filteredOrders, outputFilePath);

                logger.LogInformation($"Отфильтрованные заказы сохранены в {outputFilePath}.");
            }
            catch (Exception ex)
            {
                logger.LogError($"Произошла ошибка при обработке заказов: {ex.Message}");
            }
            finally
            {
                Log.CloseAndFlush(); // Закрытие логирования при завершении приложения
            }
        }

        private static ServiceProvider ConfigureServices(IConfiguration configuration) // Принимаем IConfiguration как параметр
        {
            return new ServiceCollection()
                .AddLogging(builder =>
                {
                    builder.AddSerilog();
                    builder.SetMinimumLevel(LogLevel.Debug);
                })
                .AddSingleton<IOrderRepository>(new OrderRepository(configuration["OrdersFilePath"], configuration["FilteredOrdersFilePath"]))
                .AddTransient<IOrderService, OrderService>()
                .BuildServiceProvider();
        }
    }
}